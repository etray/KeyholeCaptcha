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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyholeCaptcha.Core.Covers
{
    public class DotCover : Animatable
    {
        private Point StartingDotPosition { get; set; }
        private int DotDistance { get; set; }

        override public void Init()
        {
            // decide starting dot position
            StartingDotPosition = new Point(RandomnessProvider.RandFromRange(0, 3), RandomnessProvider.RandFromRange(0, 3));
        }

        override public void Update()
        {
            // move starting dot position?
        }

        override public void Draw(Image img)
        {
            int dotSpacing = 5;
            int dotWidth = 2;

            Pen pen = new Pen(Color.DarkSlateGray, dotWidth);
            for (int y = 0; y < img.Height; y += dotSpacing)
            {
                int shift = (y % 2 != 0 ? dotSpacing / 2 : 0);
                for (int x = 0 + shift; x < img.Width; x += dotSpacing)
                {
                    ImageManager.DrawDot(img, x, y, pen);
                }
            }
        }
    }
}
