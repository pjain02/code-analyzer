using System;
using System.Collections.Generic;
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
            toker.OpenFile("TokerTest.cs");
            string temp;
            while ((temp = toker.GetToken()) != "")
            {
                Console.WriteLine(temp);
            }
            toker.Close();
        }
    }
}
