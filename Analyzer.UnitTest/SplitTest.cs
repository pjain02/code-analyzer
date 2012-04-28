using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer;

namespace Analyzer.UnitTest
{
    [TestClass]
    public class SplitTest
    {
        List<String> inputs;
        List<String[]> outputs;

        [TestInitialize]
        public void Initialize()
        {
            inputs = new List<string>();
            outputs = new List<string[]>();

            inputs.Add("using Lss.Helpers;");
            string[] strs = {"using", " ", "Lss", ".", "Helpers", ";"};
            outputs.Add(strs);
        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(outputs[0], Tokenizer.Split(inputs[0]));
        }
    }
}
