using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics;
using Cryptolens.OneTimePassword;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var secret = OneTimePassword.CreateSharedSecret();
            Debug.WriteLine(OneTimePassword.CounterBasedPassword(secret, 123));
            Debug.WriteLine(OneTimePassword.TimeBasedPassword(new byte[] { 123, 123 }));

            Debug.WriteLine(secret);
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var secret = OneTimePassword.CreateSharedSecret();
            Debug.WriteLine(OneTimePassword.SharedSecretToString(secret));

            Debug.WriteLine(OneTimePassword.TimeBasedPassword(secret));

            Debug.WriteLine(OneTimePassword.GetAuthenticatorAppUrl(OneTimePassword.SharedSecretToString(secret), "artem", "Cryptolens Demo"));
        }
    }
}
