using DomainModel.Entities;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Utility
{
    public class QRGenerator
    {
        public static UserTicket QRImageGen(UserTicket ut)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(ut.Id.ToString() + " " + ut.User.FirstName + " " + ut.PurchaseDate, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);
            Bitmap qrImage = qRCode.GetGraphic(20);
            qrImage.Save(ut.Id + ut.User.FirstName, ImageFormat.Png);
            ut.QRCode = ImageConverter.ImageToByteArray(qrImage);
            return ut;
        }
    }
}
