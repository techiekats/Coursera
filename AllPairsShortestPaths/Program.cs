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
        private static HashSet<Tuple<string, int>> computedDistances;
        private static HashSet<Tuple<string, List<Tuple<string, int>>>> graph;
        private static int INFINITY = int.MaxValue;
        static void Main(string[] args)
        {
            LoadGraphFromTestData2();
            ComputeDijkstraShortestPath();
        }

        private static void ComputeDijkstraShortestPath()
        {
            String nextVertex = SOURCE;
            HashSet<KeyValuePair<string, Tuple<bool, bool>>> computationStatus = new HashSet<KeyValuePair<string, Tuple<bool, bool>>>();
            //Hashmap<String, <isProcessed, isReachable>>

            //construct initial set and compute distances
            computedDistances = new HashSet<Tuple<string, int>>();

            for (var k = graph.GetEnumerator(); k.MoveNext(); )
            {
                String key = k.Current.Item1;
                computedDistances.Add(new Tuple<string, int>(k.Current.Item1, INFINITY));
                computationStatus.Add(new KeyValuePair<string, Tuple<bool, bool>>(k.Current.Item1, new Tuple<bool,bool>(false, false)));
            }
            computedDistances.Add(new Tuple <string, int> (SOURCE, 0));

            while (graph.Count != 0)
            {
                string temp = nextVertex;
                //int distanceOfCurrentVertex = computedDistances get(nextVertex);
                int distanceOfCurrentVertex = computedDistances.First(s=> s.Item1.Equals (nextVertex)).Item2;
                int minimum = INFINITY;
                //update the graph with new calculations
                if (null != graph.First(s=> s.Item1.Equals (nextVertex)))
                {
                    computationStatus.RemoveWhere(s => s.Key.Equals(nextVertex));
                    computationStatus.Add(new KeyValuePair<string, Tuple<bool, bool>>(nextVertex, new Tuple<bool,bool>(true, true)));

                    //computationStatus.put(nextVertex, new Tuple<Boolean, Boolean>(true, true));
                    for (var distanceFromSource = graph.First (e=> e.Item1.Equals(nextVertex)).Item2.GetEnumerator(); distanceFromSource.MoveNext(); )
                    {
                        Tuple<String, int> t = distanceFromSource.Current;
//                        computationStatus.put(t.x, new Tuple<Boolean, Boolean>(computationStatus.get(t.x).x, true));
                        bool previouslyComputedValue = computationStatus.First(st=> st.Key == t.Item1).Value.Item1;
                        computationStatus.RemoveWhere(s => s.Key.Equals(t.Item1));
                        computationStatus.Add(new KeyValuePair<string, Tuple<bool, bool>>
                            (t.Item1, new Tuple<bool, bool>(previouslyComputedValue, true)));
                        
                        if (t.Item2 + distanceOfCurrentVertex < computedDistances.First (s=> s.Item1.Equals (t.Item1)).Item2)
                        {
                            computedDistances.RemoveWhere(s => s.Item1.Equals(t.Item1));
                            computedDistances.Add (new Tuple<string,int> (t.Item1, t.Item2 + distanceOfCurrentVertex));
                            //computedDistances.put(t.x, t.y + distanceOfCurrentVertex);
                        }
                        if (computedDistances.First (p=> p.Item1.Equals (t.Item1)).Item2 < minimum)
                        {
                            nextVertex = t.Item1;
                            minimum = computedDistances.First(p=> p.Item1.Equals (nextVertex)).Item2;
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
                        if (!entry.Value.Item1 && entry.Value.Item2 && computedDistances.First (s=> s.Item1.Equals (entry.Key)).Item2 < minimum)
                        {
                            nextVertex = entry.Key;
                            minimum = computedDistances.First (s=> s.Item1.Equals (entry.Key)).Item2 ;
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
                graph.Remove(graph.First (s=> s.Item1.Equals (temp)));
            }
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
            graph = new HashSet<Tuple<string, List<Tuple<string, int>>>>();

            //node 0
            List<Tuple<string, int>> list0 = new List<Tuple<string, int>>();
            list0.Add(new Tuple<String, int>("1", 4));
            list0.Add(new Tuple<String, int>("7", 8));
            graph.Add(new Tuple<string, List<Tuple<String, int>>>("0", list0));

            //node 1
            List<Tuple<String, int>> list1 = new List<Tuple<String, int>>();
            list1.Add(new Tuple<String, int>("2", 8));
            list1.Add(new Tuple<String, int>("7", 11));
            list1.Add(new Tuple<String, int>("0", 4));

            graph.Add(new Tuple<string, List<Tuple<String, int>>>("1", list1));

            //node 2
            List<Tuple<String, int>> list2 = new List<Tuple<String, int>>();
            list2.Add(new Tuple<String, int>("3", 7));
            list2.Add(new Tuple<String, int>("5", 4));
            list2.Add(new Tuple<String, int>("8", 2));
            list2.Add(new Tuple<String, int>("1", 8));

            graph.Add(new Tuple<string, List<Tuple<String, int>>>("2", list2));

            //node 3
            List<Tuple<String, int>> list3 = new List<Tuple<String, int>>();
            list3.Add(new Tuple<String, int>("4", 9));
            list3.Add(new Tuple<String, int>("5", 14));
            list3.Add(new Tuple<String, int>("2", 7));

            graph.Add(new Tuple<string, List<Tuple<String, int>>>("3", list3));

            //node 4
            List<Tuple<String, int>> list4 = new List<Tuple<String, int>>();
            list4.Add(new Tuple<String, int>("5", 10));
            list4.Add(new Tuple<String, int>("3", 9));

            graph.Add(new Tuple<string, List<Tuple<String, int>>>("4", list4));

            //node 5
            List<Tuple<String, int>> list5 = new List<Tuple<String, int>>();
            list5.Add(new Tuple<String, int>("6", 2));
            list5.Add(new Tuple<String, int>("2", 4));
            list5.Add(new Tuple<String, int>("3", 14));
            list5.Add(new Tuple<String, int>("4", 10));


            graph.Add(new Tuple<string, List<Tuple<String, int>>>("5", list5));

            //node 6
            List<Tuple<String, int>> list6 = new List<Tuple<String, int>>();
            list6.Add(new Tuple<String, int>("7", 1));
            list6.Add(new Tuple<String, int>("8", 6));
            list6.Add(new Tuple<String, int>("5", 2));

            graph.Add(new Tuple<string, List<Tuple<String, int>>>("6", list6));

            //node 7
            List<Tuple<String, int>> list7 = new List<Tuple<String, int>>();
            list7.Add(new Tuple<String, int>("8", 7));
            list7.Add(new Tuple<String, int>("0", 8));
            list7.Add(new Tuple<String, int>("1", 11));
            list7.Add(new Tuple<String, int>("6", 1));

            graph.Add(new Tuple<string, List<Tuple<String, int>>>("7", list7));

            //node 8
            List<Tuple<String, int>> list8 = new List<Tuple<String, int>>();
            list8.Add(new Tuple<String, int>("2", 2));
            list8.Add(new Tuple<String, int>("7", 6));
            list8.Add(new Tuple<String, int>("6", 6));

            graph.Add(new Tuple<string, List<Tuple<String, int>>>("8", list8));
        }

    }
}
