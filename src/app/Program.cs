using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESGroupUpdator
{
    class Program
    {
        static void Main(string[] args)
        {
            var extractor = new Extractor();
            var indexer = new Indexer();

            Console.WriteLine("Fetching...");

            var itemsToIndex = extractor.GetGroups();

            indexer.Index(itemsToIndex);

            Console.WriteLine("Press any key to exit (when indexing is complete)");
            Console.ReadLine();
        }
    }
}
