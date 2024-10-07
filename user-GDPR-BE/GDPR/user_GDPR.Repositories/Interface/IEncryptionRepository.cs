using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace user_GDPR.Services.Interface
{
    public interface IEncryptionRepository
    {
        string EncryptString(string plainText);
        string DecryptString(string cipherText);
    }
}
