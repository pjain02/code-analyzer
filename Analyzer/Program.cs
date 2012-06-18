using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Tokenizer toker = new Tokenizer();
            toker.OpenFile("../../Tokenizer.cs");
            string temp;
            while ((temp = toker.GetToken()) != "")
            {
                Debug.WriteLine(temp);
            }
            toker.Close();
        }
    }
}
