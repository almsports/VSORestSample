using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace RestSample
{
    public static class Helper
    {
        public static string PromptForUsername()
        {
            Console.WriteLine("Enter username: ");
            return Console.ReadLine();
        }
        public static string PromptForPassword()
        {
            Console.WriteLine("Enter password: ");
            return GetConsolePassword();            
        }

        /// <summary>

        /// Gets the console secure password.

        /// </summary>

        /// <returns></returns>

        private static string GetConsolePassword()
        {
            StringBuilder sb = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }

                if (cki.Key == ConsoleKey.Backspace)
                {
                    if (sb.Length > 0)
                    {
                        Console.Write("\b\0\b");
                        sb.Length--;
                    }
                    continue;
                }

                Console.Write('*');
                sb.Append(cki.KeyChar);
            }
            return sb.ToString();            
        }
    }
}
