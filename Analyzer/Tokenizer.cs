using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer
{
    public class Tokenizer
    {
        private Queue<String> _buffer;   //Token Buffer
        private String _line;   //An instance of line read from the input
        private TextReader _reader;     //Reads a file or a string

        public Tokenizer()
        {
            _buffer = new Queue<String>();
        }

        /// <summary>
        /// Opens a string as a text reader to read sequentially from it
        /// </summary>
        /// <param name="str">String to be opened</param>
        /// <returns>Whether opened successfully or not</returns>
        public bool OpenString(String str)
        {
            _line = "";
            try
            {
                _reader = new StringReader(str);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Closes the tokenizer's stream reader
        /// </summary>
        public void Close()
        {
            _reader.Close();
        }

        /// <summary>
        /// Get the next token from the tokenizer
        /// </summary>
        /// <returns>token read from the tokenizer</returns>
        public String GetToken()
        {
            //Check if the buffer is full, otherwise fill it up and then read from it
            if (_buffer.Count == 0)
                if (!FillBuffer())
                    return "";

            return _buffer.Dequeue();
        }

        private bool FillBuffer()
        {
            string line = _reader.ReadLine();
            if (line == null)
                return false;

            char[] delim = { ' ', '\t' };
            string[] tokens = line.Split(delim);
            foreach(string token in tokens)
            {
                string temp = token;
                while (temp.Length > 0)
                {
                    if (IsPunc(temp[0]) || Char.IsSymbol(temp[0]))
                    {
                        _buffer.Enqueue(temp.Remove(1, token.Length - 1));
                        temp = temp.Remove(0, 1);
                    }
                    else
                    {
                        _buffer.Enqueue(AddWord(ref temp));
                    }
                }
            }

            return true;
        }

        private bool IsPunc(char c)
        {
            if (c == '_')
                return false;
            return Char.IsPunctuation(c);
        }

        private string AddWord(ref string str)
        {
            StringBuilder word = new StringBuilder("");
            for (int i = 0; i < str.Length; i++)
            {
                if (IsPunc(str[i]) || Char.IsSymbol(str[i]))
                {
                    str = str.Remove(0, i - 1);
                    return word.ToString();
                }
                else
                    word.Append(str[i]);
            }
            str = "";
            return word.ToString();
        }
    }
}
