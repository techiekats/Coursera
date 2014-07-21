using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k_cluster
{
    class Program
    {
        static List<Tuple<int,int,int>> graph;
        static List<List<int>> verticesIncluded;
        static int nodeCount;
        static void Main(string[] args)
        {
           /* LoadGraph();
            Console.WriteLine("Max spacing=" + GetMaxSpacing(4));*/
            Console.WriteLine("Cluster count =" + ComputeMaxClustersWithHummingDistance());
            Console.ReadKey();
        }

        private static int ComputeMaxClustersWithHummingDistance()
        {
            StreamReader graphFile = new StreamReader(@"C:\Users\khyati\Documents\GitHub\Coursera\k-cluster\cluster_big_test.txt");
            UnionFindBig uf = new UnionFindBig(int.Parse(graphFile.ReadLine().Trim().Split(' ')[1]));
            var t1 = from line in graphFile.Lines()
                     let items = line.Trim().Split(' ')
                     where items.Length > 3
                     select items.ConvertToInt() ;

            foreach (var a in t1)
            {
                if (uf.Find(a) == int.MinValue)
                {
                    uf.Insert(a);
                }
            }

            return uf.ClusterCount();
        }
        private static void LoadGraph  ()
        {
            //test file op=134365
            StreamReader graphFile = new StreamReader(@"C:\Users\khyati\documents\github\coursera\k-cluster\clusters.txt");
            var t1 = from line in graphFile.Lines()
                     let items = line.Split(' ')
                     //where items.Length > 1
                     select items.Length > 1 ? new { Weight = int.Parse(items[2]), Edge = new Tuple<int, int>(int.Parse(items[0]), int.Parse(items[1])) }
                        : new {Weight = int.MinValue, Edge=new Tuple<int,int> (int.Parse (items[0]), int.Parse (items[0])) };
            t1 = t1.OrderBy(t => t.Weight);
            graph = new List<Tuple<int,int, int>>();
            foreach (var e in t1)
            {
                 graph.Add (new Tuple<int,int,int>(e.Weight,e.Edge.Item1, e.Edge.Item2));
            }
            nodeCount = graph[0].Item2;
            graph.RemoveAt(0);
            verticesIncluded = new List<List<int>>();
            Console.Write ("Graph size = " + graph.Count);
        }
        private static int GetMaxSpacing(short clusterSize)
        {
            UnionFind uf = new UnionFind(nodeCount);   
            foreach (var g in graph)
            {
                /*uf.PrintClusters();
                Console.WriteLine("--------------------");
                Console.ReadKey();*/
                if (uf.AreConnected(g.Item2, g.Item3))
                {
                    //Console.WriteLine(string.Format("{0} {1} are connected. Cluster size = {2}", g.Item2, g.Item3, uf.ClusterCount()));
                    continue;
                }
                else
                {
                    if (uf.ClusterCount() == clusterSize)
                    {
                        return g.Item1;
                    }
                    
                   // Console.WriteLine (string.Format ("Union of {0} {1}", g.Item2, g.Item3));
                    uf.Union(g.Item2, g.Item3);
                }
            }
            return -1;
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
    public static class BitStringsToIntConverter
    {
        public static int ConvertToInt (this string[] bitsString)
        {
            bool[] bits = new bool[bitsString.Length];
            for (short i = 0; i < bitsString.Length; i++ )
            {
                bits[i] = bitsString[i].Equals("1") ? true : false;
            }
            BitArray bitArray = new BitArray(bits);
           // byte [] bytes = new byte[bitsString.Length % 8 != 0 ? bitsString.Length/8 + 1 : bitsString.Length / 8];
            byte[] bytes = new byte[4];
            bitArray.CopyTo(bytes, 0);
            int result = BitConverter.ToInt32(bytes, 0);
            return result;
        }
    }
}
