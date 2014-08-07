using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesPerson
{
    class Program
    {
        private const int INFINITY = int.MaxValue;
        private static List<Tuple<double, double>> travelCoOrdinates = new List<Tuple<double, double>>();
        private static HashSet<int[]> sets;
        static void Main(string[] args)
        {
            LoadTestData();
        }

        private static int ComputeTravellingSalesManTourCost (int from=0)
        {
            /*
             * for m = 2,3,4 ..n
             *     for each set S in {1,2,...n} of size me that contains 1 
             *          for each j in S, j != 1
             *              A[Sij] = min {A[S- {j}, k] + Ckj} s.t. k belongs to S and k != j      
             * return min {A[{1,2,3,..n}, j] + Cj1} for j = 2
             */
        
            for (int m = 1; m < travelCoOrdinates.Count; m++)
            {
                foreach (var s in sets.Where(s=> s.Contains(from)))
                {
                    foreach (var j in s)
                    {
                        if (j!= from)
                        {

                        }
                    }
                }
            }
                return INFINITY;
        }
        private static void LoadTestData ()
        {
            travelCoOrdinates.Add(new Tuple<double, double>(1.0, 1.0));
            travelCoOrdinates.Add(new Tuple<double, double>(1.0, 1.0));
            travelCoOrdinates.Add(new Tuple<double, double>(1.0, 1.0));
            travelCoOrdinates.Add(new Tuple<double, double>(1.0, 1.0));
            travelCoOrdinates.Add(new Tuple<double, double>(0.0, 0.0));
            sets = new HashSet<int[]>();
            //manually for now
            for (int index = 0; index < travelCoOrdinates.Count; index ++)
            {
                sets.Add(new int[] { index });
            }
          
            for (int index = 1; index < travelCoOrdinates.Count; index++)
            {
                var crossJoin = sets.SelectMany(t1 => sets.Select(t2 => { if (t1.Count() == index && t2.Count() == 1 && t1.Intersect(t2).Count() == 0 && t1[0] > t2[0]) return t2.Union(t1); return new int[0]; })).Where(entry => entry.Count() == index+1).ToArray();
                foreach (var combination in crossJoin)
                {
                    sets.Add(combination.ToArray());
                }
            }
        }
    }
}
