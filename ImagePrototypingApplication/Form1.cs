using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KeyholeCaptcha.Core;
using KeyholeCaptcha.Core.PhraseGenerators;
using System.IO;

namespace ImagePrototypingApplication
{
    public partial class prototypingApplication : Form
    {
        private PhraseGenerator pg;
        private Image img;
        private string guid;

        public prototypingApplication()
        {
            InitializeComponent();
            pg = new WordListPhraseGenerator();
            img = new Bitmap(200, 50);
            ImageManager.ClearBackground(img, ImageManager.defaultBackgroundColor);
            loadDropdown();
            this.pictureBox1.Image = img;
            comboBox2.Text = "words";
            reload();
                
        }

        private void loadDropdown()
        {
            foreach (CaptchaMaker.CaptchaType type in Enum.GetValues(typeof(CaptchaMaker.CaptchaType)))
            {
                comboBox1.Items.Add(type.ToString());
            }
            comboBox1.Text = comboBox1.Items[0].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Validator.ValidateUserInput(guid, textBox1.Text))
            {
                textBox2.BackColor = Color.LightGreen;
                textBox2.Text = "correct";
            }
            else
            {
                textBox2.BackColor = Color.Salmon;
                textBox2.Text = "incorrect";
            }
        }

        private void reload()
        {
            textBox2.BackColor = System.Drawing.SystemColors.Control;
            textBox2.Text = string.Empty;

            CaptchaMaker maker = new CaptchaMaker();
            this.pictureBox1.Image.Dispose();

            string phrase = string.Empty;

            if (comboBox2.Text == "random")
            {
                phrase = RandomnessProvider.RandomAlphaNumericString(2) + " " + RandomnessProvider.RandomAlphaNumericString(1);
            }
            else if (comboBox2.Text == "words")
            {
                phrase = pg.RandomPhrase();
            }
            else // user specified
            {
                phrase = comboBox2.Text;
            }

            guid = Validator.Register();
            Validator.Refresh(guid, phrase);

            using (FileStream fileStream = new FileStream("captchaImage.gif", FileMode.Create))
            {
                maker.MakeCaptcha(fileStream, phrase, (CaptchaMaker.CaptchaType)Enum.Parse(
                    typeof(CaptchaMaker.CaptchaType), 
                    comboBox1.Text));
            }
            this.pictureBox1.Image = Image.FromFile(@"captchaImage.gif");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            reload();
        }
    }
}
