using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Editor.Builder;
using UnityEditor;

namespace Tests
{
    public class BuilderTest
    {
        AutoBuilder builder;
        [Test]
        public void ParsingBuildInformationFromString()
        {
            string[] strings = new string[] {
                "$Valid1",
                "Invalid1",
                "$Valid2",
                "$Valid3",
                "Invalid2",
                "$Valid4"
            };
            List<string> validStrings = builder.GetValidStrings(strings, "$");
            Assert.AreEqual(4, validStrings.Count);
        }

        [Test]
        public void BuildFailedWhenArgumentNumberIsInvalid()
        {
            string[] strings = new string[] {
                "$buildPath",
                "$logPath",
                "$win64",
                "$LZ4",
                "$Over1"
            };
            builder = new AutoBuilder(strings);
            Assert.IsFalse(builder.BuildGame());
            strings = new string[] {
                "$buildPath",
                "$logPath",
                "$win64",
            };
            builder = new AutoBuilder(strings);
            Assert.IsFalse(builder.BuildGame());
        }

        [Test]
        public void BuildFailedWhenPlatformOrCompressionModeIsInvalid()
        {
            string[] strings = new string[] {
                "$buildPath",
                "$logPath",
                "$Invalid1",
                "$LZ4"
            };
            builder = new AutoBuilder(strings);
            Assert.IsFalse(builder.BuildGame());
            strings = new string[] {
                "$buildPath",
                "$logPath",
                "$win64",
                "$Invalid1"
            };
            builder = new AutoBuilder(strings);
            Assert.IsFalse(builder.BuildGame());
        }

        [SetUp]
        public void SetUp()
        {
            builder = new AutoBuilder();
        }
    }
}
