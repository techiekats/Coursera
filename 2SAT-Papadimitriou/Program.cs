using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SAT_Papadimitriou
{
    class Program
    {
        private static int n;
        private static List<Tuple<int, int>> clauses;
        private static bool?[] variables;
        
        static void Main(string[] args)
        {
            clauses = new List<Tuple<int, int>>();
            Load2SATProblem("2sat-2.txt");
            //RemoveRedundantClauses();
            var clauseRemovalResult = RemoveRedundantClausesAsync();
            if (clauseRemovalResult.Result == true)
            {
                //solve the 2sat problem
            }
            else
            {
                Console.WriteLine("Given is an unsatisfiable problem");
            }
            Console.ReadKey();
        }

        private static bool RemoveRedundantClauses()
        {
            for (int i = 1; i <= 10; i++)
            {
                var constrastingClauses = clauses
                    .Select(t1 => new Tuple<Tuple<int,int>, Tuple<int,int>> (t1, clauses.FindLast(t2=> 
                    ((t2.Item1 == t1.Item1) && (t2.Item2 == -t1.Item2)) || 
                    ((t2.Item1 == -t1.Item1) && (t2.Item2 == t1.Item2))
                    ))).Where (t=> t.Item2 != null).Distinct(new TuplePairComparer()).AsParallel().AsSequential().ToList(); 
                if (constrastingClauses.Count() == 0) break;
                foreach (var t in constrastingClauses)
                {
                   /* if (t.Item1 > 0)
                    {
                        variables[t.Item1 - 1] = true;
                    }
                    if (t.Item2 > 0)
                    {
                        variables[t.Item2 - 1] = true;
                    }*/
                }
                //clauses.RemoveAll(t => constrastingClauses.Contains(t));
                Console.WriteLine("Removed {0} clauses", constrastingClauses.Count());
                Console.WriteLine("Current count of clauses: " + clauses.Count);
            }
            return true;
        }
        private static async Task<bool> RemoveRedundantClausesAsync()
        {
            bool isUnsatConditionEncountered = false;
            int partitionsDesired = 2000, minPartitionSize=100;
            for (int i = 0; i < 4 && !isUnsatConditionEncountered; i++)
            {
                List<Task<List<int>>> tasks = new List<Task<List<int>>>();
                int partitionSize = clauses.Count / partitionsDesired;
                if (partitionSize < minPartitionSize)
                {
                    tasks.Add(ProcessPartitionForContrastingClauses(clauses));
                }
                else
                {
                    var clausePartitions = clauses.Partition(partitionSize);
                    var enumerator = clausePartitions.GetEnumerator(); 
                    while (enumerator.MoveNext ())
                    {
                        tasks.Add(ProcessPartitionForContrastingClauses(enumerator.Current));
                    }
                }
                try
                {
                    await Task.WhenAll(tasks.ToArray()).ConfigureAwait(false); 
                    List<int> removableClauseCandidates = new List<int>();
                    foreach (var t in tasks)
                    {
                        removableClauseCandidates = removableClauseCandidates.Union<int>(t.Result).ToList<int>();
                    }
                    removableClauseCandidates = removableClauseCandidates.Distinct().ToList<int>();
                    foreach (int c in removableClauseCandidates)
                    {
                        clauses.RemoveAll(t => t.Item1 == c || t.Item2 == c);
                    }
                } 
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                //1. about 5 times or termination condition encountered
                //2. Each time, create partition indices and send those to the Tasks
                //3. From tasks, obtain resultant numbers that have to be marked true or the fact that a conflicting condition exists
            }
                return true;
        }
        private static async Task<List<int>> ProcessPartitionForContrastingClauses (List<Tuple<int,int>> clausesPartition)
        {
            Console.WriteLine("Current clause count {0}", clausesPartition.Count);
            var constrastingClauses = clausesPartition
                .Select(t1 => new Tuple<Tuple<int, int>, Tuple<int, int>>(t1, clauses.FindLast(t2 =>
                ((t2.Item1 == t1.Item1) && (t2.Item2 == -t1.Item2)) ||
                ((t2.Item1 == -t1.Item1) && (t2.Item2 == t1.Item2))
                ))).Where(t => t.Item2 != null).Distinct(new TuplePairComparer()).ToList();
            if (constrastingClauses.Count() == 0)
            {
                Console.WriteLine("Contrasting clauses count is zero");
                return new List<int>();
            }
            Console.WriteLine("Contrasting clauses count {0}", constrastingClauses.Count);
            List<int> certainTrueVertices = new List<int>();
            foreach (var t in constrastingClauses)
            {
                int median = new int[] { t.Item1.Item1, t.Item1.Item2, t.Item2.Item1, t.Item2.Item1 }.OrderBy(i => i).ElementAt(1);
                Console.WriteLine("Choosing between set ({0} {1}), ({2},{3})\tChoosing value {4}",
                    t.Item1.Item1, t.Item1.Item2, t.Item2.Item1, t.Item2.Item2, median);
                certainTrueVertices.Add(median);
            }
            if (certainTrueVertices.Any (t=> certainTrueVertices.Contains (-t)))
            {
                throw new InvalidDataException ();
            }
            return certainTrueVertices;
        }
       
        private static void Load2SATProblem(string filePath)
        {

            StreamReader TwoSatInput = new StreamReader(filePath);
            var t1 = from line in TwoSatInput.Lines()
                     let items = line.Split(' ')
                     //where items.Length > 1
                     select items.Length > 1 ? new { LHS = int.Parse(items[0]), RHS = int.Parse(items[1]) }
                        : new { LHS = int.Parse(items[0]), RHS = int.MinValue };
            foreach (var p in t1)
            {
                if (p.RHS == int.MinValue)
                {
                    n = p.LHS;
                }
                else
                {   
                    clauses.Add(new Tuple<int, int>(p.LHS, p.RHS));
                }
            }
            variables = new bool?[n];
            variables = Enumerable.Repeat<bool?>(null, n).ToArray();
            Console.WriteLine(string.Format("Loaded {0} clauses for {1} variables", clauses.Count, n));
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

    public static class ClausePartitioner
    {
        public static IEnumerable<List<T>> Partition<T>(this IList<T> source, Int32 size)
        {
            for (int i = 0; i < Math.Ceiling(source.Count / (Double)size); i++)
                yield return new List<T>(source.Skip(size * i).Take(size));
        }
    }
  
    public class TuplePairComparer : IEqualityComparer<Tuple<Tuple<int,int>,Tuple<int,int>>>
    {
        public bool Equals(Tuple<Tuple<int, int>, Tuple<int, int>> x, Tuple<Tuple<int, int>, Tuple<int, int>> y)
        {
            Console.WriteLine("Comparing ({0},{1}), ({2},{3})", x.Item1, x.Item2, y.Item1, y.Item2);
            if ((x.Item1 == y.Item1) && (x.Item2 == y.Item2))
            {
                Console.WriteLine("Result: Equal");
                return true;
            }
            if ((x.Item1 == y.Item2) && (x.Item2 == y.Item1))
            {
                Console.WriteLine("Result: Equal");
                return true;
            }
            Console.WriteLine("Result: Unequal");
            return false;
        }

        public int GetHashCode(Tuple<Tuple<int, int>, Tuple<int, int>> obj)
        {
            //if two objects are tuple equal; even if the positions are different, the sum is always constant
            int hash = obj.Item1.GetHashCode() + obj.Item2.GetHashCode();
            Console.WriteLine("Hash function of ({0},{1}) is {2}", obj.Item1, obj.Item2, hash);
            return hash;
        }
    }
}
