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
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KeyholeCaptcha.Core
{
    public class RandomnessProvider
    {
        // Since captcha is prone to tampering, we want to avoid using a predictable hash-generated number
        private static RNGCryptoServiceProvider RandomProvider = new RNGCryptoServiceProvider();

        // Higher-performance psueudo-random generator
        private static Random PseudoRandomProvider = new Random();

        public static int RandFromRange(int lower, int upper)
        {
            int range = upper - lower;
            byte[] bytes = new byte[sizeof(UInt32)];
            RandomProvider.GetBytes(bytes);
            return (int)(BitConverter.ToUInt32(bytes, 0) % (UInt32)range) + lower;
        }

        public static int PseudoRandFromRange(int lower, int upper)
        {
            int range = upper - lower;
            int random = 0;
            // Random is not threadsafe, when it fails, it begins returning the same number over and over
            lock (PseudoRandomProvider)
            {
                random = PseudoRandomProvider.Next(10000);
            }
            return (random % range) + lower;
        }

        public static char RandomAlphaNumericChar()
        {
            char [] forbiddenChars = {'\0', '0', 'O', 'I', '1'}; // some fonts render these indistinguishable
            char result = '\0';

            while (forbiddenChars.Contains(result))
            {
                int rand = RandFromRange(0, 36);
                if (rand < 10)
                {
                    result = (char)(rand + 48);
                }
                else
                {
                    result = (char)(rand + 55);
                }
            }

            return result;
        }

        // Generate n-length string of random characters in the range [A-Z|0-9]
        public static string RandomAlphaNumericString(int length)
        {
            StringBuilder result = new StringBuilder();
            // 0-9 == 48-57, A-Z == 65-90

            for (int i = 0; i < length; i++ )
            {
                result.Append(RandomAlphaNumericChar());
            }
            
            return result.ToString();
        }
    }
}
