using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesPerson
{
    class Program
    {
        private const double INFINITY = double.MaxValue;
        private static List<Tuple<double, double>> travelCoOrdinates = new List<Tuple<double, double>>();
        private static HashSet<int[]> sets;
        private static Dictionary<string, double> subsetVsNodeCost;
        static void Main(string[] args)
        {
            LoadTestData2();
            ComputeTravellingSalesManTourCost(0);
        }

        private static double ComputeTravellingSalesManTourCost (int from=0)
        {
           subsetVsNodeCost = new Dictionary<string, double>();
            foreach (var s in sets.Where(s=> s.Count() == 1))
            {
                //subsetVsNodeCost.Add(GetCommaSeparatedString(s), Euclidean(travelCoOrdinates[s[0]], travelCoOrdinates[from]));
                subsetVsNodeCost.Add(GetCommaSeparatedString(s), s[0]==from ? 0 : INFINITY);
            }
            /*
            * for m = 2,3,4 ..n
            *     for each set S in {1,2,...n} of size m that contains 1 
            *          for each j in S, j != 1
            *              A[S1j] = min {A[S- {j}, k] + Ckj} s.t. k belongs to S and k != j      
            * return min {A[{1,2,3,..n}, j] + Cj1} for j = 2
            */
            for (int m = 2; m <= travelCoOrdinates.Count; m++)
            {
                foreach (var s in sets.Where(s => s.Contains(from) && s.Count() == m))
                {
                    foreach (var destination in s)
                    {
                        if (destination != from)
                        {
                            string key = GetCommaSeparatedString(s);
                            if (!subsetVsNodeCost.ContainsKey(key))
                            {
                                subsetVsNodeCost.Add(key, INFINITY);
                            }
                            double minimum = INFINITY;
                            foreach (var k in s)
                            {
                                if (k!= destination)
                                {
                                    var reducedSubSet = s.Where(p => p != destination).ToArray();
                                    var cost = subsetVsNodeCost[GetCommaSeparatedString(reducedSubSet)] + Euclidean(travelCoOrdinates[k], travelCoOrdinates[destination]);
                                    if (cost < minimum)
                                    {
                                        subsetVsNodeCost[key] = minimum = cost;
                                    }
                                }
                            }
                        }
                    }
                }
            }
                
            /*return min j=2 to n
             * A[{1...n},j] + Cj1
             */
            var subset = sets.First(s => s.Count() == travelCoOrdinates.Count());
            double tourCost = INFINITY;
            for (int nodeIndex = 0; nodeIndex < travelCoOrdinates.Count ; nodeIndex++ )
            {
                if (nodeIndex != from)
                {
                    var reducedCostKey = GetCommaSeparatedString(subset);
                    var cost = subsetVsNodeCost[reducedCostKey] + Euclidean(travelCoOrdinates[nodeIndex], travelCoOrdinates[from]);
                    if (tourCost > cost)
                    {
                        tourCost = cost;
                    }
                }
            }
            return tourCost;
        }
        /// <summary>
        /// Optimal = 8.8
        /// </summary>
        private static void LoadTestData ()
        {
            travelCoOrdinates.Add(new Tuple<double, double>(2.0, 2.0));
            travelCoOrdinates.Add(new Tuple<double, double>(0.0, 2.0));
            travelCoOrdinates.Add(new Tuple<double, double>(0.0, 0.0));
            travelCoOrdinates.Add(new Tuple<double, double>(2.0, 0.0));
            travelCoOrdinates.Add(new Tuple<double, double>(1.0, 1.0));
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
            //Remove subsets with just one vertex
            //sets.RemoveWhere(s => s.Count() <= 1);
        }

        /// <summary>
        /// Optimal: 7.14
        /// </summary>
        private static void LoadTestData2()
        {
            travelCoOrdinates.Add(new Tuple<double, double>(1.0, 1.0));
            travelCoOrdinates.Add(new Tuple<double, double>(0.5, 0.85));
            travelCoOrdinates.Add(new Tuple<double, double>(0.85, 0.5));
            travelCoOrdinates.Add(new Tuple<double, double>(0.0, 1.0));
            travelCoOrdinates.Add(new Tuple<double, double>(-0.5, 0.85));
            travelCoOrdinates.Add(new Tuple<double, double>(-0.85, 0.5));
            travelCoOrdinates.Add(new Tuple<double, double>(-1.0, 0.0));
            travelCoOrdinates.Add(new Tuple<double, double>(-0.85, -0.5));
            travelCoOrdinates.Add(new Tuple<double, double>(-0.5, -0.85));
            travelCoOrdinates.Add(new Tuple<double, double>(0.0, -1.0));
            travelCoOrdinates.Add(new Tuple<double, double>(0.5, -0.85));
            travelCoOrdinates.Add(new Tuple<double, double>(0.85, -0.5));
            travelCoOrdinates.Add(new Tuple<double, double>(0.0, 0.0));

            sets = new HashSet<int[]>();
            //manually for now
            for (int index = 0; index < travelCoOrdinates.Count; index++)
            {
                sets.Add(new int[] { index });
            }

            for (int index = 1; index < travelCoOrdinates.Count; index++)
            {
                var crossJoin = sets.SelectMany(t1 => sets.Select(t2 => { if (t1.Count() == index && t2.Count() == 1 && t1.Intersect(t2).Count() == 0 && t1[0] > t2[0]) return t2.Union(t1); return new int[0]; })).Where(entry => entry.Count() == index + 1).ToArray();
                foreach (var combination in crossJoin)
                {
                    sets.Add(combination.ToArray());
                }
            }
            //Remove subsets with just one vertex
            //sets.RemoveWhere(s => s.Count() <= 1);
        }
        private static double Euclidean(Tuple <double, double> t1, Tuple <double, double> t2)
        {
            return Math.Sqrt(Math.Pow(t1.Item1 - t2.Item1, 2) + Math.Pow(t1.Item2 - t2.Item2, 2));
        }
        private static string GetCommaSeparatedString (Int32[] list)
        {
            string toBeReturned = string.Empty;
            foreach (Int32 num in list)
            {
                toBeReturned += (num.ToString() + ",");
            }
            toBeReturned = toBeReturned.Remove(toBeReturned.Length - 1);
            return toBeReturned;
        }
    }
}


