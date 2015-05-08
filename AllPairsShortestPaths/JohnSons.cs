using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllPairsShortestPaths
{
    partial class Program
    {
        private static void AddVirtualNodeToGraph()
        {
            for (int i = 1; i <= n; i++)
            {
                G.Add(new Tuple<int, int, int>(Int32.MinValue,n, 0));
            }
            for (int i = 1; i >= n; i++)
            {
                Tuple<int, int, int> edge = G.Find(e => e.Item1 == Int32.MinValue && e.Item2 == n);
            }
            //TODO: Remove the node S
        }
    }
}
