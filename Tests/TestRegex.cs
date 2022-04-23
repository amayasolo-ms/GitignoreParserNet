
using GitignoreParserNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace Tests.TestRegex
{
    [TestClass]
    public class TestRegex
    {
        readonly string fixture;
        //readonly string noNegativeFixture;

        public TestRegex()
        {
            fixture = File.ReadAllText(@"..\..\..\.gitignore-fixture", Encoding.UTF8);
            //noNegativeFixture = File.ReadAllText(@"..\..\..\.gitignore-no-negatives", Encoding.UTF8);
        }

        [TestMethod("should parse some content")]
        public void ParseSomeContent()
        {
            var (positives, negatives) = GitignoreParser.Parse(fixture);
            Assert.IsTrue(
                positives.Item1 != null
                && positives.Item2 != null
                && negatives.Item1 != null
                && negatives.Item2 != null
            );
        }

        [TestMethod("should accept ** wildcards")]
        public void Accept2StarWildcards()
        {
            var (positives, negatives) = GitignoreParser.Parse("a/**/b");
            Assert.AreEqual(@"(?:^\/a(?:\/|(?:\/.+\/))b(?:$|\/))", positives.Item1.ToString());
            Assert.IsTrue(positives.Item2.Length == 1);
            Assert.AreEqual(@"^\/a(?:\/|(?:\/.+\/))b(?:$|\/)", positives.Item2[0].ToString());
            Assert.AreEqual("$^", negatives.Item1.ToString());
            Assert.IsTrue(negatives.Item2.Length == 0);
        }

        [TestMethod("should accept * wildcards")]
        public void Accept1StarWildcards()
        {
            var (positives, negatives) = GitignoreParser.Parse("a/b*c");
            Assert.AreEqual(@"(?:^\/a\/b[^\/]*c(?:$|\/))", positives.Item1.ToString());
            Assert.IsTrue(positives.Item2.Length == 1);
            Assert.AreEqual(@"^\/a\/b[^\/]*c(?:$|\/)", positives.Item2[0].ToString());
            Assert.AreEqual("$^", negatives.Item1.ToString());
            Assert.IsTrue(negatives.Item2.Length == 0);
        }

        [TestMethod("should accept ? wildcards")]
        public void AcceptQuestionMarkWildcards()
        {
            var (positives, negatives) = GitignoreParser.Parse("a/b?");
            Assert.AreEqual(@"(?:^\/a\/b[^\/](?:$|\/))", positives.Item1.ToString());
            Assert.IsTrue(positives.Item2.Length == 1);
            Assert.AreEqual(@"^\/a\/b[^\/](?:$|\/)", positives.Item2[0].ToString());
            Assert.AreEqual("$^", negatives.Item1.ToString());
            Assert.IsTrue(negatives.Item2.Length == 0);
        }

        [TestMethod("should correctly encode literals")]
        public void EncodeLiterals()
        {
            var (positives, negatives) = GitignoreParser.Parse(@"a/b.c[d](e){f}\slash^_$+$$$");
            Assert.AreEqual(@"(?:^\/a\/b\.c[d]\(e\)\{f\}slash\^_\$\+\$\$\$(?:$|\/))", positives.Item1.ToString());
            Assert.IsTrue(positives.Item2.Length == 1);
            Assert.AreEqual(@"^\/a\/b\.c[d]\(e\)\{f\}slash\^_\$\+\$\$\$(?:$|\/)", positives.Item2[0].ToString());
            Assert.AreEqual("$^", negatives.Item1.ToString());
            Assert.IsTrue(negatives.Item2.Length == 0);
        }

        [TestMethod("should correctly parse character ranges")]
        public void ParseCharacterRanges()
        {
            var (positives, negatives) = GitignoreParser.Parse(@"a[c-z$].[1-9-][\[\]A-Z]-\[...]");
            Assert.AreEqual(@"(?:\/a[c-z$]\.[1-9-][\[\]A-Z]\-\[\.\.\.\](?:$|\/))", positives.Item1.ToString());
            Assert.IsTrue(positives.Item2.Length == 1);
            Assert.AreEqual(@"\/a[c-z$]\.[1-9-][\[\]A-Z]\-\[\.\.\.\](?:$|\/)", positives.Item2[0].ToString());
            Assert.AreEqual("$^", negatives.Item1.ToString());
            Assert.IsTrue(negatives.Item2.Length == 0);
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
            var (positives, negatives) = GitignoreParser.Parse("/a\nb");
            Assert.AreEqual(@"(?:^\/a(?:$|\/))|(?:\/b(?:$|\/))", positives.Item1.ToString());
            Assert.IsTrue(positives.Item2.Length == 2);
            Assert.AreEqual(@"^\/a(?:$|\/)", positives.Item2[0].ToString());
            Assert.AreEqual(@"\/b(?:$|\/)", positives.Item2[1].ToString());
            Assert.AreEqual("$^", negatives.Item1.ToString());
            Assert.IsTrue(negatives.Item2.Length == 0);
        }

        [TestMethod("should correctly transpile rooted and relative path specs")]
        public void TranspileRootedAndRelative()
        {
            var (positives, negatives) = GitignoreParser.Parse("a/b\n/c/d\ne/\nf");
            Assert.AreEqual(@"(?:^\/c\/d(?:$|\/))|(?:^\/a\/b(?:$|\/))|(?:\/e\/)|(?:\/f(?:$|\/))", positives.Item1.ToString());
            Assert.IsTrue(positives.Item2.Length == 4);
            Assert.AreEqual(@"^\/c\/d(?:$|\/)", positives.Item2[0].ToString());
            Assert.AreEqual(@"^\/a\/b(?:$|\/)", positives.Item2[1].ToString());
            Assert.AreEqual(@"\/e\/", positives.Item2[2].ToString());
            Assert.AreEqual(@"\/f(?:$|\/)", positives.Item2[3].ToString());
            Assert.AreEqual("$^", negatives.Item1.ToString());
            Assert.IsTrue(negatives.Item2.Length == 0);
        }
    }
}
