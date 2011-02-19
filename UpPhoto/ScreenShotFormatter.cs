using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;

namespace FacebookController
{
    public class ScreenShotFormatter
    {
        private int positionX;
        private int positionY;
        private int width;
        private int height;
        private byte[] photo;

        public byte[] Photo
        {
            get
            {
                return photo;
            }
        }

        private static Bitmap bmpScreenshot;
        private static Graphics gfxScreenshot;

        public ScreenShotFormatter(int x, int y, int width, int height)
        {
            positionX = x;
            positionY = y;
            this.width = width;
            this.height = height;
        }

        public byte[] GetScreenShot()
        {

            // Set the bitmap object to the size of the screen
            bmpScreenshot = new Bitmap(width,
                height,
                PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            Size screenShotSize = new Size(width,
                height);

            // Take the screenshot from the upper left corner to the right bottom corner
            gfxScreenshot.CopyFromScreen(positionX,
                positionY,
                0,
                0,
                screenShotSize,
                CopyPixelOperation.SourceCopy);

            MemoryStream ms = new MemoryStream();

            // Save the screenshot to the specified path that the user has chosen
            bmpScreenshot.Save(ms, ImageFormat.Png);

            return ms.ToArray();
        }
    }
}
