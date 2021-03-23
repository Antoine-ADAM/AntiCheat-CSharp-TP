using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace Basics
{
    public class Basics
    {
        //"testjks"//fdjl";
        //*test*/ytrytre/*ytrytre
        /// <summary>
        /// As the name states it apply a filter function on each pixel of the image
        /// </summary>
        /// <param name="image"> The image to modify</param>
        /// <param name="filter"> The function to apply on each pixel</param>
        public static Bitmap ApplyFilter(Bitmap image, Func<Color, Color> filter)
        {
            Bitmap res = new Bitmap(image.Width, image.Height);
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < image.Width; x++)
                    res.SetPixel(x,y,filter(image.GetPixel(x,y)));
            return res;
        }
        /// <summary>
        /// A Black and White filter
        /// </summary>
        /// <param name="color"> The color to modify </param>
        /// <returns> The new color</returns>
        public static Color BlackAndWhite(Color color)
        {
            return (color.R + color.G + color.B) / 3 > 127 ? Color.FromArgb(255,255,255):Color.FromArgb(0,0,0);
        }

        /// <summary>
        /// A Yellow filter
        /// </summary>
        /// <param name="color"> The color to modify </param>
        /// <returns> The new color</returns>
        public static Color Yellow(Color color)
        {
            return Color.FromArgb(color.R,color.G,0);
        }

        /// <summary>
        /// A Grayscale filter
        /// </summary>
        /// <param name="color"> The color to modify </param>
        /// <returns> The new color</returns>
        public static Color Grayscale(Color color)
        {
            int n = (int)(color.R * 0.21 + color.G * 0.72 + color.B * 0.07);
            return Color.FromArgb(n,n,n);

        }

        /// <summary>
        /// A Negative filter
        /// </summary>
        /// <param name="color"> The color to modify </param>
        /// <returns> The new color</returns>
        public static Color Negative(Color color)
        {
            return Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
        }

        /// <summary>
        /// Remove the maxes of the composants of the color
        /// </summary>
        /// <param name="color"> The color to modify </param>
        /// <returns> The new color</returns>
        public static Color RemoveMaxes(Color color)
        {
            return Color.FromArgb(color.R>=color.G && color.R>=color.B?0:color.R,color.G>=color.R && color.G>=color.B?0:color.G,color.B>=color.G && color.B>=color.R?0:color.B);
        }

        /// <summary>
        /// Create the new_image as if the image was seen in a mirror
        /// ....o.  =>  .o....
        /// ...o..  =>  ..o...
        /// ..o...  =>  ...o..
        /// .o....  =>  ....o.
        /// o.....  =>  .....o
        /// </summary>
        /// <param name="image"> The image to 'mirror'</param>
        /// <returns> The new image</returns>
        public static Bitmap Mirror(Bitmap image)
        {
            Bitmap res = new Bitmap(image);
            int demiWidth=(image.Width / 2);
            for (int y = 0; y < image.Height; y++)
                for (int x = 0; x < demiWidth; x++)
                {
                    Color t = res.GetPixel(x, y);
                    res.SetPixel(x, y, res.GetPixel( res.Width-x-1,y));
                    res.SetPixel(res.Width-x-1,y,t);
                }
            return res;
        }

        /// <summary>
        /// Apply a right rotation
        /// </summary>
        /// <param name="image"> The image to rotate</param>
        /// <returns> The new_image</returns>
        public static Bitmap RotateRight(Bitmap image)
        {
            Bitmap res = new Bitmap(image.Height, image.Width);
            for (int y = 0; y < image.Height; y++)
            for (int x = 0; x < image.Width; x++)
                res.SetPixel(y,x,image.GetPixel(x,image.Height-y-1));
            return res;
        }

        /// <summary>
        /// <!> Bonus <!>
        /// Rotate to the right n times
        /// </summary>
        /// <param name="image"> The image to rotate</param>
        /// <param name="n"> Number of rotation (n can be negative and thus must be handled properly)</param>
        /// <returns> The new_image</returns>
        public static Bitmap RotateN(Bitmap image, int n)
        {
            Bitmap res;
            switch (n%4)
            {
                case 1:
                case -3:
                    res = RotateRight(image);
                    break;
                case 2:
                case -2:
                    res = new Bitmap(image.Width, image.Height);
                    for (int y = 0; y < image.Height; y++)
                    for (int x = 0; x < image.Width; x++)
                        res.SetPixel(x,y,image.GetPixel(image.Width-x-1,image.Height-y-1));
                    break;
                case 3:
                case -1:
                    res = new Bitmap(image.Height, image.Width);
                    for (int y = 0; y < image.Height; y++)
                    for (int x = 0; x < image.Width; x++)
                        res.SetPixel(y,x,image.GetPixel(image.Width-x-1,y));
                    break;
                default:
                    res = new Bitmap(image);
                    break;
            }
            return res;
        }
    }
}
