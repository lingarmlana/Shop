using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamusTestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Нажмите любую клавишу для эмуляции 30 дней работы магазина.");
            Console.ReadKey();
            Shop myshop = new Shop();
            myshop.one_month();
            Console.ReadKey();
        }
    }
}