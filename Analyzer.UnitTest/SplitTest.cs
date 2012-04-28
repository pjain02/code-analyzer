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
            
            //Test Case 1
            inputs.Add("using Lss.Helpers;");
            string[] strs1 = {"using", " ", "Lss", ".", "Helpers", ";"};
            outputs.Add(strs1);

            //Test Case 2
            inputs.Add("namespace Analyzer.UnitTest");
            string[] strs2 = { "namespace", " ", "Analyzer", ".", "UnitTest" };
            outputs.Add(strs2);

            //Test Case 3
            inputs.Add("[TestClass]");
            string[] strs3 = { "[", "TestClass", "]" };
            outputs.Add(strs3);

            //Test Case 4
            inputs.Add("inputs = new List<string>();");
            string[] strs4 = { "inputs", " ", "=", " ", "new", " ", "List", "<", "string", ">", "(", ")", ";" };
            outputs.Add(strs4);
        }

        [TestMethod]
        public void TestMethod1()
        {
            int i = 0;
            foreach (string s in Tokenizer.Split(inputs[0]))
            {
                Assert.AreEqual(outputs[0][i], s);
                i++;
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            int i = 0;
            foreach (string s in Tokenizer.Split(inputs[1]))
            {
                Assert.AreEqual(outputs[1][i], s);
                i++;
            }
        }

        [TestMethod]
        public void TestMethod3()
        {
            int i = 0;
            foreach (string s in Tokenizer.Split(inputs[2]))
            {
                Assert.AreEqual(outputs[2][i], s);
                i++;
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            int i = 0;
            foreach (string s in Tokenizer.Split(inputs[3]))
            {
                Assert.AreEqual(outputs[3][i], s);
                i++;
            }
        }
    }
}
