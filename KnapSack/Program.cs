using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnapSack
{
    class Program
    {
        static Knapsack knapsack;
        //static List<Tuple<int, int>> knapsackInstance;
        static void Main(string[] args)
        {
            LoadKnapSackInstance();
            //LoadKnapSackTestInstance();
            Console.WriteLine (knapsack.SolveKnapSack());
            Console.ReadKey();
        }
        private static void LoadKnapSackInstance()
        {
            //test file op=134365
            StreamReader graphFile = new StreamReader(@"C:\Users\khyati\Documents\GitHub\Coursera\KnapSack\knapsack1.txt");
            var t1 = from line in graphFile.Lines()
                     let items = line.Split(' ')
                     select new { Value = int.Parse(items[0]), Weight = int.Parse(items[1]) };
            
            List<int> vals = new List<int> ();
            List<int> wts = new List<int> ();
            foreach (var t in t1)
            {
                vals.Add(t.Value);
                wts.Add(t.Weight);
            }
            int constraint = vals[0];
            int size = wts[0];
            vals.RemoveAt(0);
            wts.RemoveAt(0);
            knapsack = new Knapsack(vals.ToArray(), wts.ToArray(), constraint, size);            
        }

        private static void LoadKnapSackTestInstance()
        {
            //test file op=134365
            StreamReader graphFile = new StreamReader(@"C:\Users\khyati\Documents\GitHub\Coursera\KnapSack\utc_knapsack1.txt");
            var t1 = from line in graphFile.Lines()
                     let items = line.Split(' ')
                     select new { Value = int.Parse(items[1]), Weight = int.Parse(items[0]) };

            List<int> vals = new List<int>();
            List<int> wts = new List<int>();
            foreach (var t in t1)
            {
                vals.Add(t.Value);
                wts.Add(t.Weight);
            }
            int constraint = vals[0];
            int size = wts[0];
            vals.RemoveAt(0);
            wts.RemoveAt(0);
            knapsack = new Knapsack(vals.ToArray(), wts.ToArray(), constraint, size);
        }

    }
    public static class StreamReaderSequence
    {
        public static IEnumerable<string> Lines(this StreamReader source)
        {
            String line;

            if (source == null)
                throw new ArgumentNullException("source");
            while ((line = source.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }
}
