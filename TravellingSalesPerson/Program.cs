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
        private static List<Tuple<double, double>> travelCoOrdinates;// = new List<Tuple<double, double>>();
        //The storage requirement for this is nC(1...n) where n = number of cities. For 5 = 31. For 25 = 3,35,54,430
        //Also, algorithmic complexity = n. 2^n (25 * 33554432 = 838860800) where n = size of input graph. Hence optimization has to be thought of before writing first line of code.
        //for n = 13 this is 13 * 8192 = 106496 hence 315 times lesser than when n = 25
        private static HashSet<int[]> sets;
        private static Dictionary<Tuple<string, string>, double> subsetVsNodeCost;
        private static Dictionary<Tuple<string, string>, double> nonEuclideanDistanceSpecification;
        private static bool isDistanceNonEuclidean = false;
        static void Main(string[] args)
        {
           /* LoadAssignmentData1();
            double a =  ComputeTravellingSalesManTourCost(0);
            Console.WriteLine  ("Tour cost Part 1="+ a);
            Tuple<double, double> t1 = travelCoOrdinates[travelCoOrdinates.Count - 1];
            LoadAssignmentData2();
            double b = ComputeTravellingSalesManTourCost(0);
            Console.WriteLine("Tour cost Part 1=" + b);
            Tuple<double, double> t2 = travelCoOrdinates[0];

            double c = GetDistance(t1, t2);
            Console.WriteLine(string.Format("Euclidean distance between: [{0},{1}] & [{2},{3}]={4}", t1.Item1, t1.Item2, t2.Item1, t2.Item2, c));

            Console.WriteLine ("Tour cost = " + (a+b+c));
            
            //LoadTestDataNonEuclideanDistances();
            //LoadTestData();
            //LoadTestData2();*/
            LoadAssignmentData();
            Console.WriteLine("Tour cost =" + ComputeTravellingSalesManTourCost());
            Console.ReadKey();
        }

        private static double ComputeTravellingSalesManTourCost (int from=0)
        {
           subsetVsNodeCost = new Dictionary<Tuple<string,string>, double>();
            foreach (var s in sets.Where(s=> s.Count() == 1))
            {
                subsetVsNodeCost.Add(new Tuple<string, string>(GetCommaSeparatedString(s), s[0].ToString()), s[0]==from ? 0 : INFINITY);
            }
            for (int m = 2; m <= travelCoOrdinates.Count; m++)
            {
                Console.WriteLine("m=" + m);
                foreach (var s in sets.Where(s => s.Contains(from) && s.Count() == m))
                {
                    foreach (var destination in s)
                    {
                        if (destination != from)
                        {
                            var key = new Tuple<string, string> (GetCommaSeparatedString(s), destination.ToString());
                            subsetVsNodeCost.Add(key, INFINITY);
                            
                            double minimum = INFINITY;
                            foreach (var k in s)
                            {
                                if (k!= destination)
                                {
                                    var reducedSubSetKey = new Tuple<string, string>(GetCommaSeparatedString(s.Where(p => p != destination).ToArray()), k.ToString());
                                    if (!subsetVsNodeCost.ContainsKey(reducedSubSetKey))
                                    {
                                        subsetVsNodeCost.Add(reducedSubSetKey, INFINITY);
                                    }
                                    var cost = subsetVsNodeCost[reducedSubSetKey] + GetDistance(travelCoOrdinates[k], travelCoOrdinates[destination]);
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
            //the last vertex j you visit before you return home to the vertex 'from'
            var subset = sets.First(s => s.Count() == travelCoOrdinates.Count());
            double tourCost = INFINITY;
            for (int nodeIndex = 0; nodeIndex < travelCoOrdinates.Count ; nodeIndex++ )
            {
                if (nodeIndex != from)
                {
                    var reducedCostKey = new Tuple<string, string>(GetCommaSeparatedString(subset), nodeIndex.ToString());
                    //reducedCostKey = GetCommaSeparatedString(subset.Where (p=> p!= nodeIndex).ToArray());
                    var cost = subsetVsNodeCost[reducedCostKey] + GetDistance(travelCoOrdinates[nodeIndex], travelCoOrdinates[from]);
                    if (tourCost > cost)
                    {
                        tourCost = cost;
                    }
                }
            }
            return tourCost;
        }
        #region TEST CASES
        /// <summary>
        /// Optimal = 8.8
        /// </summary>
        private static void LoadTestData ()
        {
            travelCoOrdinates = new List<Tuple<double, double>>();
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

        private static void LoadAssignmentData1 ()
        {
            travelCoOrdinates = new List<Tuple<double, double>>();
            travelCoOrdinates.Add(new Tuple<double, double>(20833.3333,7100.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(20900.0000,7066.6667));
            travelCoOrdinates.Add(new Tuple<double, double>(21300.0000,3016.6667));
            travelCoOrdinates.Add(new Tuple<double, double>(21600.0000,4150.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(21600.0000,4966.6667));
            travelCoOrdinates.Add(new Tuple<double, double>(21600.0000,6500.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(22183.3333,3133.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(22583.3333,4300.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(22683.3333,2716.6667));
            travelCoOrdinates.Add(new Tuple<double, double>(23616.6667,5866.6667));
            travelCoOrdinates.Add(new Tuple<double, double>(23700.0000,5933.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(23883.3333,4533.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(24166.6667,3250.0000));

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

        private static void LoadAssignmentData2()
        {
            travelCoOrdinates = new List<Tuple<double, double>>();
            travelCoOrdinates.Add(new Tuple<double, double>(25149.1667, 2365.8333));
            travelCoOrdinates.Add(new Tuple<double, double>(26133.3333, 4500.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(26150.0000, 0550.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(26283.3333, 2766.6667));
            travelCoOrdinates.Add(new Tuple<double, double>(26433.3333, 3433.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(26550.0000, 3850.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(26733.3333, 1683.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(27026.1111, 3051.9444));
            travelCoOrdinates.Add(new Tuple<double, double>(27096.1111, 3415.8333));
            travelCoOrdinates.Add(new Tuple<double, double>(27153.6111, 3203.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(27166.6667, 9833.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(27233.3333, 0450.0000));

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
        }

        private static void LoadAssignmentData()
        {
            travelCoOrdinates = new List<Tuple<double, double>>();
            //CLUSTER 1
            travelCoOrdinates.Add(new Tuple<double, double>(24166.6667, 3250.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(20833.3333, 7100.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(20900.0000, 7066.6667));
            travelCoOrdinates.Add(new Tuple<double, double>(21300.0000, 3016.6667));
            travelCoOrdinates.Add(new Tuple<double, double>(21600.0000, 4150.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(21600.0000, 4966.6667));
            travelCoOrdinates.Add(new Tuple<double, double>(21600.0000, 6500.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(22183.3333, 3133.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(22583.3333, 4300.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(22683.3333, 2716.6667));
            travelCoOrdinates.Add(new Tuple<double, double>(23616.6667, 5866.6667));
            travelCoOrdinates.Add(new Tuple<double, double>(23700.0000, 5933.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(23883.3333, 4533.3333));
            //CLUSTER 2
            travelCoOrdinates.Add(new Tuple<double, double>(25149.1667, 2365.8333));
            travelCoOrdinates.Add(new Tuple<double, double>(26133.3333, 4500.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(26150.0000, 0550.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(26283.3333, 2766.6667));
            travelCoOrdinates.Add(new Tuple<double, double>(26433.3333, 3433.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(26550.0000, 3850.0000));
            travelCoOrdinates.Add(new Tuple<double, double>(26733.3333, 1683.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(27026.1111, 3051.9444));
            travelCoOrdinates.Add(new Tuple<double, double>(27096.1111, 3415.8333));
            travelCoOrdinates.Add(new Tuple<double, double>(27153.6111, 3203.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(27166.6667, 9833.3333));
            travelCoOrdinates.Add(new Tuple<double, double>(27233.3333, 0450.0000));


            sets = new HashSet<int[]>();
            //manually for now
            int[] largestSet = new int[travelCoOrdinates.Count];
            for (int index = 0; index < travelCoOrdinates.Count; index++)
            {
                sets.Add(new int[] { index });
                largestSet[index] = index;
            }

            for (int index = 1; index < 13; index++) //apart from final combination, the above is a cluster of 12 + 13 nodes
            {
                var crossJoin = sets.SelectMany(t1 => sets.Select(t2 => { if (t1.Count() == index && t2.Count() == 1 && t1.Intersect(t2).Count() == 0 && t1[0] > t2[0]) 
                    return t2.Union(t1); return new int[0]; })).Where(entry => entry.Count() == index + 1).ToArray();
                foreach (var combination in crossJoin)
                {
                    sets.Add(combination.ToArray());
                }
            }
            sets.Add(largestSet);
        }

        private static void LoadTestDataNonEuclideanDistances ()
        {
            isDistanceNonEuclidean = true;
            travelCoOrdinates = new List<Tuple<double, double>>();
            travelCoOrdinates.Add(new Tuple<double, double>(0.0, 0.0));
            travelCoOrdinates.Add(new Tuple<double, double>(1.0, 1.0));
            travelCoOrdinates.Add(new Tuple<double, double>(2.0, 2.0));
            travelCoOrdinates.Add(new Tuple<double, double>(3.0, 3.0));

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

            nonEuclideanDistanceSpecification = new Dictionary<Tuple<string, string>, double>();
            nonEuclideanDistanceSpecification.Add (new Tuple<string,string>("0", "1"), 1);
            nonEuclideanDistanceSpecification.Add (new Tuple<string,string>("0", "2"), 4);
            nonEuclideanDistanceSpecification.Add (new Tuple<string,string>("0", "3"), 2);
            nonEuclideanDistanceSpecification.Add(new Tuple<string, string>("1", "2"), 6);
            nonEuclideanDistanceSpecification.Add(new Tuple<string, string>("1", "3"), 3);
            nonEuclideanDistanceSpecification.Add(new Tuple<string, string>("2", "3"), 5);
        }
        /// <summary>
        /// Optimal: 7.63
        /// </summary>
        private static void LoadTestData2()
        {
            travelCoOrdinates = new List<Tuple<double, double>>();
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
        #endregion

        #region HELPER METHODS
        private static double GetDistance(Tuple <double, double> t1, Tuple <double, double> t2)
        {
            if (isDistanceNonEuclidean)
            {
                return nonEuclideanDistanceSpecification.First(d => (d.Key.Item1 == (Convert.ToInt32(t1.Item1).ToString()) && d.Key.Item2 == Convert.ToInt32(t2.Item1).ToString()) || (d.Key.Item1 == (Convert.ToInt32(t2.Item1).ToString()) && d.Key.Item2 == Convert.ToInt32(t1.Item1).ToString())).Value;
            }
            return GetEuclideanDistance(t1, t2);
        }
        private static double GetEuclideanDistance(Tuple <double, double> t1, Tuple <double, double> t2)
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
        #endregion
    }
}


