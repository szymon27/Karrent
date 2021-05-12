using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace Karrent
{
    static class Extensions
    {
        public static BitmapImage ToBitmapImage(this byte[] byteArray)
        {
            MemoryStream memoryStream = new MemoryStream(byteArray);
            Image image = Image.FromStream(memoryStream);
            Bitmap bitmap = new Bitmap(image);
            memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            memoryStream.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.EndInit();
            return bitmapImage;
        }
    }
}
