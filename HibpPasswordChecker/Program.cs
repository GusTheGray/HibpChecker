using HibpPasswordChecker.Client;
using System;

namespace HibpPasswordChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to password checker. Please enter a string (Ctrl+C) to exit");
            var input = Console.ReadLine();

            var client = new HibpClient();

            var has = client.PasswordHasBeenPwned(input);
        }
    }
}
