using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeyholeCaptcha.Core;
using System.Collections.Generic;
using KeyholeCaptcha.Core.PhraseGenerators;

namespace UnitTest.Core
{
    [TestClass]
    public class PhraseGeneratorTests
    {
        PhraseGenerator PhraseGenerator { get; set; }

        public PhraseGeneratorTests()
        {
            PhraseGenerator = new WordListPhraseGenerator();
        }

        [TestMethod]
        public void RandomPhraseTest()
        {
            string phrase1 = PhraseGenerator.RandomPhrase();
            string phrase2 = PhraseGenerator.RandomPhrase();
            Assert.IsFalse(string.IsNullOrWhiteSpace(phrase1));
            Assert.IsFalse(string.IsNullOrWhiteSpace(phrase2));
            Assert.IsFalse(phrase1 == phrase2);
        }

        [TestMethod]
        public void LoadWordListTest()
        {
            Assert.IsTrue(((WordListPhraseGenerator)PhraseGenerator).WordList.Count > 0);
        }
    }
}