using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace GitignoreParserNet
{
    public sealed class GitignoreParser
    {
        private readonly (Regex, Regex[]) Positives;
        private readonly (Regex, Regex[]) Negatives;

        public GitignoreParser(string content)
        {
            var parsed = Parse(content);
            Positives = parsed.positives;
            Negatives = parsed.negatives;
        }

        public static ((Regex, Regex[]) positives, (Regex, Regex[]) negatives) Parse(string content)
        {
            (List<string> positive, List<string> negative) parsed = content
                .Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
                .Select(line => line.Trim())
                .Where(line => !string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
                .Aggregate<string, (List<string>, List<string>), (List<string>, List<string>)>(
                    (new List<string>(), new List<string>()),
                    ((List<string> positive, List<string> negative) lists, string line) =>
                    {
                        if (line.StartsWith("!"))
                            lists.negative.Add(line.Substring(1));
                        else
                            lists.positive.Add(line);
                        return (lists.positive, lists.negative);
                    },
                    ((List<string> positive, List<string> negative) lists) => lists
                );
            (Regex, Regex[]) submatch(List<string> list)
            {
                if (list.Count() == 0)
                {
                    return (new Regex("$^"), new Regex[0]);
                }
                else
                {
                    var reList = list.OrderBy(str => str).Select(PrepareRegexPattern).ToList();
                    return (
                        new Regex($"(?:{string.Join(")|(?:", reList)})"),
                        reList.Select(s => new Regex(s)).ToArray()
                    );
                }
            }
            return (submatch(parsed.positive), submatch(parsed.negative));
        }

        /// Return TRUE when the given `input` path PASSES the gitignore filters,
        /// i.e. when the given input path is DENIED.
        ///
        /// Notes:
        /// - you MUST postfix a input directory with '/' to ensure the gitignore
        ///   rules can be applied conform spec.
        /// - you MAY prefix a input directory with '/' when that directory is
        ///   'rooted' in the same directory as the compiled .gitignore spec file.
#if DEBUG
        public bool Accepts(string input, bool? expected = null)
#else
        public bool Accepts(string input)
#endif
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                input = input.Replace('\\', '/');

            if (!input.StartsWith("/"))
                input = "/" + input;

            var acceptRe = Negatives.Item1;
            var acceptTest = acceptRe.IsMatch(input);
            var denyRe = Positives.Item1;
            var denyTest = denyRe.IsMatch(input);
            var returnVal = acceptTest || !denyTest;
            // See the test/fixtures/gitignore.manpage.txt near line 680 (grep for "uber-nasty"):
            // to resolve chained rules which reject, then accept, we need to establish
            // the precedence of both accept and reject parts of the compiled gitignore by
            // comparing match lengths.
            // Since the generated consolidated regexes are lazy, we must loop through all lines' regexes instead:
            Match acceptMatch = null, denyMatch = null;
            if (acceptTest && denyTest)
            {
                foreach (var re in Negatives.Item2)
                {
                    var m = re.Match(input);
                    if (m.Success)
                    {
                        if (acceptMatch?.Success != true)
                            acceptMatch = m;
                        else if (acceptMatch.Groups[0].Value.Length < m.Value.Length)
                            acceptMatch = m;
                    }
                }
                foreach (var re in Positives.Item2)
                {
                    var m = re.Match(input);
                    if (m.Success)
                    {
                        if (denyMatch?.Success != true)
                            denyMatch = m;
                        else if (denyMatch.Groups[0].Value.Length < m.Value.Length)
                            denyMatch = m;
                    }
                }
                // acceptMatch = acceptRe.Match(input);
                // denyMatch = denyRe.Match(input);
                returnVal = acceptMatch.Groups[0].Value.Length >= denyMatch.Groups[0].Value.Length;
            }
#if DEBUG
            if (expected != null && expected != returnVal)
                Diagnose(
                    "accepts",
                    input,
                    (bool)expected,
                    acceptRe,
                    acceptTest,
                    acceptMatch,
                    denyRe,
                    denyTest,
                    denyMatch,
                    "(Accept || !Deny)",
                    returnVal
                );
#endif
            return returnVal;
        }

        /// Return TRUE when the given `input` path FAILS the gitignore filters,
        /// i.e. when the given input path is ACCEPTED.
        ///
        /// Notes:
        /// - you MUST postfix a input directory with '/' to ensure the gitignore
        ///   rules can be applied conform spec.
        /// - you MAY prefix a input directory with '/' when that directory is
        ///   'rooted' in the same directory as the compiled .gitignore spec file.
#if DEBUG
        public bool Denies(string input, bool? expected = null)
#else
        public bool Denies(string input)
#endif
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                input = input.Replace('\\', '/');

            if (!input.StartsWith("/"))
                input = "/" + input;

            var acceptRe = Negatives.Item1;
            var acceptTest = acceptRe.IsMatch(input);
            var denyRe = Positives.Item1;
            var denyTest = denyRe.IsMatch(input);
            // boolean logic:
            //
            // Denies = !Accepts =>
            // Denies = !(Accept || !Deny) =>
            // Denies = (!Accept && !!Deny) =>
            // Denies = (!Accept && Deny)
            var returnVal = !acceptTest && denyTest;
            // See the test/fixtures/gitignore.manpage.txt near line 680 (grep for "uber-nasty"):
            // to resolve chained rules which reject, then accept, we need to establish
            // the precedence of both accept and reject parts of the compiled gitignore by
            // comparing match lengths.
            // Since the generated regexes are all set up to be GREEDY, we can use the
            // consolidated regex for this, instead of having to loop through all lines' regexes:
            Match acceptMatch = null, denyMatch = null;
            if (acceptTest && denyTest)
            {
                foreach (var re in Negatives.Item2)
                {
                    var m = re.Match(input);
                    if (m.Success)
                    {
                        if (acceptMatch?.Success != true)
                            acceptMatch = m;
                        else if (acceptMatch.Groups[0].Value.Length < m.Value.Length)
                            acceptMatch = m;
                    }
                }
                foreach (var re in Positives.Item2)
                {
                    var m = re.Match(input);
                    if (m.Success)
                    {
                        if (denyMatch?.Success != true)
                            denyMatch = m;
                        else if (denyMatch.Groups[0].Value.Length < m.Value.Length)
                            denyMatch = m;
                    }
                }
                // acceptMatch = acceptRe.Match(input);
                // denyMatch = denyRe.Match(input);
                // boolean logic: !(A>=B) => A<B
                returnVal = acceptMatch.Groups[0].Value.Length < denyMatch.Groups[0].Value.Length;
            }
#if DEBUG
            if (expected != null && expected != returnVal)
                Diagnose(
                    "denies",
                    input,
                    (bool)expected,
                    acceptRe,
                    acceptTest,
                    acceptMatch,
                    denyRe,
                    denyTest,
                    denyMatch,
                    "(!Accept && Deny)",
                    returnVal
                );
#endif
            return returnVal;
        }

        /// Return TRUE when the given `input` path is inspected by any .gitignore
        /// filter line.
        ///
        /// You can use this method to help construct the decision path when you
        /// process nested .gitignore files: .gitignore filters in subdirectories
        /// MAY override parent .gitignore filters only when there's actually ANY
        /// filter in the child .gitignore after all.
        ///
        /// Notes:
        /// - you MUST postfix a input directory with '/' to ensure the gitignore
        ///   rules can be applied conform spec.
        /// - you MAY prefix a input directory with '/' when that directory is
        ///   'rooted' in the same directory as the compiled .gitignore spec file.
#if DEBUG
        public bool Inspects(string input, bool? expected = null)
#else
        public bool Inspects(string input)
#endif
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                input = input.Replace('\\', '/');

            if (!input.StartsWith("/"))
                input = "/" + input;

            var acceptRe = Negatives.Item1;
            var acceptTest = acceptRe.IsMatch(input);
            var denyRe = Positives.Item1;
            var denyTest = denyRe.IsMatch(input);
            // when any filter 'touches' the input path, it must match,
            // no matter whether it's a deny or accept filter line:
            var returnVal = acceptTest || denyTest;
#if DEBUG
            if (expected != null && expected != returnVal)
                Diagnose(
                    "inspects",
                    input,
                    (bool)expected,
                    acceptRe,
                    acceptTest,
                    null,
                    denyRe,
                    denyTest,
                    null,
                    "(Accept || Deny)",
                    returnVal
                );
#endif
            return returnVal;
        }

        private static readonly Regex RangeRegex = new Regex(@"^((?:[^\[\\]|(?:\\.))*)\[((?:[^\]\\]|(?:\\.))*)\]");

        private static string PrepareRegexPattern(string pattern)
        {
            // https://git-scm.com/docs/gitignore#_pattern_format
            //
            // * ...
            //
            // * If there is a separator at the beginning or middle (or both) of the pattern,
            //   then the pattern is relative to the directory level of the particular
            //   .gitignore file itself.
            //   Otherwise the pattern may also match at any level below the .gitignore level.
            //
            // * ...
            //
            // * For example, a pattern `doc/frotz/` matches `doc/frotz` directory, but
            //   not `a/doc/frotz` directory; however `frotz/` matches `frotz` and `a/frotz`
            //   that is a directory (all paths are relative from the .gitignore file).
            //
#if DEBUG
            string input = pattern;
#endif
            string re = "";
            bool rooted = false, directory = false;
            if (pattern.StartsWith("/"))
            {
                rooted = true;
                pattern = pattern.Substring(1);
            }
            if (pattern.EndsWith("/"))
            {
                directory = true;
                pattern = pattern.Substring(0, pattern.Length - 1);
            }

            string transpileRegexPart(string _re)
            {
                // unescape for these will be escaped again in the subsequent `.Replace(...)`,
                // whether they were escaped before or not:
                _re = Regex.Replace(_re, @"\\(.)", "$1");
                // escape special regex characters:
                _re = Regex.Replace(_re, @"[\-\[\]\{\}\(\)\+\.\\\^\$\|]", @"\$&");
                _re = Regex.Replace(_re, @"\?", "[^/]");
                _re = Regex.Replace(_re, @"\/\*\*\/", "(?:/|(?:/.+/))");
                _re = Regex.Replace(_re, @"^\*\*\/", "(?:|(?:.+/))");
                _re = Regex.Replace(_re, @"\/\*\*$", m =>
                {
                    directory = true;       // `a/**` should match `a/`, `a/b/` and `a/b`, the latter by implication of matching directory `a/`
                    return "(?:|(?:/.+))";  // `a/**` also accepts `a/` itself
                });
                _re = Regex.Replace(_re, @"\*\*", ".*");
                // `a/*` should match `a/b` and `a/b/` but NOT `a` or `a/`
                // meanwhile, `a/*/` should match `a/b/` and `a/b/c` but NOT `a` or `a/` or `a/b`
                _re = Regex.Replace(_re, @"\/\*(\/|$)", "/[^/]+$1");
                _re = Regex.Replace(_re, @"\*", "[^/]*");
                _re = Regex.Replace(_re, @"\/", @"\/");
                return _re;
            }

            // keep character ranges intact:
            Regex rangeRe = RangeRegex;
            // ^ could have used the 'y' sticky flag, but there's some trouble with infinite loops inside
            //   the matcher below then...
            for (Match match; (match = rangeRe.Match(pattern)).Success;)
            {
                if (match.Groups[1].Value.Contains("/"))
                {
                    rooted = true;
                    // ^ cf. man page:
                    //
                    //   If there is a separator at the beginning or middle (or both)
                    //   of the pattern, then the pattern is relative to the directory
                    //   level of the particular .gitignore file itself. Otherwise
                    //   the pattern may also match at any level below the .gitignore level.
                }
                re += transpileRegexPart(match.Groups[1].Value);
                re += '[' + match.Groups[2].Value + ']';

                pattern = pattern.Substring(match.Length);
            }
            if (!string.IsNullOrWhiteSpace(pattern))
            {
                if (pattern.Contains("/"))
                {
                    rooted = true;
                    // ^ cf. man page:
                    //
                    //   If there is a separator at the beginning or middle (or both)
                    //   of the pattern, then the pattern is relative to the directory
                    //   level of the particular .gitignore file itself. Otherwise
                    //   the pattern may also match at any level below the .gitignore level.
                }
                re += transpileRegexPart(pattern);
            }

            // prep regexes assuming we'll always prefix the check string with a '/':
            re = (rooted ? @"^\/" : @"\/") + re;
            // cf spec:
            //
            //   If there is a separator at the end of the pattern then the pattern
            //   will only match directories, otherwise the pattern can match
            //   **both files and directories**.                   (emphasis mine)
            // if `directory`: match the directory itself and anything within
            // otherwise: match the file itself, or, when it is a directory, match the directory and anything within
            re += (directory ? @"\/" : @"(?:$|\/)");

            // regex validation diagnostics: better to check if the part is valid
            // then to discover it's gone haywire in the big conglomerate at the end.
#if DEBUG
            try
            {
                new Regex($"(?:{re})");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Failed regex:", input, re, ex);
            }
#endif
            return re;
        }

