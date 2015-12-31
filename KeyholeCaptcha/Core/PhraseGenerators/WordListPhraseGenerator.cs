// Copyright (c) 2015-2016 - Elton T. Ray
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KeyholeCaptcha.Core.PhraseGenerators
{
    public class WordListPhraseGenerator : PhraseGenerator
    {
        private static IList<string> wordListBackingValue = new List<string>(); 
        public IList<string> WordList 
        { 
            get
            {
                lock (wordListBackingValue)
                {
                    if (wordListBackingValue.Count <= 0)
                    {
                        LoadWordList(KeyholeCaptcha.resources.wordlist);
                    }
                }

                return wordListBackingValue;
            }
             
            private set
            {
                wordListBackingValue = value;            
            }
        }

        public WordListPhraseGenerator()
        {
        }

        // from byte[] resource
        public void LoadWordList(byte[] wordList)
        {
            if (wordList == null || wordList.Length == 0)
            {
                throw new Exception("Word list not found.");
            }
            string line;
            using (MemoryStream memoryStream = new MemoryStream(wordList))
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(memoryStream))
                {
                    while ((line = file.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line) && line[0] != '#')
                        {
                            wordListBackingValue.Add(line.Trim());
                        }
                    }
                }
            }
        }

        override public string RandomPhrase()
        {
            if (WordList.Count <= 0)
            {
                throw new Exception("Word list can not be empty.");
            }

            return WordList[RandomnessProvider.RandFromRange(0, WordList.Count)];
        }
    }
}
