using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPairsShortestPaths
{
    class Program
    {
        private static string SOURCE = "0";

        private static Dictionary<string, int> computedDistances;
        private static Dictionary<string, List<Tuple<string, int>>> graph;
        private static int INFINITY = int.MaxValue;
        static void Main(string[] args)
        {
            LoadGraphFromTestData2();
            ComputeDijkstraShortestPath();

            Console.WriteLine("Vertex\t\tDistance From Source");
            for (var gEnum = computedDistances.GetEnumerator(); gEnum.MoveNext(); )
            {
                Console.WriteLine(string.Format("{0}\t\t\t{1}", gEnum.Current.Key, gEnum.Current.Value));
            }
            Console.ReadKey();
        }

        /*EXPECTED OUTPUT:
       * Vertex   Distance from Source
          0                0
          1                4
          2                12
          3                19
          4                21
          5                11
          6                9
          7                8
          8                14
       * */
        private static void LoadGraphFromTestData2()
        {
            graph = new Dictionary<string, List<Tuple<string, int>>>();

            //node 0
            List<Tuple<string, int>> list0 = new List<Tuple<string, int>>();
            list0.Add(new Tuple<String, int>("1", 4));
            list0.Add(new Tuple<String, int>("7", 8));
            graph.Add("0", list0);
            //node 1
            List<Tuple<String, int>> list1 = new List<Tuple<String, int>>();
            list1.Add(new Tuple<String, int>("2", 8));
            list1.Add(new Tuple<String, int>("7", 11));
            list1.Add(new Tuple<String, int>("0", 4));

            graph.Add("1", list1);

            //node 2
            List<Tuple<String, int>> list2 = new List<Tuple<String, int>>();
            list2.Add(new Tuple<String, int>("3", 7));
            list2.Add(new Tuple<String, int>("5", 4));
            list2.Add(new Tuple<String, int>("8", 2));
            list2.Add(new Tuple<String, int>("1", 8));

            graph.Add("2", list2);

            //node 3
            List<Tuple<String, int>> list3 = new List<Tuple<String, int>>();
            list3.Add(new Tuple<String, int>("4", 9));
            list3.Add(new Tuple<String, int>("5", 14));
            list3.Add(new Tuple<String, int>("2", 7));

            graph.Add("3", list3);

            //node 4
            List<Tuple<String, int>> list4 = new List<Tuple<String, int>>();
            list4.Add(new Tuple<String, int>("5", 10));
            list4.Add(new Tuple<String, int>("3", 9));

            graph.Add("4", list4);

            //node 5
            List<Tuple<String, int>> list5 = new List<Tuple<String, int>>();
            list5.Add(new Tuple<String, int>("6", 2));
            list5.Add(new Tuple<String, int>("2", 4));
            list5.Add(new Tuple<String, int>("3", 14));
            list5.Add(new Tuple<String, int>("4", 10));


            graph.Add("5", list5);

            //node 6
            List<Tuple<String, int>> list6 = new List<Tuple<String, int>>();
            list6.Add(new Tuple<String, int>("7", 1));
            list6.Add(new Tuple<String, int>("8", 6));
            list6.Add(new Tuple<String, int>("5", 2));

            graph.Add("6", list6);

            //node 7
            List<Tuple<String, int>> list7 = new List<Tuple<String, int>>();
            list7.Add(new Tuple<String, int>("8", 7));
            list7.Add(new Tuple<String, int>("0", 8));
            list7.Add(new Tuple<String, int>("1", 11));
            list7.Add(new Tuple<String, int>("6", 1));

            graph.Add("7", list7);

            //node 8
            List<Tuple<String, int>> list8 = new List<Tuple<String, int>>();
            list8.Add(new Tuple<String, int>("2", 2));
            list8.Add(new Tuple<String, int>("7", 6));
            list8.Add(new Tuple<String, int>("6", 6));

            graph.Add("8", list8);
        }

        private static void ComputeDijkstraShortestPath()
        {
            String nextVertex = SOURCE;
            HashSet<KeyValuePair<string, Tuple<bool, bool>>> computationStatus = new HashSet<KeyValuePair<string, Tuple<bool, bool>>>();
            //Hashmap<String, <isProcessed, isReachable>>

            //construct initial set and compute distances
            computedDistances = new Dictionary<string, int>();

            for (var k = graph.GetEnumerator(); k.MoveNext(); )
            {
                String key = k.Current.Key;
                computedDistances.Add(k.Current.Key, INFINITY);
                computationStatus.Add(new KeyValuePair<string, Tuple<bool, bool>>(k.Current.Key, new Tuple<bool,bool>(false, false)));
            }
            computedDistances[SOURCE] = 0;
            while (graph.Count != 0)
            {
                string temp = nextVertex;
                int distanceOfCurrentVertex = computedDistances[nextVertex];
                int minimum = INFINITY;
                //update the graph with new calculations
                if (graph.ContainsKey (nextVertex))
                {
                    computationStatus.RemoveWhere(s => s.Key.Equals(nextVertex));
                    computationStatus.Add(new KeyValuePair<string, Tuple<bool, bool>>(nextVertex, new Tuple<bool,bool>(true, true)));

                    //computationStatus.put(nextVertex, new Tuple<Boolean, Boolean>(true, true));
                    for (var distanceFromSource = graph[nextVertex].GetEnumerator(); distanceFromSource.MoveNext(); )
                    {
                        Tuple<String, int> t = distanceFromSource.Current;
//                        computationStatus.put(t.x, new Tuple<Boolean, Boolean>(computationStatus.get(t.x).x, true));
                        bool previouslyComputedValue = computationStatus.First(st=> st.Key == t.Item1).Value.Item1;
                        computationStatus.RemoveWhere(s => s.Key.Equals(t.Item1));
                        computationStatus.Add(new KeyValuePair<string, Tuple<bool, bool>>
                            (t.Item1, new Tuple<bool, bool>(previouslyComputedValue, true)));
                        
                        if (t.Item2 + distanceOfCurrentVertex < computedDistances[t.Item1])
                        {
                            computedDistances[t.Item1] = t.Item2 + distanceOfCurrentVertex;
                        }
                        if (computedDistances[t.Item1] < minimum)
                        {
                            nextVertex = t.Item1;
                            minimum = computedDistances[nextVertex];
                        }
                    }
                }
                else
                {
                    var it = computationStatus.GetEnumerator();
                    minimum = INFINITY;
                    while (it.MoveNext())
                    {
                        var entry = it.Current;
                        if (!entry.Value.Item1 && entry.Value.Item2 && computedDistances[entry.Key] < minimum)
                        {
                            nextVertex = entry.Key;
                            minimum = computedDistances[entry.Key] ;
                        }
                    }
                }

                /*PRINT TEST OUTPUT
                 * System.out.println("Node:"+ temp);
                Iterator<Entry<String, int>> it = computedDistances.entrySet().iterator();
                while (it.hasNext())
                {
                    Entry<String, int> entry = it.next();
                    System.out.println("Node:" + entry.getKey() + "\tDistance:" + computedDistances.get(entry.getKey()) + "\n");
                }
                System.out.println("----------------------------------------------");*/

                //remove the vertex from source
                graph.Remove(temp);
            }
        }
      
        private static void ComputeBellmanForShortestPaths()
        {
            int[][] matrix = new int [2][];
            for (int temp = 0; temp < 2; temp++)
            {
                matrix[temp] = new int[graph.Count];
                Array.ForEach(matrix[temp], (a => a = 0)); //initialized all values to zero
            }
            bool valueDecreasedInFinalIteration = false;
            for (int i = 0; i < graph.Count; i++)
            {
                Array.Copy(matrix[1], matrix[0], graph.Count);
                Array.ForEach(matrix[1], (a => a = int.MaxValue)); //initialized all values to zero

                for (int v = 0; v < graph.Count; v++)
                {
                    int value1 = (i != 0) ? matrix[0][v] : int.MaxValue;

                    int value2 = int.MaxValue;
                    foreach (int t in matrix[0])
                    {
                        int temp = t + computedDistances[t.ToString()];
                        value2 = temp < value2 ? temp : value2;
                    }
                    matrix[1][v] = Math.Min(value1, value2);
                }
            }
            if (valueDecreasedInFinalIteration)
            {
                Console.WriteLine("Graph has negative edges");
            }
        }
    }
}