#if DEBUG
        public class OnFailEventArgs : EventArgs
        {
            public string Query { get; set; }
            public string Input { get; set; }
            public bool Expected { get; set; }
            public Regex AcceptRe { get; set; }
            public bool AcceptTest { get; set; }
            public Match AcceptMatch { get; set; }
            public Regex DenyRe { get; set; }
            public bool DenyTest { get; set; }
            public Match DenyMatch { get; set; }
            public string Combine { get; set; }
            public bool ReturnVal { get; set; }

            public OnFailEventArgs(
                string query,
                string input,
                bool expected,
                Regex acceptRe,
                bool acceptTest,
                Match acceptMatch,
                Regex denyRe,
                bool denyTest,
                Match denyMatch,
                string combine,
                bool returnVal)
            {
                Query = query;
                Input = input;
                Expected = expected;
                AcceptRe = acceptRe;
                AcceptTest = acceptTest;
                AcceptMatch = acceptMatch;
                DenyRe = denyRe;
                DenyTest = denyTest;
                DenyMatch = denyMatch;
                Combine = combine;
                ReturnVal = returnVal;
            }
        }
        public delegate void OnFailEventHandler(object sender, OnFailEventArgs e);
        public event OnFailEventHandler OnFail;

        /// Helper invoked when any `Accepts()`, `Denies()` or `Inspects()`
        /// fail to help the developer analyze what is going on inside:
        /// some gitignore spec bits are non-intuitive / non-trivial, after all.
        private void Diagnose(
            string query,
            string input,
            bool expected,
            Regex acceptRe,
            bool acceptTest,
            Match acceptMatch,
            Regex denyRe,
            bool denyTest,
            Match denyMatch,
            string combine,
            bool returnVal
            )
        {
            if (OnFail != null)
            {
                OnFail(this,
                    new OnFailEventArgs(
                            query,
                            input,
                            expected,
                            acceptRe,
                            acceptTest,
                            acceptMatch,
                            denyRe,
                            denyTest,
                            denyMatch,
                            combine,
                            returnVal
                        ));
                return;
            }
            Console.WriteLine($"'{query}': {{");
            Console.WriteLine($"\tquery: '{query}',");
            Console.WriteLine($"\tinput: '{input}',");
            Console.WriteLine($"\texpected: '{expected}',");
            Console.WriteLine($"\tacceptRe: '{acceptRe}',");
            Console.WriteLine($"\tacceptTest: '{acceptTest}',");
            Console.WriteLine($"\tacceptMatch: '{acceptMatch}',");
            Console.WriteLine($"\tdenyRe: '{denyRe}',");
            Console.WriteLine($"\tdenyTest: '{denyTest}',");
            Console.WriteLine($"\tdenyMatch: '{denyMatch}',");
            Console.WriteLine($"\tcombine: '{combine}',");
            Console.WriteLine($"\treturnVal: '{returnVal}'");
            Console.WriteLine("}");
        }
#endif
    }
}
