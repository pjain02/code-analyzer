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
        private TextReader _reader;     //Reads a file or a string

        public Tokenizer()
        {
            _buffer = new Queue<String>();
        }

        public bool OpenFile(String path)
        {
            try
            {
                _reader = new StreamReader(path);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Opens a string as a text reader to read sequentially from it
        /// </summary>
        /// <param name="str">String to be opened</param>
        /// <returns>Whether opened successfully or not</returns>
        public bool OpenString(String str)
        {
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
            string _line;
            do
            {
                _line = _reader.ReadLine();
            } while (_line == "");
            if (_line == null)
                return false;

            /* Comments and qouted strings are considered as single tokens
             * and have to taken care of before tokenizing the rest of the line
             */
            do
            {
                int posCComm = _line.IndexOf("/*");
                int posCppComm = _line.IndexOf("//");
                int posSQoute = _line.IndexOf('\'');
                int posDQoute = _line.IndexOf('\"');

                int[] numbers = { posCComm, posCppComm, posDQoute, posSQoute };
                for (int i = 0; i < numbers.Length; i++)
                    numbers[i] = numbers[i] == -1 ? Int32.MaxValue : numbers[i];

                int first = numbers.Min();
                String lineToTokenize = _line;
                if (posCComm == first)
                    lineToTokenize = TokenizeCComm(ref _line);
                else if (posCppComm == first)
                    lineToTokenize = TokenizeCppComm(ref _line);
                else if (posSQoute == first)
                    lineToTokenize = TokenizeSQoute(ref _line);
                else if (posDQoute == first)
                    lineToTokenize = TokenizeDQoute(ref _line);
                else
                    _line = "";

                //The next phase
                if (lineToTokenize == "" && _line != "")
                    continue;
                char[] delim = { ' ', '\t' };
                string[] tokens = lineToTokenize.Split(delim);
                foreach (string token in tokens)
                {
                    string temp = token;
                    while (temp.Length > 0)
                    {
                        //Check for punctuation
                        if (IsPunc(temp[0]) || Char.IsSymbol(temp[0]))
                        {
                            _buffer.Enqueue(temp[0].ToString());
                            temp = temp.Remove(0, 1);
                        }
                        //Add the word
                        else
                        {
                            _buffer.Enqueue(GetWord(ref temp));
                        }
                    }
                }

            } while (_line != "");

            return true;
        }

        private string TokenizeCComm(ref string _line)
        {
            //Check if the comment starts at beginning
            int startPos = _line.IndexOf("/*");
            string retStr = "";
            if (startPos != 0)
            {
                retStr = _line.Remove(startPos);
                _line = _line.Remove(0, startPos);
                return retStr;
            }

            //If it starts at the beginning
            int endPos = _line.IndexOf("*/");
            if (endPos != -1)
            {
                _buffer.Enqueue(_line.Remove(endPos + 2));
                _line = _line.Remove(0, endPos + 2);
            }
            else
            {
                //The comment spans multiple lines and they have to be
                //retrieved too
                StringBuilder commToken = new StringBuilder();
                commToken.Append(_line);
                string nextLine;
                do
                {
                    nextLine = _reader.ReadLine();
                    endPos = nextLine.IndexOf("*/");
                    if (endPos == -1)
                    {
                        commToken.Append("\n" + nextLine);
                        continue;
                    }
                    else
                    {
                        commToken.Append("\n" + nextLine.Remove(endPos + 2));
                        _buffer.Enqueue(commToken.ToString());
                        _line = nextLine.Remove(0, endPos + 2);
                    }
                } while (endPos == -1);

            }
            return retStr;
        }

        private string TokenizeCppComm(ref string _line)
        {
            string retStr = "";
            int startPos = _line.IndexOf("//");
            if (startPos == 0)
            {
                _buffer.Enqueue(_line);
                _line = "";
            }
            else
            {
                retStr = _line.Remove(startPos);
                _line = _line.Remove(0, startPos + 2);
            }
            
            return retStr;
        }

        private string TokenizeSQoute(ref string _line)
        {
            //Check if the qoute is at the beginning
            int startPos = _line.IndexOf('\'');
            string retStr = "";
            if (startPos == 0)
            {
                StringBuilder sQoute = new StringBuilder();
                sQoute.Append('\'');
                for (int i = 1; i < _line.Length; i++)
                {
                    sQoute.Append(_line[i]);
                    /* Check the case where ending qoute might one of the following case
                     * Ignore: "Test\'"
                     * Valid: "Test\\'" */
                    if (_line[i] == '\'' && (_line[i - 1] != '\\' || _line[i - 2] == '\\'))
                    {
                        _buffer.Enqueue(sQoute.ToString());
                        _line = _line.Remove(0, i + 1);
                        break;
                    }
                }
            }
            else
            {
                retStr = _line.Remove(startPos);
                _line = _line.Remove(0, startPos);
            }

            return retStr;
        }

        private string TokenizeDQoute(ref string _line)
        {
            //Check if the qoute is at the beginning
            int startPos = _line.IndexOf('\"');
            string retStr = "";
            if (startPos == 0)
            {
                StringBuilder dQoute = new StringBuilder();
                dQoute.Append('\"');
                for (int i = 1; i < _line.Length; i++)
                {
                    dQoute.Append(_line[i]);
                    /* Check the case where ending qoute might one of the following case
                     * Ignore: "Test\'"
                     * Valid: "Test\\'" */
                    if (_line[i] == '\"' && (_line[i - 1] != '\\' || _line[i - 2] == '\\'))
                    {
                        _buffer.Enqueue(dQoute.ToString());
                        _line = _line.Remove(0, i + 1);
                        break;
                    }
                }
            }
            else
            {
                retStr = _line.Remove(startPos);
                _line = _line.Remove(0, startPos);
            }

            return retStr;
        }

        private bool IsPunc(char c)
        {
            if (c == '_')
                return false;
            return Char.IsPunctuation(c);
        }

        private string GetWord(ref string str)
        {
            StringBuilder word = new StringBuilder("");
            for (int i = 0; i < str.Length; i++)
            {
                if (IsPunc(str[i]) || Char.IsSymbol(str[i]))
                {
                    str = str.Remove(0, i);
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
