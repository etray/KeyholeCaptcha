using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeyholeCaptcha.Core;

namespace UnitTest.Core
{
    [TestClass]
    public class ValidatorTest
    {
        [TestMethod]
        public void SetDepthTest()
        {
            int defaultDepth = Validator.MaxDepth;
            Assert.IsTrue(defaultDepth > 0 && defaultDepth < int.MaxValue);
            Validator.MaxDepth = defaultDepth + 1;
            Assert.IsTrue(defaultDepth != Validator.MaxDepth);
        }

        [TestMethod]
        public void GenerateGuidTest()
        {
            string guid = Validator.GenerateGuid();
            Assert.IsFalse(string.IsNullOrWhiteSpace(guid));
            Assert.IsTrue(guid.Length > 10 && guid.Length < 40);
            string anotherGuid = Validator.GenerateGuid();
            Assert.AreNotEqual(guid, anotherGuid);
        }

        [TestMethod]
        public void AddPhraseRequest()
        {
            try
            {
                string guid = Validator.Register();
                Validator.Refresh(guid, "zzz z");
                Assert.IsTrue(Validator.ValidateUserInput(guid, "zzzz"));
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void Validate()
        {
            string guid = Validator.Register();
            Validator.Refresh(guid, "s triNg1");
            Assert.IsFalse(Validator.ValidateUserInput(guid, "bozo"));
            Assert.IsFalse(Validator.ValidateUserInput("bozoGuid", "bozo"));
            Assert.IsTrue(Validator.ValidateUserInput(guid, " STRIN G1 "));
        }



        [TestMethod]
        public void FailedValidations()
        {
            string phrase = "PhraseToBeValidated";

            string guid = Validator.Register();
            Validator.Refresh(guid, phrase);

            for (int i = 0; i < 5; i++)
            {
                Assert.IsFalse(Validator.ValidateUserInput(guid, "bozo"));
            }
            Assert.IsTrue(Validator.ValidateUserInput(guid, phrase));

            // do fail past threshold
            guid = Validator.Register();
            Validator.Refresh(guid, phrase);

            for (int i = 0; i < Validator.MaxFailures + 1; i++)
            {
                Assert.IsFalse(Validator.ValidateUserInput(guid, "bozo"));
            }
            Assert.IsFalse(Validator.ValidateUserInput(guid, phrase));

        }


        [TestMethod]
        public void TestMaxDepth()
        {
            for (int i = 0; i < Validator.MaxDepth + 5; i++)
            {
                Validator.Register();
            }

            Assert.IsTrue(Validator.PhraseRequests.Count == Validator.MaxDepth);
        }

    }
}
