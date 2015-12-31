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
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyholeCaptcha.Core
{
    public class GifWriter
    {
        public static int defaultFrameDelayMilliseconds = 100;

        public void Write(Stream stream, IList<Image> images, int frameDelayMilliseconds)
        {
            // credit to Rick van den Bosch who originally demonstrated how to do this:
            // http://rick.bloggingabout.net/2005/05/10/howto-create-an-animated-gif-using-net-c/

            Byte[] applicationExtensionBlock = new Byte[19];
            applicationExtensionBlock[0] = 33;  // extension introducer
            applicationExtensionBlock[1] = 255; // application extension
            applicationExtensionBlock[2] = 11;  // size of block
            applicationExtensionBlock[3] = 78;  // N
            applicationExtensionBlock[4] = 69;  // E
            applicationExtensionBlock[5] = 84;  // T
            applicationExtensionBlock[6] = 83;  // S
            applicationExtensionBlock[7] = 67;  // C
            applicationExtensionBlock[8] = 65;  // A
            applicationExtensionBlock[9] = 80;  // P
            applicationExtensionBlock[10] = 69; // E
            applicationExtensionBlock[11] = 50; // 2
            applicationExtensionBlock[12] = 46; // .
            applicationExtensionBlock[13] = 48; // 0
            applicationExtensionBlock[14] = 3;  // Size of block
            applicationExtensionBlock[15] = 1;  // Loop == true
            applicationExtensionBlock[16] = 0;  // repetitions low byte (0 == continuous)
            applicationExtensionBlock[17] = 0;  // repetitions high byte
            applicationExtensionBlock[18] = 0;  // Block terminator

            Byte[] graphicalControlExtensionBlock = new Byte[8];
            graphicalControlExtensionBlock[0] = 33;     // Extension introducer
            graphicalControlExtensionBlock[1] = 249;    // Graphic control extension
            graphicalControlExtensionBlock[2] = 4;      // Size of block
            graphicalControlExtensionBlock[3] = 9;      // Flags: reserved, disposal method, user input, transparent color
            int hudredths = frameDelayMilliseconds / 10;
            graphicalControlExtensionBlock[4] = (byte)(hudredths & 0xff);           // Delay time low byte
            graphicalControlExtensionBlock[5] = (byte)((hudredths >> 8) & 0xff);    // Delay time high byte
            graphicalControlExtensionBlock[6] = 255;    // Transparent color index
            graphicalControlExtensionBlock[7] = 0;      // Block terminator

            MemoryStream memoryStream = new MemoryStream();
            Byte[] imageBuffer;

            using (BinaryWriter binaryWriter = new BinaryWriter(stream))
            {
                foreach (var image in images)
                {
                    image.Save(memoryStream, ImageFormat.Gif);
                    imageBuffer = memoryStream.ToArray();

                    if (images.IndexOf(image)==0)
                    {
                        binaryWriter.Write(imageBuffer, 0, 781); // Header & global color table
                        binaryWriter.Write(applicationExtensionBlock, 0, 19); // Application extension
                    }

                    binaryWriter.Write(graphicalControlExtensionBlock, 0, 8); // Graphic extension
                    binaryWriter.Write(imageBuffer, 789, imageBuffer.Length - 790); // Image data

                    if (images.IndexOf(image)==images.Count - 1)
                    {
                        binaryWriter.Write(";"); // Image terminator
                    }
                    memoryStream.SetLength(0);
                }
            }
        }
    }
}
