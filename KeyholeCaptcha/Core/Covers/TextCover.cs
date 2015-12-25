// Copyright (c) 2015 - Elton T. Ray
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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyholeCaptcha.Core.Covers
{
    class TextCover : Animatable
    {
        override public void Init()
        {

        }

        override public void Update()
        {

        }

        override public void Draw(Image img)
        {
            Font font = new Font("monospace", 10);
            Rectangle dimensions = ImageManager.TextRectangle(img, "X", font);

            for (int y = 0; y < img.Height; y += dimensions.Height)
            {
                for (int x=0;x<img.Width;x+=dimensions.Width)
                {
                    ImageManager.DrawText(img, "" + RandomnessProvider.RandomAlphaNumericChar(), x, y, font, Color.DarkSlateGray);
                }
            }
            font = new Font("monospace", 10);
            dimensions = ImageManager.TextRectangle(img, "X", font);
            for (int y = dimensions.Height / 2; y < img.Height; y += dimensions.Height)
            {
                for (int x = dimensions.Width / 2; x < img.Width; x += dimensions.Width)
                {
                    ImageManager.DrawText(img, "" + RandomnessProvider.RandomAlphaNumericChar(), x, y, font, Color.DarkSlateGray);
                }
            }

            font = new Font("monospace", 14);
            dimensions = ImageManager.TextRectangle(img, "X", font);
            for (int y = dimensions.Height / 4; y < img.Height; y += dimensions.Height)
            {
                for (int x = dimensions.Width / 4; x < img.Width; x += dimensions.Width)
                {
                    ImageManager.DrawText(img, "" + RandomnessProvider.RandomAlphaNumericChar(), x, y, font, Color.SlateGray);
                }
            }
        }
    }
}