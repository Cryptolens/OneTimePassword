using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Diagnostics;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Debug.WriteLine(OneTimePassword.HOTP.GetToken(new byte[] { 123, 123 }, 123));
        }
    }
}
