using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeyholeCaptcha.Core;
using System.Diagnostics;

namespace UnitTest.Core
{
    [TestClass]
    public class RandomnessProviderTest
    {
        [TestMethod]
        public void RandFromRangeTest()
        {
            int min = -5;
            int max = 5;

            // secure random number generator
            int[] valueCounts = new int[max - min];
            for (var i = 0; i < 100; i++)
            {
                int value = RandomnessProvider.RandFromRange(min, max);

                // make sure no values fall outside of desired range
                if (value < min || value > max)
                {
                    Assert.Fail();
                }
                valueCounts[value - min]++;
            }

            // check that each value in range is represented
            for (var i = 0; i < valueCounts.Length; i++)
            {
                Assert.IsTrue(valueCounts[i] > 0);
            }

            // pseudorandom generator
            valueCounts = new int[max - min];
            for (var i = 0; i < 100; i++)
            {
                int value = RandomnessProvider.PseudoRandFromRange(min, max);

                // make sure no values fall outside of desired range
                if (value < min || value > max)
                {
                    Assert.Fail();
                }
                valueCounts[value - min]++;
            }

            // check that each value in range is represented
            for (var i = 0; i < valueCounts.Length; i++)
            {
                Assert.IsTrue(valueCounts[i] > 0);
            }
        }

        [TestMethod]
        public void RandomStringTest()
        {
            for (int i = 1; i < 22; i++)
            {
                string randString = RandomnessProvider.RandomAlphaNumericString(i);
                Assert.AreEqual(randString.Length, i);
                Assert.AreEqual(true, randString.Count(char.IsLetterOrDigit) == randString.Length);
            }
        }
    }
}
