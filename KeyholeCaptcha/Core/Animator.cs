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
    public class Animator
    {       
        public Rectangle BoundingBox { get; set; }

        public Animator(int width, int height)
        {
            BoundingBox = new Rectangle(0, 0, width, height);
        }

        public IList<Image> Animate(int numFrames, IList<Animatable> layers)
        {
            IList<Image> frames = new List<Image>();
            InitLayers(layers);

            for (int i = 0; i < numFrames; i++)
            {
                Image image = new Bitmap(BoundingBox.Width, BoundingBox.Height);
                ImageManager.ClearBackground(image, ImageManager.defaultBackgroundColor);
                UpdateLayers(image, layers);
                DrawLayers(image, layers);
                frames.Add(image);
            }

            return frames;
        }

        private void InitLayers(IList<Animatable> layers)
        {           
            foreach (var layer in layers)
            {
                layer.Init();
            }
        }

        private void UpdateLayers(Image image, IList<Animatable> layers)
        {
            foreach (var layer in layers)
            {
                layer.Update();
            }
        }

        private void DrawLayers(Image image, IList<Animatable> layers)
        {
            foreach (var layer in layers)
            {
                layer.Draw(image);
            }
        }
    }
}
