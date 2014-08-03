using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPairsShortestPaths
{
    class Program
    {
        private static Dictionary<string, int> computedDistances;
        private static Dictionary<string, List<Tuple<string, int>>> graph;
        private static Dictionary<string, List<Tuple<string, int>>> incidentEdgesGraph;
        private static int INFINITY = int.MaxValue;
        static void Main(string[] args)
        {
            //LoadDirectedTestGraph();
            //ComputeDijkstraShortestPath();     
            //LoadDirectedTestGraphWithNegativeCycle();
           // LoadDirectedTestGraphWithNegativeCycle2();
           //LoadDirectedTestGraphWithCycle();
            LoadDirectedTestGraphForJohnsons();
            var result = ComputeShortestPathDistance();
            if (!result.Item1)
            {
                Console.WriteLine("Vertex\t\tDistance From Source");
                for (var gEnum = computedDistances.GetEnumerator(); gEnum.MoveNext(); )
                {
                    Console.WriteLine(string.Format("{0}\t\t\t{1}", gEnum.Current.Key, gEnum.Current.Value));
                }
            }         
            else
            {
                Console.WriteLine("Graph has negative cycle(s)");
            }
            Console.ReadKey();
        }

        #region ALGORITHMS
        private static Tuple<bool,int> ComputeShortestPathDistance ()
        {
            for (var item = incidentEdgesGraph.Values.GetEnumerator(); item.MoveNext(); )
            {
                item.Current.Add(new Tuple<string, int>("0", 0));
            }
            incidentEdgesGraph.Add("0", new List<Tuple<string, int>>());
            List<Tuple<string, int>> temp = graph.Keys.Select(k=> new Tuple<string, int>(k, 0)).ToList();
            graph.Add("0", new List<Tuple<string, int>>(temp));
            bool containsNegativeCycles = ComputeBellmanForShortestPaths();
            int shortestDistance = int.MaxValue;

            if (containsNegativeCycles)
            {
                return new Tuple<bool, int>(containsNegativeCycles, 0);
            }
            else
            {
                //remove the extra nodes and garbage collection
                incidentEdgesGraph = null;
                graph.Remove("0");
                
                //recompute edge weights
                List<string> keySetClone = graph.Keys.ToList();
                for (var g = keySetClone.GetEnumerator(); g.MoveNext(); )
                {
                    graph[g.Current] = graph[g.Current].Select(n => { n = new Tuple<string, int>(n.Item1, n.Item2 + computedDistances[g.Current] - computedDistances[n.Item1]); return n; }).ToList();
                }
                for (var g = keySetClone.GetEnumerator(); g.MoveNext();)
                {
                    ComputeDijkstraShortestPath(g.Current);
                    shortestDistance = shortestDistance > computedDistances.Min(d => d.Value)? computedDistances.Min(d => d.Value) : shortestDistance;
                    Console.WriteLine("New shortest distance=" + shortestDistance + " for node =" + g.Current);
                }
            }
            return new Tuple<bool, int>(false, shortestDistance);
        }
        private static void ComputeDijkstraShortestPath(string source)
        {
            String nextVertex = source;
            Dictionary<string, Tuple<bool, bool>> computationStatus = new Dictionary<string,Tuple<bool,bool>>();
            var clone = new Dictionary<string, List<Tuple<string, int>>>(graph) ;
            //construct initial set and compute distances
            computedDistances = new Dictionary<string, int>();

            for (var k = clone.GetEnumerator(); k.MoveNext(); )
            {
                string key = k.Current.Key;
                computedDistances.Add(k.Current.Key, INFINITY);
                computationStatus.Add(k.Current.Key, new Tuple<bool, bool>(false, false));
            }
            computedDistances[nextVertex] = 0;
            int nodeCount = clone.Count;
            while (nodeCount != 0)
            {
                --nodeCount;
                string temp = nextVertex;
                int distanceOfCurrentVertex = computedDistances[nextVertex];
                int minimum = INFINITY;
                //update the clone with new calculations
                if (clone.ContainsKey(nextVertex))
                {
                    computationStatus [nextVertex] = new Tuple<bool, bool>(true, true);
                    for (var distanceFromSource = clone[nextVertex].GetEnumerator(); distanceFromSource.MoveNext(); )
                    {
                        Tuple<String, int> t = distanceFromSource.Current;
                        bool previouslyComputedValue = computationStatus[t.Item1].Item1;
                        computationStatus[t.Item1] = new Tuple<bool, bool>(previouslyComputedValue, true);
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
                            minimum = computedDistances[entry.Key];
                        }
                    }
                }

                //remove the vertex from source
                clone.Remove(temp);
            }
        }
        private static bool ComputeBellmanForShortestPaths()
        {
            int[][] matrix = new int [2][];
            matrix[0] = new int[graph.Count];
            matrix[1] = new int[graph.Count];
            for (int index = 0; index < graph.Count; index ++ )
            {
                matrix[1][index] = int.MaxValue;
            }
            matrix[1][0] = 0;//source vertex
            
            bool valueDecreasedInFinalIteration = true;
            int i;
            for (i = 0; i < graph.Count; i++)
            {
                valueDecreasedInFinalIteration = false;
                Array.Copy(matrix[1], matrix[0], graph.Count);
                for (int index = 0; index < graph.Count; index++)
                {
                    matrix[1][index] = int.MaxValue;
                }
                for (int v = 0; v < graph.Count;  v++)
                {
                    int value1 =  matrix[0][v];

                    int value2 = int.MaxValue;
                    foreach ( var edge in incidentEdgesGraph[v.ToString()])
                    {
                        int temp = matrix[0][int.Parse(edge.Item1)] == int.MaxValue ? int.MaxValue //to avoid overflow
                            :matrix[0][int.Parse(edge.Item1)] + edge.Item2;
                        value2 = temp < value2 ? temp : value2;
                    }
                    matrix[1][v] = Math.Min(value1, value2);
                    valueDecreasedInFinalIteration = valueDecreasedInFinalIteration  || (matrix[0][v] > matrix[1][v]);
                }
            }
            computedDistances = new Dictionary<string,int>();
            for (int k=0; k < graph.Count(); k++)
            {
                computedDistances.Add(k.ToString(), matrix[1][k]);
            }
            return valueDecreasedInFinalIteration;
             
        }
        #endregion


        #region UNIT TESTING METHODS
        private static void LoadDirectedTestGraph()
        {
            graph = new Dictionary<string, List<Tuple<string, int>>>();

            //node 0
            List<Tuple<string, int>> list0 = new List<Tuple<string, int>>();
            list0.Add(new Tuple<String, int>("1", 2));
            list0.Add(new Tuple<String, int>("2", 4));
            graph.Add("0", list0);

            //node 1
            List<Tuple<string, int>> list1 = new List<Tuple<string, int>>();
            list1.Add(new Tuple<String, int>("3", 2));
            list1.Add(new Tuple<String, int>("2", 1));
            graph.Add("1", list1);

            //node 2
            List<Tuple<string, int>> list2 = new List<Tuple<string, int>>();
            list2.Add(new Tuple<String, int>("4", 4));
            graph.Add("2", list2);

            //node 3
            List<Tuple<string, int>> list3 = new List<Tuple<string, int>>();
            list3.Add(new Tuple<String, int>("4", 2));
            graph.Add("3", list3);

            //node 4
            graph.Add("4", new List<Tuple<string, int>>());

            incidentEdgesGraph = new Dictionary<string, List<Tuple<string, int>>>();
            incidentEdgesGraph.Add("0", new List<Tuple<string, int>>());
            List<Tuple<string, int>> _list1 = new List<Tuple<string, int>>();
            _list1.Add(new Tuple<string, int>("0", 2));
            incidentEdgesGraph.Add("1", _list1);

            List<Tuple<string, int>> _list2 = new List<Tuple<string, int>>();
            _list2.Add(new Tuple<string, int>("1", 1));
            _list2.Add(new Tuple<string, int>("0", 4));
            incidentEdgesGraph.Add("2", _list2);

            List<Tuple<string, int>> _list3 = new List<Tuple<string, int>>();
            _list3.Add(new Tuple<string, int>("1", 2));
            incidentEdgesGraph.Add("3", _list3);

            List<Tuple<string, int>> _list4 = new List<Tuple<string, int>>();
            _list4.Add(new Tuple<string, int>("2", 4));
            _list4.Add(new Tuple<string, int>("3", 2));
            incidentEdgesGraph.Add("4", _list4);



        }
        private static void LoadDirectedTestGraphWithNegativeCycle()
        {
            graph = new Dictionary<string, List<Tuple<string, int>>>();

            //node 0
            List<Tuple<string, int>> list0 = new List<Tuple<string, int>>();
            list0.Add(new Tuple<String, int>("1", 4));
            graph.Add("0", list0);

            //node 1
            List<Tuple<string, int>> list1 = new List<Tuple<string, int>>();
            list1.Add(new Tuple<String, int>("3", -1));
            graph.Add("1", list1);

            //node 2
            List<Tuple<string, int>> list2 = new List<Tuple<string, int>>();
            list2.Add(new Tuple<String, int>("1", -3));
            graph.Add("2", list2);

            //node 3
            List<Tuple<string, int>> list3 = new List<Tuple<string, int>>();
            list3.Add(new Tuple<String, int>("4", 2));
            list3.Add(new Tuple<String, int>("2", -2));
            graph.Add("3", list3);

            //node 4
            graph.Add("4", new List<Tuple<string, int>>());

            incidentEdgesGraph = new Dictionary<string, List<Tuple<string, int>>>();
            incidentEdgesGraph.Add("0", new List<Tuple<string, int>>());
            List<Tuple<string, int>> _list1 = new List<Tuple<string, int>>();
            _list1.Add(new Tuple<string, int>("0", 4));
            _list1.Add(new Tuple<string, int>("2", -3));

            incidentEdgesGraph.Add("1", _list1);

            List<Tuple<string, int>> _list2 = new List<Tuple<string, int>>();
            _list2.Add(new Tuple<string, int>("3", -2));
            incidentEdgesGraph.Add("2", _list2);

            List<Tuple<string, int>> _list3 = new List<Tuple<string, int>>();
            _list3.Add(new Tuple<string, int>("1", -1));
            incidentEdgesGraph.Add("3", _list3);

            List<Tuple<string, int>> _list4 = new List<Tuple<string, int>>();
            _list4.Add(new Tuple<string, int>("3", 4));
            incidentEdgesGraph.Add("4", _list4);



        }
        private static void LoadDirectedTestGraphWithNegativeCycle2()
        {
            graph = new Dictionary<string, List<Tuple<string, int>>>();

            //node 0
            List<Tuple<string, int>> list0 = new List<Tuple<string, int>>();
            list0.Add(new Tuple<String, int>("1", 2));
            list0.Add(new Tuple<String, int>("4", 3));
            graph.Add("0", list0);

            //node 1
            List<Tuple<string, int>> list1 = new List<Tuple<string, int>>();
            list1.Add(new Tuple<String, int>("3", -3));
            list1.Add(new Tuple<String, int>("4", 4));
            graph.Add("1", list1);

            //node 2
            List<Tuple<string, int>> list2 = new List<Tuple<string, int>>();
            list2.Add(new Tuple<String, int>("1", -5));
            list2.Add(new Tuple<String, int>("5", 5));
            graph.Add("2", list2);

            //node 3
            List<Tuple<string, int>> list3 = new List<Tuple<string, int>>();
            list3.Add(new Tuple<String, int>("2", -4));
            graph.Add("3", list3);

            //node 4
            graph.Add("4", new List<Tuple<string, int>>());
            graph.Add("5", new List<Tuple<string, int>>());


            incidentEdgesGraph = new Dictionary<string, List<Tuple<string, int>>>();
            incidentEdgesGraph.Add("0", new List<Tuple<string, int>>());
            List<Tuple<string, int>> _list1 = new List<Tuple<string, int>>();
            _list1.Add(new Tuple<string, int>("0", 2));
            _list1.Add(new Tuple<string, int>("2", -5));

            incidentEdgesGraph.Add("1", _list1);

            List<Tuple<string, int>> _list2 = new List<Tuple<string, int>>();
            _list2.Add(new Tuple<string, int>("3", -4));
            incidentEdgesGraph.Add("2", _list2);

            List<Tuple<string, int>> _list3 = new List<Tuple<string, int>>();
            _list3.Add(new Tuple<string, int>("1", -3));
            incidentEdgesGraph.Add("3", _list3);

            List<Tuple<string, int>> _list4 = new List<Tuple<string, int>>();
            _list4.Add(new Tuple<string, int>("0", 3));
            _list4.Add(new Tuple<string, int>("1", 4));
            incidentEdgesGraph.Add("4", _list4);

            List<Tuple<string, int>> _list5 = new List<Tuple<string, int>>();
            _list5.Add(new Tuple<string, int>("2", 5));
            incidentEdgesGraph.Add("5", _list5);


        }
        private static void LoadDirectedTestGraphWithCycle()
        {
            graph = new Dictionary<string, List<Tuple<string, int>>>();

            //node 0
            List<Tuple<string, int>> list0 = new List<Tuple<string, int>>();
            list0.Add(new Tuple<String, int>("1", 2));
            list0.Add(new Tuple<String, int>("4", 3));
            graph.Add("0", list0);

            //node 1
            List<Tuple<string, int>> list1 = new List<Tuple<string, int>>();
            list1.Add(new Tuple<String, int>("3", -3));
            list1.Add(new Tuple<String, int>("4", 4));
            graph.Add("1", list1);

            //node 2
            List<Tuple<string, int>> list2 = new List<Tuple<string, int>>();
            list2.Add(new Tuple<String, int>("1", 8));
            list2.Add(new Tuple<String, int>("5", 5));
            graph.Add("2", list2);

            //node 3
            List<Tuple<string, int>> list3 = new List<Tuple<string, int>>();
            list3.Add(new Tuple<String, int>("2", -4));
            graph.Add("3", list3);

            //node 4
            graph.Add("4", new List<Tuple<string, int>>());
            graph.Add("5", new List<Tuple<string, int>>());


            incidentEdgesGraph = new Dictionary<string, List<Tuple<string, int>>>();
            incidentEdgesGraph.Add("0", new List<Tuple<string, int>>());
            List<Tuple<string, int>> _list1 = new List<Tuple<string, int>>();
            _list1.Add(new Tuple<string, int>("0", 2));
            _list1.Add(new Tuple<string, int>("2", 8));

            incidentEdgesGraph.Add("1", _list1);

            List<Tuple<string, int>> _list2 = new List<Tuple<string, int>>();
            _list2.Add(new Tuple<string, int>("3", -4));
            incidentEdgesGraph.Add("2", _list2);

            List<Tuple<string, int>> _list3 = new List<Tuple<string, int>>();
            _list3.Add(new Tuple<string, int>("1", -3));
            incidentEdgesGraph.Add("3", _list3);

            List<Tuple<string, int>> _list4 = new List<Tuple<string, int>>();
            _list4.Add(new Tuple<string, int>("0", 3));
            _list4.Add(new Tuple<string, int>("1", 4));
            incidentEdgesGraph.Add("4", _list4);

            List<Tuple<string, int>> _list5 = new List<Tuple<string, int>>();
            _list5.Add(new Tuple<string, int>("2", 5));
            incidentEdgesGraph.Add("5", _list5);


        }
        private static void LoadDirectedTestGraphForJohnsons()
        {
            graph = new Dictionary<string, List<Tuple<string, int>>>();

            //node 1
            List<Tuple<string, int>> list1 = new List<Tuple<string, int>>();
            list1.Add(new Tuple<String, int>("2", -2));

            graph.Add("1", list1);

            //node 2
            List<Tuple<string, int>> list2 = new List<Tuple<string, int>>();
            list2.Add(new Tuple<String, int>("3", -1));

            graph.Add("2", list2);

            //node 3
            List<Tuple<string, int>> list3 = new List<Tuple<string, int>>();
            list3.Add(new Tuple<String, int>("1", 4));
            list3.Add(new Tuple<String, int>("4", 2));
            list3.Add(new Tuple<String, int>("5", -3));
            graph.Add("3", list3);

            //node 4
            graph.Add("4", new List<Tuple<string, int>>());

            //node 5
            graph.Add("5", new List<Tuple<string, int>>());

            List<Tuple<string, int>> list6 = new List<Tuple<string, int>>();
            list6.Add(new Tuple<String, int>("5", -4));
            list6.Add(new Tuple<String, int>("4", 1));
            graph.Add("6", list6);


            incidentEdgesGraph = new Dictionary<string, List<Tuple<string, int>>>();
            List<Tuple<string, int>> _list1 = new List<Tuple<string, int>>();
            _list1.Add(new Tuple<string, int>("3", 4));
            incidentEdgesGraph.Add("1", _list1);

            List<Tuple<string, int>> _list2 = new List<Tuple<string, int>>();
            _list2.Add(new Tuple<string, int>("1", -2));
            incidentEdgesGraph.Add("2", _list2);

            List<Tuple<string, int>> _list3 = new List<Tuple<string, int>>();
            _list3.Add(new Tuple<string, int>("2", -1));
            incidentEdgesGraph.Add("3", _list3);

            List<Tuple<string, int>> _list4 = new List<Tuple<string, int>>();
            _list4.Add(new Tuple<string, int>("3", 2));
            _list4.Add(new Tuple<string, int>("6", 1));
            incidentEdgesGraph.Add("4", _list4);

            List<Tuple<string, int>> _list5 = new List<Tuple<string, int>>();
            _list5.Add(new Tuple<string, int>("3", -3));
            _list5.Add(new Tuple<string, int>("6", -4));
            incidentEdgesGraph.Add("5", _list5);

            incidentEdgesGraph.Add("6", new List<Tuple<string, int>>());
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
        private static void LoadUndirectedTestGraph()
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
        #endregion
    }
}
