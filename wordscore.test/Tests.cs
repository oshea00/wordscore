using NUnit.Framework;
using System;
using System.Collections.Generic;
using wordscore;

namespace Tests
{
    public class Tests
    {
        [Test]
        public void CanHashWordToHistogram()
        {
            var (histStr, letters) = Histogram.HistKey("BALLY");
            Assert.AreEqual("11000000000200000000000010", histStr);
            Assert.AreEqual("01000000000000100000000011", Convert.ToString(letters,2).PadLeft(26,'0'));
        }

        [TestCase("","",ExpectedResult = true)]
        [TestCase("ABC", "CBA", ExpectedResult = true)]
        [TestCase("ABC", "AC", ExpectedResult = true)]
        [TestCase("XXXXX", "XY", ExpectedResult = false)]
        public bool SubwordIsAnagramOfWord(string word, string subword)
        {
            return Histogram.HistInHist(Histogram.HistKey(word), Histogram.HistKey(subword));
        }

        [Test]
        public void CanFindSuperWordForSimpleDictionary()
        {
            var words = new string[] {
                "ALLLL","BALLY","ALLY","BALL","ALL","BAY","AL","LA"
            };

            var finder = new SuperWordFinder(words);
            var superWords = finder.FindSupers();
            Assert.AreEqual(new[] {"BALLY"},superWords);
            Assert.AreEqual(6, finder.MaxScore);
        }
    }
}