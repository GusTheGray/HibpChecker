using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace HibpPasswordChecker.Client
{

    public class HibpClient
    {
        public static HttpClient _client;
        private const string url = "https://api.pwnedpasswords.com/range/";

        static HibpClient()
        {
            _client = new HttpClient();
        }

        public bool PasswordHasBeenPwned(string password)
        {
            (string prefix, string suffix) = Sha1Operation(password);

            var result = _client.GetAsync($"{url}/{prefix}").Result;
            var response = result.Content.ReadAsStringAsync().Result;

            var list = response.Split(Environment.NewLine);

            var hit = Array.Find(list, e => e.StartsWith(suffix));

            if(hit != null)
            {
                var num = hit.Split(':')[1];
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine($"password has appeared in {num} breaches");
                Console.ResetColor();
                return true;
            }

            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine($"Password has not appeared in any breaches");
            Console.ResetColor();
            return false;
        }

        private (string, string) Sha1Operation(string password)
        {
            byte[] hashBytes;
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            var hashStr = BitConverter.ToString(hashBytes).Replace("-", "");

            return (hashStr.Substring(0, 5), hashStr.Substring(5));
        }
    }
}
