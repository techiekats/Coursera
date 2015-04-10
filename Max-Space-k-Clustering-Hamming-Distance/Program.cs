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
        private static Dictionary<uint, uint> G;
        private static uint k = UInt32.MinValue;
        private static int n = Int32.MinValue;
        private static List<List<uint>> preGeneratedXORsequences;
        static void Main(string[] args)
        {
            LoadNodes("clustering_big.txt");
            k = 3;
            ComputeXORSequences();
            FindClusters();
           // Console.WriteLine("Cluster count = " + UF.Values.Distinct().Count());
            Console.ReadKey();
        }

       
        #region Clustering
        private static void FindClusters()
        {
            int n = G.Count;
            var graphNodesEnumerator = G.Keys.GetEnumerator();
            for (int distance = 1; distance <= k; distance++)
            {
                while (graphNodesEnumerator.MoveNext())
                {
                    MarkCloseNeighbours(graphNodesEnumerator.Current, distance);
                }
            }
        }
        private static void MarkCloseNeighbours (uint inputNode, int hammingDistance)
        {
            foreach (int i in preGeneratedXORsequences[hammingDistance - 1])
            {
                uint prospectiveNeighborValue = preGeneratedXORsequences[hammingDistance - 1][i] ^ inputNode;
                if (G.ContainsKey (prospectiveNeighborValue))
                {
                    G[prospectiveNeighborValue] = inputNode;
                }
            }
        }
        #endregion
        
        #region Initialization
        private static void LoadNodes(string filePath)
        {
            StreamReader graphFile = new StreamReader(filePath);

            var t1 = graphFile.Lines().GetEnumerator();
            G = new Dictionary<uint, uint>();
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
                        G.Add(node, node);//self - parent
                    }
               }
            }
            //currentClusterCount = G.Count;
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
