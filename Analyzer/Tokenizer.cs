using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public static class Tokenizer
    {
        /// <summary>
        /// Split the given line into various tokens
        /// </summary>
        /// <param name="str">The string to be tokenized</param>
        /// <returns>An array of string tokens</returns>
        public static string[] Split(string str)
        {
            string[] finalTokens = {"test", "test"};
            List<String> initialTokens = new List<string>();
            StringBuilder token = new StringBuilder();
            foreach (char c in str)
            {
                if (c == ' ')
                {
                    initialTokens.Add(token.ToString());
                    initialTokens.Add(" ");
                    token.Clear();
                }
                else if (c == '.')
                {
                    initialTokens.Add(token.ToString());
                    initialTokens.Add(".");
                    token.Clear();
                }
                else if (c == ';')
                {
                    initialTokens.Add(token.ToString());
                    initialTokens.Add(";");
                    token.Clear();
                }
                else if (c == '(')
                {
                    initialTokens.Add(token.ToString());
                    initialTokens.Add("(");
                    token.Clear();
                }
                else if (c == ')')
                {
                    initialTokens.Add(token.ToString());
                    initialTokens.Add(")");
                    token.Clear();
                }
                else if (c == '{')
                {
                    initialTokens.Add(token.ToString());
                    initialTokens.Add("{");
                    token.Clear();
                }
                else if (c == '}')
                {
                    initialTokens.Add(token.ToString());
                    initialTokens.Add("}");
                    token.Clear();
                }
                else if (c == '[')
                {
                    initialTokens.Add(token.ToString());
                    initialTokens.Add("[");
                    token.Clear();
                }
                else if (c == ']')
                {
                    initialTokens.Add(token.ToString());
                    initialTokens.Add("]");
                    token.Clear();
                }
                else
                    token.Append(c);
            }

            return initialTokens.ToArray();
        }
    }
}
