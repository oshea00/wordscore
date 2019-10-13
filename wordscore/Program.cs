using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace wordscore
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine(@"Usage: [wordcount | dotnet run] pathToDictionary\file.txt");
                return;
            }

            var words = File.ReadAllLines(args[0]);

            Console.WriteLine("Searching dictionary (could take a minute or two...)");
            var finder = new SuperWordFinder(words);
            finder.FindSupers();
            finder.ShowResults();
        }
    }


    class DescOrder : IComparer<(string histStr, int letters)>
    {
        public int Compare((string histStr, int letters) x, 
                        (string histStr, int letters) y) => y.histStr.CompareTo(x.histStr);
    }

}
