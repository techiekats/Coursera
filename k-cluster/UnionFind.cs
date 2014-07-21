using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace k_cluster
{
    class UnionFind
    {
        private Dictionary<int, int> vertexLeaderMapping;
        public UnionFind(int nodeCount)
        {
            Dictionary<int, int> load = new Dictionary<int, int>();
            for (int i = 1; i <= nodeCount; i++)
            {
                load.Add(i, i);
            }
            vertexLeaderMapping = load;
        }
        public int Find(int node)
        {
            return vertexLeaderMapping.ContainsKey(node) ? vertexLeaderMapping[node] : int.MinValue;
        }
        public int ClusterCount()
        {
            return vertexLeaderMapping.Values.Distinct().Count();
        }
        public bool AreConnected(int node1, int node2)
        {
            return Find (node1) != int.MinValue && (Find(node1) == Find (node2));
        }
        public void Union(int node1, int node2)
        {
            int parent1 = Find(node1);
            int parent2 = Find(node2);
           /* bool aVertexToBeAdded = (parent1 == int.MinValue || parent2 == int.MinValue);
            if (aVertexToBeAdded)
            {
                int parent = Math.Max(parent1, parent2);
                if (parent == int.MinValue)//i.e. both are absent
                {
                    parent = node1;
                }
                if (vertexLeaderMapping.ContainsKey(node1))
                {
                    vertexLeaderMapping[node1] = parent;
                }
                else
                {
                    vertexLeaderMapping.Add(node1, parent);
                }
                if (vertexLeaderMapping.ContainsKey(node2))
                {
                    vertexLeaderMapping[node2] = parent;
                }
                else
                {
                    vertexLeaderMapping.Add(node2, parent);
                }
            }
            else
            {*/
               if (parent1 != parent2)
                {
                    var ct1 = vertexLeaderMapping.Where(c1 => c1.Value == parent1);
                    var ct2 = vertexLeaderMapping.Where(c2 => c2.Value == parent2);
                    if (ct1.Count() <= ct2.Count())
                    {
                        var ls = ct1.ToArray<KeyValuePair<int, int>>();
                        foreach (var b1 in ls)
                        {
                            vertexLeaderMapping[b1.Key] = parent2;
                        }
                    }
                    else
                    {
                        var ls = ct2.ToArray<KeyValuePair<int, int>>();
                        foreach ( var b2 in ls)
                        {
                            vertexLeaderMapping[b2.Key] = parent1;
                        }
                    }
               // }
            }
        }
        public void PrintClusters()
        {
            int[] distinctClusters = vertexLeaderMapping.Values.Distinct().ToArray<int>();
            foreach (int c in distinctClusters)
            {
                Console.WriteLine("Cluster leader: " + c);
                var subset =  vertexLeaderMapping.Where((k, v) => v == c).ToList<KeyValuePair<int,int>>();
                foreach (var s in subset)
                {
                    Console.WriteLine(string.Format("{0}\t", s.Key));
                }
            }
        }
    }
}
