using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Interface
{
    public interface IEmailService
    {
        void SendQRCode(UserTicket userTicket);
    }
}
