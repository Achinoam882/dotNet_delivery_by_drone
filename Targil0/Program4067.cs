using System;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome4067();
            Welcome4931();
            Console.ReadKey();
        }
        static partial void Welcome4931();
        private static void Welcome4067()
        {
            Console.WriteLine("Enter your name: ");
            String str = Console.ReadLine();
            Console.WriteLine("{0},welcome to my first console application", str);
        }
    }
}