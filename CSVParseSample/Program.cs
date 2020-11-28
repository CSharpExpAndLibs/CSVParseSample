using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CSVParserLib;

namespace CSVParseSample
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:CSVParseSample Filename");
                goto END;
            }
            (List<string[]> list, string errMsg) = CSVParser.Parse(args[0]);
            if (list == null)
            {
                Console.WriteLine("Error:" + errMsg);
                goto END;
            }

            foreach (string[] fields in list)
            {
                foreach (string f in fields)
                {
                    Console.Write($"{f} ");
                }
                Console.WriteLine();
            }
        END:
            Console.WriteLine("Press Any Key");
            Console.ReadLine();
        }
    }
}
