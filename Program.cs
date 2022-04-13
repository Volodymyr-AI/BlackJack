using System;
using System.Text;

namespace BlackJack
{
    internal class Program
    {
        

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8; // won't see ♠♥♣♦ without it

            Console.WriteLine("Welcome to BlackJack♠♥♣♦");
            Console.WriteLine("Press <Enter> to start");
            Console.ReadKey();
            GameStart();
        }
    }
}
