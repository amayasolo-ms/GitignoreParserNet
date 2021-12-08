
using GitignoreParserNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace Tests
{
    [TestClass]
    public class TestRegex
    {
        readonly string fixture;
        readonly string noNegativeFixture;

        public TestRegex()
        {
            fixture = File.ReadAllText(@"..\..\..\.gitignore-fixture", Encoding.UTF8);
            noNegativeFixture = File.ReadAllText(@"..\..\..\.gitignore-no-negatives", Encoding.UTF8);
        }

        [TestMethod("should parse some content")]
        public void ParseSomeContent()
        {
            var parsed = GitignoreParser.Parse(fixture);
            Assert.IsTrue(
                parsed.positives.Item1 != null
                && parsed.positives.Item2 != null
                && parsed.negatives.Item1 != null
                && parsed.negatives.Item2 != null
            );
        }

        [TestMethod("should accept ** wildcards")]
        public void Accept2StarWildcards()
        {
            var parsed = GitignoreParser.Parse("a/**/b");
            Assert.AreEqual(@"(?:^\/a(?:\/|(?:\/.+\/))b(?:$|\/))", parsed.positives.Item1.ToString());
            Assert.IsTrue(parsed.positives.Item2.Length == 1);
            Assert.AreEqual(@"^\/a(?:\/|(?:\/.+\/))b(?:$|\/)", parsed.positives.Item2[0].ToString());
            Assert.AreEqual(@"$^", parsed.negatives.Item1.ToString());
            Assert.IsTrue(parsed.negatives.Item2.Length == 0);
        }

        [TestMethod("should accept * wildcards")]
        public void Accept1StarWildcards()
        {
            var parsed = GitignoreParser.Parse("a/b*c");
            Assert.AreEqual(@"(?:^\/a\/b[^\/]*c(?:$|\/))", parsed.positives.Item1.ToString());
            Assert.IsTrue(parsed.positives.Item2.Length == 1);
            Assert.AreEqual(@"^\/a\/b[^\/]*c(?:$|\/)", parsed.positives.Item2[0].ToString());
            Assert.AreEqual(@"$^", parsed.negatives.Item1.ToString());
            Assert.IsTrue(parsed.negatives.Item2.Length == 0);
        }

        [TestMethod("should accept ? wildcards")]
        public void AcceptQuestionMarkWildcards()
        {
            var parsed = GitignoreParser.Parse("a/b?");
            Assert.AreEqual(@"(?:^\/a\/b[^\/](?:$|\/))", parsed.positives.Item1.ToString());
            Assert.IsTrue(parsed.positives.Item2.Length == 1);
            Assert.AreEqual(@"^\/a\/b[^\/](?:$|\/)", parsed.positives.Item2[0].ToString());
            Assert.AreEqual(@"$^", parsed.negatives.Item1.ToString());
            Assert.IsTrue(parsed.negatives.Item2.Length == 0);
        }

        [TestMethod("should correctly encode literals")]
        public void EncodeLiterals()
        {
            var parsed = GitignoreParser.Parse(@"a/b.c[d](e){f}\slash^_$+$$$");
            Assert.AreEqual(@"(?:^\/a\/b\.c[d]\(e\)\{f\}slash\^_\$\+\$\$\$(?:$|\/))", parsed.positives.Item1.ToString());
            Assert.IsTrue(parsed.positives.Item2.Length == 1);
            Assert.AreEqual(@"^\/a\/b\.c[d]\(e\)\{f\}slash\^_\$\+\$\$\$(?:$|\/)", parsed.positives.Item2[0].ToString());
            Assert.AreEqual(@"$^", parsed.negatives.Item1.ToString());
            Assert.IsTrue(parsed.negatives.Item2.Length == 0);
        }

        [TestMethod("should correctly parse character ranges")]
        public void ParseCharacterRanges()
        {
            var parsed = GitignoreParser.Parse(@"a[c-z$].[1-9-][\[\]A-Z]-\[...]");
            Assert.AreEqual(@"(?:\/a[c-z$]\.[1-9-][\[\]A-Z]\-\[\.\.\.\](?:$|\/))", parsed.positives.Item1.ToString());
            Assert.IsTrue(parsed.positives.Item2.Length == 1);
            Assert.AreEqual(@"\/a[c-z$]\.[1-9-][\[\]A-Z]\-\[\.\.\.\](?:$|\/)", parsed.positives.Item2[0].ToString());
            Assert.AreEqual(@"$^", parsed.negatives.Item1.ToString());
            Assert.IsTrue(parsed.negatives.Item2.Length == 0);
        }

        [TestMethod("should treat \"/a\" as rooted, while \"b\" should match anywhere")]
        // see bullet 6 of https://git-scm.com/docs/gitignore#_pattern_format
        //
        // If there is a separator at the beginning or middle (or both) of the pattern,
        // then the pattern is relative to the directory level of the particular
        // .gitignore file itself. Otherwise the pattern may also match at any level
        // below the .gitignore level.
        public void ARootedBAnywhere()
        {
            var parsed = GitignoreParser.Parse("/a\nb");
            Assert.AreEqual(@"(?:^\/a(?:$|\/))|(?:\/b(?:$|\/))", parsed.positives.Item1.ToString());
            Assert.IsTrue(parsed.positives.Item2.Length == 2);
            Assert.AreEqual(@"^\/a(?:$|\/)", parsed.positives.Item2[0].ToString());
            Assert.AreEqual(@"\/b(?:$|\/)", parsed.positives.Item2[1].ToString());
            Assert.AreEqual(@"$^", parsed.negatives.Item1.ToString());
            Assert.IsTrue(parsed.negatives.Item2.Length == 0);
        }

        [TestMethod("should correctly transpile rooted and relative path specs")]
        public void TranspileRootedAndRelative()
        {
            var parsed = GitignoreParser.Parse("a/b\n/c/d\ne/\nf");
            Assert.AreEqual(@"(?:^\/c\/d(?:$|\/))|(?:^\/a\/b(?:$|\/))|(?:\/e\/)|(?:\/f(?:$|\/))", parsed.positives.Item1.ToString());
            Assert.IsTrue(parsed.positives.Item2.Length == 4);
            Assert.AreEqual(@"^\/c\/d(?:$|\/)", parsed.positives.Item2[0].ToString());
            Assert.AreEqual(@"^\/a\/b(?:$|\/)", parsed.positives.Item2[1].ToString());
            Assert.AreEqual(@"\/e\/", parsed.positives.Item2[2].ToString());
            Assert.AreEqual(@"\/f(?:$|\/)", parsed.positives.Item2[3].ToString());
            Assert.AreEqual(@"$^", parsed.negatives.Item1.ToString());
            Assert.IsTrue(parsed.negatives.Item2.Length == 0);
        }
    }
}
