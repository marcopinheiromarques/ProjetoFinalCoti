using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Utilitario
{
    public class Criptografia
    {
        public static string Encriptar(string texto)
        {
            MD5    md5  = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(texto));

            return BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper();
        }
    }
}
