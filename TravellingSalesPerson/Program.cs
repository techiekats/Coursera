using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingSalesPerson
{
    class Program
    {
        private const double INFINITY = double.MaxValue;
        private static List<Tuple<double, double>> Points;
        
        static void Main(string[] args)
        {
            //LoadGraph("test1.txt"); //Optimal cost 8.8
            //LoadGraph("test2.txt"); //Optimal cost 7.63
            //LoadGraph("test3.txt"); //Optimal cost 4
            LoadGraph("test4.txt"); 
            //LoadGraph("tsp.txt");
            double minimumTourCost = ComputeMinimumCostOfSubgraph(0, Points.Count()-1);
            Console.WriteLine("TSP cost = " + minimumTourCost);
            Console.ReadKey();
        }
        private static double ComputeMinimumCostOfSubgraph (int startIndex, int endIndex)
        {
            Tuple<double, double> source = Points[startIndex];
            double[][] G = ComputeDistanceMatrix(startIndex, endIndex);
            List<int[]> sets = new List<int[]>();
            List<double> A = new List<double>();
            
            //initialize for m=1
            for (int i = startIndex; i <= endIndex; i++)
            {
                int[] singlePointSet = new int[1];
                singlePointSet[0] = i;
                sets.Add(singlePointSet);                
            }
            //compute sets for others
            for (int j = 2; j <= endIndex-startIndex + 1; j++) //Sub problem size
            {
                sets = ComputeRequiredSubsets(sets, j, startIndex);                
            }
            A = Enumerable.Repeat(INFINITY, sets.Count()).ToList(); A[0] = 0;

            for (int j = 2; j <= endIndex - startIndex + 1; j++)
            {
                //travelling sales man computations
                int subSetStart = sets.FindIndex(t => t.Count() == j);
                int subSetEnd = sets.FindLastIndex(t => t.Count() == j);
                for (int index = subSetStart; index <= subSetEnd; index++)
                {
                    var candidateSubTours = sets.Where(t => t.Count() == j - 1 && t.Intersect(sets[index]).Count() != 0
                        && t.Except(sets[index]).Count() == 0);
                    double minCost = INFINITY;
                    foreach (var tour in candidateSubTours)
                    {
                        int tourIndex = sets.FindIndex (t=> t == tour);
                        if (A[tourIndex] != INFINITY)
                        {
                        var setLeftOut = sets[index].Except(tour);
                            int nodeLeftOut = setLeftOut.Count () == 1? setLeftOut.ToArray()[0] : setLeftOut.ToArray()[1]; //start index will always be the 0th elem
                        double minEuclideanDistance = INFINITY;
                            foreach (int k in tour)
                            {
                                minEuclideanDistance = Math.Min(minEuclideanDistance, G[nodeLeftOut][k]);
                            }
                            minCost = Math.Min(minCost, A[tourIndex] + minEuclideanDistance);
                        }
                    }
                    A[index] = minCost;
                }
            }
            //Final brute force search
            double tourCostFinalSet = A[sets.FindIndex(p => p.Count() == endIndex - startIndex +1)];
            double minimumTourCost = INFINITY;
            var p1 = Points[0];
            for (int j = startIndex + 1; j <= endIndex; j++)
            {
                var p2 = Points[j];
                double eDistance = Math.Sqrt(System.Convert.ToDouble(((p1.Item1 - p2.Item1) * (p1.Item1 - p2.Item1) + (p1.Item2 - p2.Item2) * (p1.Item2 - p2.Item2)).ToString()));
                if (eDistance + tourCostFinalSet < minimumTourCost)
                {
                    minimumTourCost = eDistance + tourCostFinalSet;
                }
            }
            return minimumTourCost;
        }

        private static double[][] ComputeDistanceMatrix(int startIndex, int endIndex)
        {
            double[][] euclideanDistance = new double[endIndex - startIndex + 1] [];
            Parallel.For(startIndex, endIndex+1, i => {
                euclideanDistance[i] = new double[endIndex - startIndex + 1];
                var p1 = Points[i];
                for (int j = startIndex; j <= endIndex; j++)
                {
                    var p2 = Points[j];
                    euclideanDistance[i][j] = Math.Sqrt(System.Convert.ToDouble (((p1.Item1 - p2.Item1) * (p1.Item1 - p2.Item1) + (p1.Item2 - p2.Item2) * (p1.Item2 - p2.Item2)).ToString()));
                }
            });
            return euclideanDistance;
        }

        private static List<int[]> ComputeRequiredSubsets (List<int[]> sets, int dimension, int startNode)
        {
            var s1 = sets.Where(t1 => t1.Count() == dimension - 1 && t1.Contains(startNode));
            var s2 = sets.Where(t2 => t2.Count() == 1);
            foreach (var t1 in s1.ToList())
            {
                foreach (var t2 in s2.ToList())
                {
                    var union = t1.Union(t2).Distinct();
                    if (union.Count() > t1.Count()) //as per construction of s1 and s2; t1 never have lesser elements than t1
                    //also, if count after union is same, the set already exists
                    {
                        if (t1.Last() < t2.Last())//further removal of duplicates
                        {
                            sets.Add(union.ToArray());
                        }
                    }
                }
            }
            return sets;
        }
       
        private static double ComputeMinimumDistanceWithNearestNeighborHeuristic (int startIndex, int endIndex)
        {
            Tuple<double, double> source = Points[startIndex];
            double[][] G = ComputeDistanceMatrix(startIndex, endIndex);
            bool[] isTraversed = new bool [endIndex-startIndex + 1];
            double tourCost = 0;

            int nextIndex = startIndex;
            while (isTraversed.Any (t=> !t))
            {
                double closestDistance = G[nextIndex].Min();
                tourCost += closestDistance;
               
            }
            return tourCost;
        }
        #region HELPER METHODS
        private static void LoadGraph(string filePath) 
        {

            StreamReader graphFile = new StreamReader(filePath);
            var t1 = from line in graphFile.Lines()
                     let items = line.Split(' ')
                     //where items.Length > 1
                     select items.Length > 1 ? new { X = double.Parse(items[0]), Y = double.Parse(items[1]) }
                        : new { X = double.Parse(items[0]), Y = double.MinValue };
            foreach (var p in t1)
            {
                if (p.Y != double.MinValue)
                {
                    Points.Add (new Tuple<double, double>(p.X, p.Y));
                }
                else
                {
                    Points = new List<Tuple<double, double>>((int)p.X);
                }
            }
            Console.WriteLine("Loaded {0} points", Points.Capacity);
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


