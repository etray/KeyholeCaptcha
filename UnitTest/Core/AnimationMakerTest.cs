using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using KeyholeCaptcha.Core;
using KeyholeCaptcha.Core.Covers;
using System.IO;
using KeyholeCaptcha.Core.PhraseGenerators;

namespace UnitTest.Core
{
    /// <summary>
    /// Summary description for AnimationMakerTest
    /// </summary>
    [TestClass]
    public class AnimationMakerTest
    {
        [TestMethod]
        public void AnimateTest()
        {
            PhraseGenerator generator = new WordListPhraseGenerator();
            int numFrames = 20;
            Rectangle imageBox = new Rectangle(0, 0, 200, 80);
            Animator maker = new Animator(imageBox.Width, imageBox.Height);
            maker.BoundingBox = imageBox;
            IList<Animatable> layers = new List<Animatable>();
            Animatable phrase = new Phrase(generator.RandomPhrase(), ImageManager.defaultFont);
            phrase.ImageBox = imageBox;
            phrase.NumberOfFrames = numFrames;
            phrase.DrawingColor = ImageManager.defaultTextColor;
            layers.Add(phrase);
            Animatable staticCover = new StaticCover();
            layers.Add(staticCover);
            IList<Image> images = maker.Animate(numFrames, layers);

            string gifFile = "AnimateTest.gif";
            GifWriter writer = new GifWriter();
            FileStream outFile = new FileStream(gifFile, FileMode.Create);
            writer.Write(outFile, images, GifWriter.defaultFrameDelayMilliseconds);
            outFile.Close();
        }
    }
}
