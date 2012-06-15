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
            toker.OpenString("\"Test");
            string temp = toker.GetToken();
        }
    }
}
