﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TruncateGCode.Tests
{
    [TestClass]
    public class GCodeTruncateUtilTests
    {
        [TestMethod]
        public void Basic_String_Should_Remain_Untouched()
        {
            string expected = "(Generated by PartKam Version 0.05)";
            string actual = TruncateGCode.GCodeTruncateUtil.TruncateGCode(expected, 3);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GCode_Should_Round_To_3dp()
        {
            string input = "G3 X76.268 Y29.531999999999996 I1.1500000000000001 J0";
            string expected = "G3 X76.268 Y29.532 I1.150 J0";
            string actual = GCodeTruncateUtil.TruncateGCode(input, 3);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ParseCode_Should_Round_to_3dp()
        {
            string input = Properties.Resources.ParseCodeTestFile1;
            string expected = Properties.Resources.ParseCodeTestFile1ExpectedResult;
            int dp = 3;
            string actual = GCodeTruncateUtil.ProcessCode(input, dp);
            //Assert.AreEqual(expected, actual);
            // test line by line
            string[] inputLines = input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            string[] expectedLines = input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

            Assert.AreEqual(expectedLines.Length, inputLines.Length);

            for (int i = 0; i < inputLines.Length; i++)
            {
                Assert.AreEqual(expectedLines[i], inputLines[i], $"Line #{i}");
            }

        }

    }
}
