using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApiCrackChecker.Helpers
{
    public class CypherHelper
    {
        public static String GenerarSalt()
        {
            Random r = new Random();
            String salt = "";
            for (int i = 0; i < 30; i++)
            {
                int numGenerado = r.Next(65, 122);
                char letra = Convert.ToChar(numGenerado);
                salt += letra;
            }
            return salt;
        }
        public static byte[] CifradoHashSHA256(String text)
        {
            UnicodeEncoding converter = new UnicodeEncoding();
            byte[] input = converter.GetBytes(text);
            byte[] output;
            SHA256Managed sha256 = new SHA256Managed();
            output = sha256.ComputeHash(input);
            return output;
        }

        public static bool CompararBytes(byte[] ar1, byte[] ar2)
        {
            UnicodeEncoding converter = new UnicodeEncoding();
            // Si no coincide la longitud de los arrays devolvemos false
            if (ar1.Length != ar2.Length)
            {
                return false;
            }
            else
            {
                // Si no hacemos un bucle por cada posición del array para comparar los bytes
                for (int i = 0; i < ar1.Length; i++)
                {
                    // Mientras coincidan nunca entrará aqui.
                    if (!(ar1[i].Equals(ar2[i])))
                    {
                        // Si no coinciden, devolvemos false
                        return false;
                    }
                }
                // Si realiza todo el bucle devolvemos true.
                return true;
            }
        }
    }
}
