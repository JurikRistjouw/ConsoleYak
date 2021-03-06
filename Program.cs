using System;
using System.Globalization;
using YakHerd;

namespace ConsoleYak
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2 || !int.TryParse(args[1], out int _))
            {
                Console.WriteLine("usage: ConsoleYak.exe filename age");
                return;
            }

            var herd = Herd.ReadHerd(args[0]);
            herd.CalculateHerd(int.Parse(args[1]));
            DisplayTotalsInConsole(herd);
        }

        private static void DisplayTotalsInConsole(Herd herd)
        {
            Console.WriteLine("In Stock:");
            Console.WriteLine($"\t{herd.Milk.ToString("#.##0", CultureInfo.InvariantCulture)} liters of milk");
            Console.WriteLine($"\t{herd.Hides} skins of wool");
            Console.WriteLine("Herd:");
            foreach (LabYak yak in herd.LabYaks)
            {
                Console.WriteLine($"\t {yak.Name} {yak.Age.ToString("#.#0", CultureInfo.InvariantCulture)} years old");
            }

            Console.ReadKey();
        }

    }
}
