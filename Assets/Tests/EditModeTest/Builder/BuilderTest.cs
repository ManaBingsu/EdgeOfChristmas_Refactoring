using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Editor.Builder;

namespace Tests
{
    public class BuilderTest
    {
        Builder builder;
        [Test]
        public void CommandLine_ParsingBuildInformationFromString()
        {
            string[] strings = new string[] {
                "$Valid1",
                "Error1",
                "$Valid2",
                "$Valid3",
                "Error2",
                "$Valid4"
            };
            List<string> validStrings = builder.GetValidStrings(strings, "$");
            Assert.AreEqual(4, validStrings.Count);
        }

        [Test]
        public void GetValidBuildInfo()
        {

        }

        [SetUp]
        public void SetUp()
        {
            builder = new Builder();
        }
    }
}
