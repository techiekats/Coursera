using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Max_Space_k_Clustering
{
    class Program
    {
        private static List<Tuple<int, int, int>> G;
        private static List<Tuple<int, int>> UF;
        private static int m, n;
        private static int currentClusterCount;
        static void Main(string[] args)
        {
            LoadGraphAndSortByEdgeWeight("clustering1.txt");
            InitializeUnionFindArray();
            currentClusterCount = n;
            int k = 4;
            /*Cluster test data-2
             * k    max_spacing
             * 2    4472
             * 3    3606
             * 4    1414
             */
            int max_spacing = Int32.MaxValue;
            foreach (var edge in G)
            {
                if (Find (edge.Item1) != Find (edge.Item2))
                {
                    if (currentClusterCount > k)
                    {
                        int newParent = Union(edge.Item1, edge.Item2);
                        currentClusterCount--;
                    }
                    else
                    {
                        max_spacing = edge.Item3;
                        break;
                    }
                }
            }
            Console.WriteLine(string.Format("Max-spacing {0}-clustering:{1}", k, max_spacing));
            Console.ReadKey();
        }
        #region Initialization
        private static void InitializeUnionFindArray()
        {
            UF = new List<Tuple<int, int>>(n+1); //because the edge numbers are not zero based
            for (int i = 0; i <= n; i++)
            {
                UF.Add (new Tuple<int, int>(0, i)); //rank , parent
            }
        }
        private static void LoadGraphAndSortByEdgeWeight(string filePath)
        {
            //test file op=134365
            StreamReader graphFile = new StreamReader(filePath);
            var t1 = from line in graphFile.Lines()
                     let items = line.Split(' ')
                     //where items.Length > 1
                     select items.Length > 1 ? new { Weight = int.Parse(items[2]), Edge = new Tuple<int, int>(int.Parse(items[0]), int.Parse(items[1])) }
                        : new { Weight = int.MinValue, Edge = new Tuple<int, int>(int.Parse(items[0]), int.Parse(items[0])) };
            t1 = t1.OrderBy(t => t.Weight);
            G = new List<Tuple<int, int, int>>();
            foreach (var e in t1)
            {
                G.Add(new Tuple<int, int, int>(e.Edge.Item1, e.Edge.Item2,e.Weight));
            }
            n = G[0].Item2; 
            G.RemoveAt(0);
            Console.WriteLine("Loaded and sorted input graph");
        }
        #endregion

        #region UnionFind Methods
        private static int Find (int vertex)
        {
            int parent = UF[vertex].Item2;
            int currentNode = vertex;
            while (currentNode != parent) //hence root node because self parent
            {
                currentNode = parent;
                parent = UF[currentNode].Item2;
            }
            return parent;
        }
        private static int Union (int v1, int v2)
        {
            int root1 = Find(v1);
            int root2 = Find(v2);
            int rank1 = UF[root1].Item1;
            int rank2 = UF[root2].Item1;
            int newParent;
            if (rank1 == rank2)
            {
                //update ranks at random, say root 1
                UF[root1] = new Tuple<int, int>(++rank1, root1); //since a root node, hence is it's own parent
                newParent = root1;
            }
            else
            {
                newParent = (rank1 > rank2) ? root1 : root2;
            }
            
            ApplyPathCompression(v1, newParent);
            ApplyPathCompression(v2, newParent);
            return newParent;
        }
        private static void ApplyPathCompression (int vertex, int parent)
        {
            int currentNode = vertex;
            while (currentNode != parent)
            {
                int temp = UF[currentNode].Item2;
                UF[currentNode] = new Tuple<int, int>(UF[currentNode].Item1, parent);
                currentNode = temp;
            }
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
