using QRCoder;
using System.Drawing;

namespace Utility
{
    public class QRGenerator
    {
        public static byte[] QRImageGen(string ticket,string fullname,string qrcode)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(ticket +"\n"+fullname+"\n"+qrcode, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);
            Bitmap qrImage = qRCode.GetGraphic(20);
            //qrImage.Save(id, ImageFormat.Png);
            var byteImage = ImageConverter.ImageToByteArray(qrImage);
            return byteImage;
        }
    }
}
