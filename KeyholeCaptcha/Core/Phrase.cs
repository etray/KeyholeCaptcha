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

namespace KeyholeCaptcha.Core
{
    public class Phrase : Animatable
    {
        private int[] xPositions;
        private int[] yPositions;
        private Image internalImage;
        private int CurrentPostion { get; set; }
        public bool RandomTextPosition { get; set; }

        public Phrase(string phrase, Font font)
        {
            Text = phrase;
            Font = font;
            RandomTextPosition = true;
        }

        override public void Init()
        {
            xPositions = new int[NumberOfFrames];
            yPositions = new int[NumberOfFrames];
            internalImage = new Bitmap(ImageBox.Width, ImageBox.Height);
            BoundingBox = ImageManager.TextRectangle(internalImage, Text, Font);
            BoundingBox.Offset(0 - BoundingBox.X, 0 - BoundingBox.Y);

            int midX = (ImageBox.Width / 2 - BoundingBox.Width / 2);
            int midY = ImageBox.Height / 2 - BoundingBox.Height / 2;
            int halfCycleLength = NumberOfFrames / 2;

            int xIncrement = ((midX + midX / 4) - (midX - midX / 4)) / halfCycleLength;
            int yIncrement = 0;

            int randomXOffset = 0;
            int randomYOffset = 0;

            if (RandomTextPosition)
            {
                randomXOffset = RandomnessProvider.RandFromRange(-7, 7);
                randomYOffset = RandomnessProvider.RandFromRange(-7, 7);
            }

            int x = (midX - midX / 4) + randomXOffset;
            int y = midY + randomYOffset;

            int endPos = NumberOfFrames - 1;
            for (int i = 0; i < (halfCycleLength + 1); i++)
            {
                x += xIncrement;
                y += yIncrement;

                xPositions[i] = x;
                yPositions[i] = y;

                // to achieve a smooth loop, end frames mirror beginning frames
                if (i > 0)
                {
                    xPositions[endPos] = x;
                    yPositions[endPos] = y;
                    endPos--;
                }
            }

            CurrentPostion = -1;
        }

        override public void Update()
        {
            CurrentPostion++;
            if (CurrentPostion >= xPositions.Length)
            {
                CurrentPostion = 0;
            }
            SetPos(xPositions[CurrentPostion], yPositions[CurrentPostion]);
        }

        override public void Draw(Image img)
        {
            ImageManager.DrawText(img, Text, BoundingBox.X, BoundingBox.Y, Font, DrawingColor);
        }

        private void SetPos(int x, int y)
        {
            BoundingBox = new Rectangle(x,y,BoundingBox.Width,BoundingBox.Height);
        }
    }
}
