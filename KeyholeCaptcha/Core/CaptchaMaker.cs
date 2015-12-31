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

using KeyholeCaptcha.Core.Covers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyholeCaptcha.Core
{
    public class CaptchaMaker
    {
        // TODO: make globally configurable:
        // font
        // text size
        // background color
        // text color
        // overlay color
        // vertical line width
        // animation length in frames
        // image width
        // image height
        //KeyholeCaptcha.Properties.Settings.Default.WordListPath;
        // validation queue depth
        // random phrase length

        // TODO: make configurable
        public static int defaultWidth = 200;
        public static int defaultHeight = 50;

        public int NumberOfFrames = 20;
        public Rectangle ImageBox = new Rectangle(0, 0, defaultWidth, defaultHeight);

        public enum CaptchaType { 
            CoveredByStatic,
            CoveredByRandomLetters,
            CoveredByBars,
            CoveredByDots
        }

        public void MakeCaptcha(Stream outStream, string phrase, CaptchaType type)
        {
            IList<Animatable> layers = new List<Animatable>();
            Animatable animationPhrase;
            Animatable coverLayer;

            switch(type)
            {
                case CaptchaType.CoveredByRandomLetters:
                    animationPhrase = new Phrase(phrase, ImageManager.defaultFont);
                    animationPhrase.ImageBox = ImageBox;
                    animationPhrase.NumberOfFrames = NumberOfFrames;
                    animationPhrase.DrawingColor = ImageManager.defaultTextColor;
                    layers.Add(animationPhrase);
                    coverLayer = new TextCover();
                    layers.Add(coverLayer);
                    break;
                case CaptchaType.CoveredByStatic:
                    animationPhrase = new Phrase(phrase, ImageManager.defaultFont);
                    animationPhrase.ImageBox = ImageBox;
                    animationPhrase.NumberOfFrames = NumberOfFrames;
                    animationPhrase.DrawingColor = ImageManager.defaultTextColor;
                    layers.Add(animationPhrase);
                    coverLayer = new StaticCover();
                    layers.Add(coverLayer);
                    break;
                case CaptchaType.CoveredByBars:
                    animationPhrase = new Phrase(phrase, ImageManager.defaultFont);
                    animationPhrase.ImageBox = ImageBox;
                    animationPhrase.NumberOfFrames = NumberOfFrames;
                    animationPhrase.DrawingColor = ImageManager.defaultTextColor;
                    layers.Add(animationPhrase);
                    coverLayer = new LineCover();
                    layers.Add(coverLayer);
                    break;
                case CaptchaType.CoveredByDots:
                    animationPhrase = new Phrase(phrase, ImageManager.defaultFont);
                    animationPhrase.ImageBox = ImageBox;
                    animationPhrase.NumberOfFrames = NumberOfFrames;
                    animationPhrase.DrawingColor = ImageManager.defaultTextColor;
                    layers.Add(animationPhrase);
                    coverLayer = new DotCover();
                    layers.Add(coverLayer);
                    break;
                default:
                    throw new Exception("Unknown CaptchaType");
            }

            Animator maker = new Animator(ImageBox.Width, ImageBox.Height);
            IList<Image> images = maker.Animate(NumberOfFrames, layers);

            GifWriter writer = new GifWriter();
            writer.Write(outStream, images, GifWriter.defaultFrameDelayMilliseconds);
        }
    }
}
