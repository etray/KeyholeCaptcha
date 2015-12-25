using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeyholeCaptcha.Core;
using System.Collections.Generic;
using System.IO;

namespace UnitTest.Core
{
    [TestClass]
    public class GifWriterTest
    {
        [TestMethod]
        public void WriteTest()
        {
            string gifFile = "out.gif";
            int imgWidth = 175;
            int imgHeight = 70;

            if (File.Exists(gifFile))
            {
                File.Delete(gifFile);
            }

            GifWriter writer = new GifWriter();
            IList<Image> images = new List<Image>();

            for (int i = 0; i < 10; i++)
            {
                Image img = new Bitmap(imgWidth, imgHeight);
                ImageManager.ClearBackground(img, ImageManager.defaultBackgroundColor);
                ImageManager.DrawText(img, "word" + i, 10 + i, 10, ImageManager.defaultFont, ImageManager.defaultTextColor);
                images.Add(img);
            }

            FileStream outFile = new FileStream(gifFile, FileMode.Create);

            writer.Write(outFile, images, GifWriter.defaultFrameDelayMilliseconds);

            outFile.Close();

            Assert.IsTrue(File.Exists(gifFile));
            Assert.IsTrue(new FileInfo(gifFile).Length > 0);

            Image inImage = Image.FromFile(gifFile);

            Assert.IsTrue(inImage.Width == imgWidth && inImage.Height == imgHeight);
        }
    }
}
