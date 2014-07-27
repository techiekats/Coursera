using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k_cluster
{
    class UnionFindBig
    {
        private int clusterIndex = 0, numBits;
        private Dictionary<long, int> distanceClusterMapping;
        public UnionFindBig(int numberOfBits)
        {
            distanceClusterMapping = new Dictionary<long, int>();
            numBits = numberOfBits;
        }
        public int Find(long node)
        {
            return distanceClusterMapping.ContainsKey(node) ? distanceClusterMapping[node] : int.MinValue;
        }
        public int ClusterCount()
        {
            return distanceClusterMapping.Values.Distinct().Count();
        }
        public bool AreConnected(int dist1, int dist2)
        {
            return Find (dist1) != int.MinValue && (Find(dist1) == Find (dist2));
        }
        public void Union(int node1, int node2)
        {
            int parent1 = Find(node1);
            int parent2 = Find(node2);
                if (parent1 != parent2)
                {
                    var ct1 = distanceClusterMapping.Where(c1 => c1.Value == parent1);
                    var ct2 = distanceClusterMapping.Where(c2 => c2.Value == parent2);
                    if (ct1.Count() <= ct2.Count())
                    {
                        var ls = ct1.ToArray<KeyValuePair<long, int>>();
                        foreach (var b1 in ls)
                        {
                            distanceClusterMapping[b1.Key] = parent2;
                        }
                    }
                    else
                    {
                        var ls = ct2.ToArray<KeyValuePair<long, int>>();
                        foreach ( var b2 in ls)
                        {
                            distanceClusterMapping[b2.Key] = parent1;
                        }
                    }
            }
        }
        public void Insert (long node)
        {
            ++clusterIndex;
            BitArray rawBytes;
            int byteCount = sizeof(long);
                //numBits % 8 != 0 ? numBits % 8 + 1 : numBits / 8;
            if (BitConverter.IsLittleEndian)
            {
                byte[] allBytes =  BitConverter.GetBytes(node);
                rawBytes = new BitArray (allBytes.Take<byte>(byteCount).ToArray());
               
                distanceClusterMapping.Add(node, clusterIndex); //distance 0

                for (short i = 0; i < numBits - 1; i++)
                {
                    BitArray temp = rawBytes.Clone() as BitArray;
                    temp[i] = !temp[i]; //distance 1
                    Console.WriteLine("Distance 1");
                    byte[] tempBytes = new byte[byteCount];
                    temp.PrintBits();
                    temp.CopyTo(tempBytes, 0);
                    Console.WriteLine("Distance 2");
                    long distance1node = BitConverter.ToInt64(tempBytes, 0);
                    if (distanceClusterMapping.ContainsKey(distance1node))
                    {
                        int newCluster = distanceClusterMapping[distance1node];
                        long[] keysOfCurrentCluster = distanceClusterMapping.Where<KeyValuePair<long, int>>((k, v) => v == clusterIndex).Select((k1) => k1.Key).ToArray<long>();
                        foreach (long key in keysOfCurrentCluster)
                        {
                            distanceClusterMapping.Remove(key);
                            distanceClusterMapping.Add(key, newCluster);
                            Console.WriteLine(string.Format("Merging clusters {0} and {1}", newCluster, clusterIndex));
                            clusterIndex = newCluster;
                        }
                    }
                    else
                    {
                        distanceClusterMapping.Add(distance1node, clusterIndex);
                    }
                    //Assuming the node is the center of the cluster, find the 1-distant and 2-distant points
                   for (int j = i+1 ; j < numBits; j++)
                    {
                        //if (j != i)
                        //{
                            BitArray temp2 = temp.Clone() as BitArray;
                            temp2[j] = !temp2[j];//distance 2
                            temp2.CopyTo(tempBytes, 0);
                            temp2.PrintBits();
                            int distance2Node = BitConverter.ToInt32(tempBytes, 0);
                            if (distanceClusterMapping.ContainsKey(distance2Node))
                            {
                                int newCluster = distanceClusterMapping[distance2Node];
                                long[] keysOfCurrentCluster = distanceClusterMapping.Where<KeyValuePair<long, int>>((k, v) => v == clusterIndex).Select((k1) => k1.Key).ToArray<long>();
                                foreach (long key in keysOfCurrentCluster)
                                {
                                    distanceClusterMapping.Remove(key);
                                    distanceClusterMapping.Add(key, newCluster);
                                    Console.WriteLine(string.Format("Merging clusters {0} and {1}", newCluster, clusterIndex));
                                    clusterIndex = newCluster;
                                }
                            }
                            else
                            {
                                distanceClusterMapping.Add(distance2Node, clusterIndex);
                            }
 //                       }
                    }

                }
            }
            else
            {
                throw new NotImplementedException("Khyati assumed it is Little Endian");
            }
        }
    }

    static class BitArrayPrinter
    {
        public static void PrintBits (this BitArray bits)
        {
            IEnumerator iterator = bits.GetEnumerator();
            while (iterator.MoveNext())
            {
                Console.Write(iterator.Current.ToString().Equals("True") ? 1 : 0);
            }
            Console.Write("\n");
        }
    }

}
