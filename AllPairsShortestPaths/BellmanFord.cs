using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPairsShortestPaths
{
    partial class Program
    {
        private static List<int> A0;
        private static List<int> A1;
        private static bool DetectNegativeCycle ()
        {
            //Initialize vectors
            A0 = new List<int>(n);
            A1 = new List<int>(n);
            A0 = Enumerable.Repeat<int>(Int32.MaxValue, n).ToList<int>();
            A1 = Enumerable.Repeat<int>(Int32.MaxValue, n).ToList<int>();
            A0[0] = 0; //source vertex
            A1[0] = 0;
            int successiveNoChangeCount = 0;
            //Run cycle #edges + 1 times to detect negative cycle
            //Assuming 1 is the source vertex
            for (int i = 1; i <= m + 1; i++) //constrain the maximum number of vertices (hence edges) permitted in path. 
                //SCOPE: for optimization here. If for two consecutive iterations no change in vector values, can break out with inference as no negative cost cycles
            {
                for (int v= 1; v <= n; v++)
                {
                    Tuple<int, int, int> directDistanceEntry = G.Find(e => e.Item1 == 1 && e.Item2 == v);
                    int directDistance = (null == directDistanceEntry) ? Int32.MaxValue
                        : directDistanceEntry.Item3;

                    int minimum_distance = Int32.MaxValue;
                    for (int u = 1; u <= n; u++)
                    {
                         Tuple<int, int, int> indirectDistanceTuple = G.Find(e => e.Item1 == u && e.Item2 == v);
                            int indirectDistance = A0[u - 1] == Int32.MaxValue || null == indirectDistanceTuple ? Int32.MaxValue
                                : A0[u - 1] + indirectDistanceTuple.Item3;
                            minimum_distance = Math.Min(minimum_distance, indirectDistance);
                    }
                    A1[v - 1] = Math.Min(minimum_distance, directDistance);  
                }
                if (!IsDecrementingTrend())
                {
                    successiveNoChangeCount++;
                    if (successiveNoChangeCount == 3)
                    {
                        Console.WriteLine("No change for last 3 iterations");
                        return false;
                    }
                }
                else
                {
                    successiveNoChangeCount = 0;
                }
                if (i != m + 1)
                {
                    A0.Clear();
                    A0 = A1.ToList<int>();
                }
            }

            return IsDecrementingTrend(); ;
        }
        private static bool IsDecrementingTrend ()
        {
            for (int i = 1; i <= n; i++)
            {
                if (A1[i - 1] < A0[i - 1])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
