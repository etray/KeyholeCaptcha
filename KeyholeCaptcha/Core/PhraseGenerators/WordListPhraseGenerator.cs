﻿// Copyright (c) 2015 - Elton T. Ray
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
        public IList<string> WordList { get; private set; }

        public WordListPhraseGenerator()
        {
            WordList = new List<string>();
            LoadWordList(KeyholeCaptcha.resources.wordlist);
        }

        public WordListPhraseGenerator(string wordList) : this()
        {
            LoadWordList(wordList);
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
                            WordList.Add(line.Trim());
                        }
                    }
                }
            }
        }

        // from filename
        public void LoadWordList(string wordList)
        {
            if (!File.Exists(wordList))
            {
                throw new Exception("Word list file: '" + wordList + "' not found.");
            }

            string line;
            using (System.IO.StreamReader file = new System.IO.StreamReader(wordList))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line) && line[0] != '#')
                    {
                        WordList.Add(line.Trim());
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
