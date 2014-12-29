using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Enumerations
{
    public static class BuiltInSecureStoreCredentialType
    {
        public static string UserName = "UserName";
        public static string Password = "Password";
        public static string Pin = "Pin";
        public static string Key = "Key";
        public static string Generic = "Generic";

        public static string WindowsUserName = "WindowsUserName";
        public static string WindowsPassword = "WindowsPassword";
        public static string Certificate = "Certificate";
        public static string CertificatePassword = "CertificatePassword";
    }
}
