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
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyholeCaptcha.Core
{
    public class ImageManager
    {
        public static Color defaultTextColor = Color.DarkSlateGray;
        public static Color defaultBackgroundColor = Color.White;
        public static Font defaultFont = new Font("sans-serif", 25);
        public static Pen defaultPen = new Pen(Color.DarkSlateGray, 3);

        // Render Text at a certain position
        public static void DrawText(Image image, string text, float x, float y, Font font, Color color)
        {
            Brush brush = new SolidBrush(color);
            PointF point = new PointF(x, y);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                graphics.DrawString(text, font, brush, point);
            }
        }

        // calculate rectangle bounding box for specified text
        public static Rectangle TextRectangle(Image img, string text, Font font)
        {
            Rectangle result = new Rectangle();
            
            using (Graphics graphics = Graphics.FromImage(img))
            {
                SizeF size = graphics.MeasureString(text, font);
                result.X = 0;
                result.Y = 0;
                result.Width = (int)size.Width;
                result.Height = (int)size.Height;
            }

            return result;
        }

        // clear background
        public static void ClearBackground(Image image, Color color)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.Clear(color);
            }
        }

        // draw line on image
        public static void DrawLine(Image image, int x1, int y1, int x2, int y2, Pen pen)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        // draw dot on image
        public static void DrawDot(Image image, int x, int y, Pen pen)
        {
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                graphics.DrawEllipse(pen, x, y, pen.Width, pen.Width);
            }
        }
    }
}
