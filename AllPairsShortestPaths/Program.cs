using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPairsShortestPaths
{
    partial class Program
    {
        private static Dictionary<string, int> computedDistances;
        private static Dictionary<string, List<Tuple<string, int>>> graph;
        private static Dictionary<string, List<Tuple<string, int>>> incidentEdgesGraph;
        private static int INFINITY = int.MaxValue;
        private static List<Tuple<int, int, int>> G; //source, destination, weight
        private static int n, m; //#vertices, #edges
        static void Main(string[] args)
        {            
            LoadGraphFromFile("g2.txt");
            bool halt = DetectNegativeCycle();
            if (halt)
            {              
                Console.WriteLine("Graph has negative cycle(s)");
            } 
            else
            {
                Console.WriteLine("Graph has no negative cost cycles");
            }
            Console.ReadKey();
        }

        private static void LoadGraphFromFile(string filePath)
        {
            StreamReader graphFile = new StreamReader(filePath);
            var t1 = from line in graphFile.Lines()
                     let items = line.Split(' ')
                     //where items.Length > 1
                     select items.Length > 2 ? new { Weight = int.Parse(items[2]), Edge = new Tuple<int, int>(int.Parse(items[0]), int.Parse(items[1])) }
                        : new { Weight = int.MinValue, Edge = new Tuple<int, int>(int.Parse(items[0]), int.Parse(items[1])) };
            G = new List<Tuple<int, int, int>>();
            foreach (var e in t1)
            {
                G.Add(new Tuple<int, int, int>(e.Edge.Item1, e.Edge.Item2, e.Weight));
            }
            n = G[0].Item1;
            m = G[0].Item2;
            G.RemoveAt(0);
            Console.WriteLine(string.Format ("Loaded graph with {0} vertices and {1} edges", n, m));
        }

        #region ALGORITHMS
        private static Tuple<bool,int> ComputeShortestPathDistance ()
        {
            Dictionary<string, int> retainedTransforms;

            for (var item = incidentEdgesGraph.Values.GetEnumerator(); item.MoveNext(); )
            {
                item.Current.Add(new Tuple<string, int>("0", 0));
            }
            incidentEdgesGraph.Add("0", new List<Tuple<string, int>>());
            List<Tuple<string, int>> temp = graph.Keys.Select(k=> new Tuple<string, int>(k, 0)).ToList();
            graph.Add("0", new List<Tuple<string, int>>(temp));
            Console.WriteLine("Invoking Bellman ford with dummy node");
            bool containsNegativeCycles = ComputeBellmanForShortestPaths();
            //store the edges original weights
            retainedTransforms = new Dictionary<string,int>(computedDistances);
            int shortestDistance = INFINITY;

            if (containsNegativeCycles)
            {
                return new Tuple<bool, int>(containsNegativeCycles, 0);
            }
            else
            {
                //remove the extra nodes and garbage collection
                incidentEdgesGraph = null;
                graph.Remove("0");
                Console.WriteLine("Recomputing weights");
                //recompute edge weights
                List<string> keySetClone = graph.Keys.ToList();
                for (var g = keySetClone.GetEnumerator(); g.MoveNext(); )
                {
                    graph[g.Current] = graph[g.Current].Select(n => { n = new Tuple<string, int>(n.Item1, n.Item2 + computedDistances[g.Current] - computedDistances[n.Item1]); return n; }).ToList();
                }
                for (var g = keySetClone.GetEnumerator(); g.MoveNext();)
                {
                    Console.WriteLine("Invoking Dijkstra for node: " + g.Current);
                    ComputeDijkstraShortestPath(g.Current);
                    //update computed distances
                    var computedDistancesKeySet = computedDistances.Keys.ToList();
                    foreach (var k in computedDistancesKeySet)
                    {
                        if (graph[g.Current].Where(s => s.Item1.Equals(k)).Count() != 0)
                        {
                            computedDistances[k] = graph[g.Current].First(s => s.Item1.Equals(k)).Item2 - retainedTransforms[g.Current] + retainedTransforms[k];
                        }
                    }
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
                matrix[1][index] = INFINITY;
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
                    matrix[1][index] = INFINITY;
                }
                for (int v = 0; v < graph.Count;  v++)
                {
                    int value1 =  matrix[0][v];

                    int value2 = INFINITY;
                    foreach ( var edge in incidentEdgesGraph[v.ToString()])
                    {
                        int temp = matrix[0][int.Parse(edge.Item1)] == INFINITY ? INFINITY //to avoid overflow
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
