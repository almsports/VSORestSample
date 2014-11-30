using System;
using System.Collections.Generic;
using System.Linq;
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
            return Console.ReadLine();
        }
    }
}
