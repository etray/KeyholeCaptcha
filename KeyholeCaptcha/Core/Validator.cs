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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Collections;

namespace KeyholeCaptcha.Core
{
    internal class RequestEntry
    {
        public RequestEntry()
        {
            Validated = false;
        }
        
        public RequestEntry(string phrase)
        {
            Phrase = phrase;
            Validated = false;
        }
        public string Phrase { get; set; }
        public int ErrorCount { get; set; }
        public bool Validated { get; set; }
    }

    public static class Validator
    {
        // once depth is reached, we start forgetting oldest captcha requests
        public static int MaxDepth { get; set;}
        public static int MaxFailures { get; set; }

        public static OrderedDictionary PhraseRequests { get; set; }

        static Validator()
        {
            PhraseRequests = new OrderedDictionary();
            
            // TODO: make configurable
            MaxDepth = 500;
            MaxFailures = 10;
        }

        public static string Register()
        {
            string guid = GenerateGuid();
            AddRequest(guid, null);
            return guid;
        }

        public static bool Refresh(string guid, string phrase)
        {
            bool result = true;
            lock (PhraseRequests)
            {
                if (PhraseRequests.Contains(guid))
                {
                    ((RequestEntry)PhraseRequests[guid]).Phrase = phrase;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

        public static bool IsActiveGuid(string guid)
        {
            bool result = false;
            lock (PhraseRequests)
            {
                if (PhraseRequests.Contains(guid))
                {
                    result = true;
                }
            }

            return result;
        }

        public static void AddRequest(string guid, string phrase)
        {
            lock(PhraseRequests)
            {
                if (PhraseRequests.Count >= MaxDepth)
                {
                    PhraseRequests.RemoveAt(0);
                }

                PhraseRequests.Add(guid, new RequestEntry(phrase));
            }
        }

        // validate user input
        public static bool ValidateUserInput (string guid, string phrase)
        {
            bool result = false;
            string originalPhrase = string.Empty;

            lock (PhraseRequests)
            {
                if (PhraseRequests.Contains(guid))
                {
                    originalPhrase = ((RequestEntry)PhraseRequests[guid]).Phrase;
                }

                if (!string.IsNullOrWhiteSpace(originalPhrase) && !string.IsNullOrWhiteSpace(phrase))
                {
                    originalPhrase = originalPhrase.ToUpper();
                    originalPhrase = Regex.Replace(originalPhrase, @"\s+", "");

                    string comparisonPhrase = phrase.ToUpper();
                    comparisonPhrase = Regex.Replace(comparisonPhrase, @"\s+", "");

                    result = comparisonPhrase == originalPhrase;

                    int errorCount = 0;

                    // if failed, increment the count - we watch this to prevent a brute-force attack
                    // which we handle by no longer taking the request seriously
                    if (result)
                    {
                        ((RequestEntry)PhraseRequests[guid]).Validated = true;
                    }
                    else
                    {
                        errorCount = ((RequestEntry)PhraseRequests[guid]).ErrorCount++;
                    }

                    // remove entry on too many failed
                    if (errorCount >= MaxFailures)
                    {
                        PhraseRequests.Remove(guid);
                    }           
                }
            } // lock(PhraseRequests)

            return result;
        }

        // validates that the captcha guid represents a request whose user input was found to be correct
        // entry is deleted after this is called, otherwise you could keep reusing the guid
        public static bool IsValidatedGuid(string guid)
        {
            bool result = false;
            lock (PhraseRequests)
            {
                if (PhraseRequests.Contains(guid))
                {
                    result = ((RequestEntry)PhraseRequests[guid]).Validated;
                }
            }

            DisposeEntry(guid);

            return result;
        }

        public static void DisposeEntry(string guid)
        {
            lock (PhraseRequests)
            {
                if (PhraseRequests.Contains(guid))
                {
                    PhraseRequests.Remove(guid);
                }
            }                    
        }

        public static string GenerateGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
