using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace Utility
{
    public class ImageConverter
    {
        /*public static byte[] ImageToByteArray(IFormFile imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.CopyTo(ms);
                return ms.ToArray();
            }
        }*/
        public static byte[] ImageToByteArray(string sPath)
        {
            //Open FileStream to read file
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            //When you use BinaryReader, you need to supply number of bytes 
            //to read from file.
            //In this case we want to read entire file. 
            //So supplying total number of bytes.
            using (var ms = new MemoryStream())
            {
                fStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        public static byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            using (var ms = new MemoryStream(byteArrayIn))
            {
                var returnImage = Image.FromStream(ms);

                return returnImage;
            }
        }
    }
}
