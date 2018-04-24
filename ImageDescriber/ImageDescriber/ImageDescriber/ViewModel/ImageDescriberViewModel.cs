using ImageDescriber.Converters;
using ImageDescriber.Model;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ImageDescriber.ViewModel
{
    public class ImageDescriberViewModel
    {
        FormView ViewContext { get; set; }

        public ImageDescriberViewModel(FormView context)
        {
            this.ViewContext = context;
        }

        public void LoadImage(object sender, EventArgs e)
        {
            var result = ViewContext.FileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    using (var stream = ViewContext.FileDialog.OpenFile())
                    {
                        if (stream != null)
                        {
                            var image = new Bitmap(stream);
                            ViewContext.PictureBox.Image = image;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        public void ConvertToBlackWhite(object sender, EventArgs e)
        {
            if (!IsImageExist())
                return;

            var image = ViewContext.PictureBox.Image;
            var blackWhiteImage = MakeGrayscale3(image as Bitmap);
            ViewContext.PictureBox.Image = blackWhiteImage;
        }

        private bool IsImageExist()
        {
            var image = ViewContext.PictureBox.Image;
            if (image == null)
                return false;
            return true;
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        public void ConvertToBinary(object sender, EventArgs e)
        {
            if (!IsImageExist())
                return;

            var image = ViewContext.PictureBox.Image;
            var rgbtohsv = ChangeRGBToHSV(image as Bitmap);
            //var binaryImage = MakebinaryImage(image as Bitmap);
            ViewContext.PictureBox.Image = rgbtohsv;

        }

        public Bitmap ChangeRGBToHSV(Bitmap original)
        {
           
             var image = ViewContext.PictureBox.Image as Bitmap;
            // Bitmap image = new Bitmap(original.Width, original.Height);
            int[,] imageCode = new int[image.Width, image.Height];
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color c = image.GetPixel(i, j);
                  //  byte red = c.R;
                  //  byte green = c.G;
                  //  byte blue = c.B;
                 //   HSV value = ColorConverters.RGBToHSV(new RGB(red, green, blue));
                    if (c.GetHue() > 0 && c.GetHue() < 150) //You can change the value
                    {
                        imageCode[i, j] = 1; //1 for White
                    }
                    else
                    {
                        imageCode[i, j] = 0; //0 for Black
                    }
                }
            }
            Bitmap map = new Bitmap(image.Width, image.Height);
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    if (imageCode[i, j] == 1)
                    {
                        map.SetPixel(i, j, Color.Black);

                    }
                    else
                    {
                        map.SetPixel(i, j, Color.White);
                    }
                }
            }
            return map;
        }

        
    }
}