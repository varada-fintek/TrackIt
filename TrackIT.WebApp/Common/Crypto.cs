using System;
using System.Configuration;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace TrackIT.WebApp.Common
{
    public sealed class Crypto
    {
        private Crypto()
        {
        }

        public static string EncryptData(string stringToEncrypt)
        {
            return Cryptographer.EncryptSymmetric("SymRagasiyam", stringToEncrypt);
        }

        public static string DecryptData(string stringToDecrypt)
        {
            return Cryptographer.DecryptSymmetric("SymRagasiyam", stringToDecrypt);
        }

        public static byte[] CreateHash(string stringToHash)
        {
            byte[] valueToHash = Encoding.UTF8.GetBytes(stringToHash);
            return Cryptographer.CreateHash("HashRagasiyam", valueToHash);
        }
        public static bool CompareHash(byte[] stringToCompare, byte[] stringHash)
        {
            return Cryptographer.CompareHash("HashRagasiyam", stringToCompare, stringHash);            
        }
    }
}
