﻿using System;
using dotNetTips.Utility.Standard;
using dotNetTips.Utility.Standard.Tester;
using dotNetTips.Utility.Standard.Tester.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dotNetTips.Tips.Utility.Standard.Tests
{
    [TestClass]
    public class TypeHelperTests
    {

        [TestMethod]
        public void GetPropertyValuesTest()
        {
            var person = RandomData.GeneratePerson<PersonProper>();

            var result = TypeHelper.GetPropertyValues(person);

            Assert.IsTrue(result.Count > 5);

            var exTest = new ArgumentOutOfRangeException("TESTPARAM", "TESTMESSAGE");

            result = TypeHelper.GetPropertyValues(exTest);

            Assert.IsTrue(result.Count > 1);
        }

        [TestMethod]
        public void GetTypeDisplayNameTest()
        {
            var person = RandomData.GeneratePerson<PersonProper>();

            var result = TypeHelper.GetTypeDisplayName(person);

            Assert.IsTrue(result == "dotNetTips.Utility.Standard.Tester.Models.PersonProper");

            result = TypeHelper.GetTypeDisplayName(person, true);

            Assert.IsTrue(result == "dotNetTips.Utility.Standard.Tester.Models.PersonProper");

            result = TypeHelper.GetTypeDisplayName(typeof(int), true, true, true, '-');

            Assert.IsTrue(result == "int");
        }
    }
}
