using QRCoder;
using System.Drawing;

namespace Utility
{
    public class QRGenerator
    {
        public static byte[] QRImageGen(string id,string username,string url)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(url+"?d="+id+"&u="+username, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);
            Bitmap qrImage = qRCode.GetGraphic(20);
            //qrImage.Save(id, ImageFormat.Png);
            var byteImage = ImageConverter.ImageToByteArray(qrImage);
            return byteImage;
        }
    }
}
