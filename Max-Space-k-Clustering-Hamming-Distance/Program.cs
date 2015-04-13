using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Max_Space_k_Clustering_Hamming_Distance
{
    class Program
    {
        private static Dictionary<uint, Tuple<uint, uint>> G;
        private static uint k = UInt32.MinValue;
        private static int n = Int32.MinValue;
        private static int clusterCount;
        private static List<List<uint>> preGeneratedXORsequences;
        static void Main(string[] args)
        {
            /*//UTC 1
            LoadNodes("clustering_test_1.txt");
            k = 1; //Correct output = 4*/

            /*//UTC 2
            LoadNodes("clustering_test_2.txt");
            k = 1; //Correct output = 1*/
            /*//UTC 3
            LoadNodes("clustering_test_3.txt");
            k = 2;//Correct output = 3*/
            /*//UTC 4
            LoadNodes("clustering_test_4.txt");
            k = 2;//Correct output = 11*/
            /*//UTC 5
            LoadNodes("clustering_test_5.txt");
            k = 2;//Correct output = 989*/
            //FINAL RUN
            LoadNodes("clustering_big.txt");
            k = 2;//OUTPUT=6118 !!!!*/
            ComputeXORSequences();
            FindClusters();
            Console.WriteLine("Cluster count = " + clusterCount);
            Console.ReadKey();
        }

       
        #region Clustering
        private static void FindClusters()
        {
            int n = G.Count;
            List<uint> nodes = G.Keys.ToList();
            for (int distance = 1; distance <= k; distance++)
            {
                var graphNodesEnumerator = nodes.GetEnumerator();
                while (graphNodesEnumerator.MoveNext())
                {
                    MarkCloseNeighbours(graphNodesEnumerator.Current, distance);
                }
            }

            //simply maintain a list of parent pointers and in the end identify distinct parents
            //nodes that have not participated at all are single point clusters and hence will be counted in the calculation
        }
        private static void MarkCloseNeighbours (uint inputNode, int hammingDistance)
        {
            foreach (uint i in preGeneratedXORsequences[hammingDistance - 1])
            {
                uint prospectiveNeighborValue = i ^ inputNode;
                if (G.ContainsKey (prospectiveNeighborValue))
                {
                    if (Find(prospectiveNeighborValue) != Find(inputNode))
                    {
                        Union(prospectiveNeighborValue, inputNode);
                        clusterCount--;
                    }
                }
            }
        }
        #endregion

        #region Union Find members
        private static uint Find(uint vertex)
        {
            uint parent = G[vertex].Item1;
            uint currentNode = vertex;
            while (currentNode != parent) //hence root node because self parent
            {
                currentNode = parent;
                parent = G[currentNode].Item1;
            }
            return parent;
        }
        private static uint Union(uint v1, uint v2)
        {
            uint root1 = Find(v1);
            uint root2 = Find(v2);
            uint rank1 = G[root1].Item2;
            uint rank2 = G[root2].Item2;
            uint newParent;
            if (rank1 == rank2)
            {
                //update ranks at random, say root 1
                G[root1] = new Tuple<uint, uint>(root1, ++rank1); //since a root node, hence is it's own parent
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
        private static void ApplyPathCompression(uint vertex, uint parent)
        {
            uint currentNode = vertex;
            while (currentNode != parent)
            {
                uint temp = G[currentNode].Item1;
                G[currentNode] = new Tuple<uint, uint>(parent, G[currentNode].Item2);
                currentNode = temp;
            }
        }
        
        #endregion

        #region Initialization
        private static void LoadNodes(string filePath)
        {
            StreamReader graphFile = new StreamReader(filePath);

            var t1 = graphFile.Lines().GetEnumerator();
            G = new Dictionary<uint, Tuple<uint, uint>>();
            while (t1.MoveNext ())
            {
               if (k == UInt32.MinValue)
               {
                   k = uint.Parse(t1.Current.Split(' ')[1]);
               }
               else
               {
                   uint node = t1.Current.Replace(" ", string.Empty).ConvertToUnsignedInt();
                   n = t1.Current.Replace(" ", string.Empty).Length;
                    if (!G.ContainsKey (node))
                    {
                        G.Add(node, new Tuple <uint, uint> (node,0));//self - parent, rank
                    }
               }
            }
            clusterCount = G.Count();
        }

        private static void ComputeXORSequences()
        {
            preGeneratedXORsequences = new List<List<uint>>();
            for (int distance = 1; distance <=k; distance++)
            {
                preGeneratedXORsequences.Add(new List<uint>());
                if (distance==1) //base case
                {
                    for (int j = 0; j < n; j++)
                    {
                        preGeneratedXORsequences[distance - 1].Add(uint.Parse (Math.Pow(2, j).ToString()));
                    }
                }
                else
                {
                    for (int i = 0; i < preGeneratedXORsequences[0].Count(); i++)
                    {
                        for (int j = 0; j < preGeneratedXORsequences[distance - 2].Count(); j++)
                        {
                                uint hammingXOR = preGeneratedXORsequences[0][i] ^ preGeneratedXORsequences[distance - 2][j];                  
                                var bitRepresentation = new BitArray (BitConverter.GetBytes(hammingXOR));
                                short hammingDistance = 0;
                                var bitsEnumerator = bitRepresentation.GetEnumerator(); 
                                while (bitsEnumerator.MoveNext())
                                {
                                    if (bitsEnumerator.Current.Equals(true))
                                    {
                                        hammingDistance++;
                                    }
                                }
                                if (hammingDistance == distance)
                                {
                                    if (!preGeneratedXORsequences[distance-1].Contains (hammingXOR))
                                    preGeneratedXORsequences[distance - 1].Add(hammingXOR);
                                }
                            
                        }
                    }
                }
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
    public static class BitStringsToIntConverter
    {
        public static int ConvertToInt(this string bitsString)
        {
            bool[] bits = new bool[bitsString.Length];
            for (short i = 0; i < bitsString.Length; i++)
            {
                bits[i] = bitsString[i] == '1' ? true : false;
            }
            BitArray bitArray = new BitArray(bits);
            // byte [] bytes = new byte[bitsString.Length % 8 != 0 ? bitsString.Length/8 + 1 : bitsString.Length / 8];
            byte[] bytes = new byte[sizeof(int)];
            bitArray.CopyTo(bytes, 0);
            int result = BitConverter.ToInt32(bytes, 0);
            return result;
        }
        public static uint ConvertToUnsignedInt (this string bitsString)
        {
            bool[] bits = new bool[bitsString.Length];
            for (short i = 0; i < bitsString.Length; i++)
            {
                bits[i] = bitsString[i] == '1' ? true : false;
            }
            BitArray bitArray = new BitArray(bits);
            // byte [] bytes = new byte[bitsString.Length % 8 != 0 ? bitsString.Length/8 + 1 : bitsString.Length / 8];
            byte[] bytes = new byte[sizeof(uint)];
            bitArray.CopyTo(bytes, 0);
            uint result = BitConverter.ToUInt32(bytes, 0);
            return result;

        }
    }  
}
