using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    public class UniqueCode
    {
        public readonly string EncryptParameter = "SecretParameter";
    }
    public class CustomIDataProtection
    {
        private readonly IDataProtector protector;
        public CustomIDataProtection(IDataProtectionProvider dataProtectionProvider, UniqueCode uniqueCode)
        {
            protector = dataProtectionProvider.CreateProtector(uniqueCode.EncryptParameter);
        }
        public string Encode(string data)
        {
            return protector.Protect(data);
        }
        public string Decode(string data)
        {
            try
            {
                return protector.Unprotect(data);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
