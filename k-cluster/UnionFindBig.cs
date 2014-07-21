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
        private Dictionary<int, int> distanceClusterMapping;
        public UnionFindBig(int numberOfBits)
        {
            distanceClusterMapping = new Dictionary<int, int>();
            numBits = numberOfBits;
        }
        public int Find(int node)
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
                        var ls = ct1.ToArray<KeyValuePair<int, int>>();
                        foreach (var b1 in ls)
                        {
                            distanceClusterMapping[b1.Key] = parent2;
                        }
                    }
                    else
                    {
                        var ls = ct2.ToArray<KeyValuePair<int, int>>();
                        foreach ( var b2 in ls)
                        {
                            distanceClusterMapping[b2.Key] = parent1;
                        }
                    }
            }
        }
        public void Insert (int node)
        {
            ++clusterIndex;
            BitArray rawBytes;
            int byteCount = numBits % 8 != 0 ? numBits % 8 + 1 : numBits / 8;
            if (BitConverter.IsLittleEndian)
            {
                byte[] allBytes =  BitConverter.GetBytes(node);
                rawBytes = new BitArray (allBytes.Take<byte>(byteCount).ToArray());
               
                distanceClusterMapping.Add(node, clusterIndex); //distance 0

                for (short i = 0; i < numBits - 1; i++)
                {
                    BitArray temp = rawBytes;
                    temp[i] = !temp[i]; //distance 1
                    byte[] tempBytes = new byte[byteCount];
                    temp.CopyTo(tempBytes, 0);

                    distanceClusterMapping.Add(BitConverter.ToInt32(tempBytes, 0), clusterIndex);
                   /* for (int j = i + 1 ; j < numBits; j++)
                    {
                        BitArray temp2 = temp;
                        temp2[j] = !temp2[j];//distance 2
                        temp2.CopyTo(tempBytes, 0);
                        distanceClusterMapping.Add(BitConverter.ToInt32(tempBytes, 0), clusterIndex);
                    }*/

                }
            }
            else
            {
                throw new NotImplementedException("Khyati assumed it is Little Endian");
            }
        }
    }
}
