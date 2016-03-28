using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*  Problem statement: Given an array S of integers are there 3 elements s.t. a + b + c = 0?
 *  Also, the tuple <a,b,c> should be non descending i.e. a <= b <= c
 *  Find all such unique elements*/
namespace _3_sum
{
    /// <summary>
    /// Solves the 3-sum problem with space complexity: O() and time complexity O()
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            int [] S = GiveInputArray(3);
            Console.WriteLine("Checking uniqueness...");
            if (S.Distinct().Count() != S.Count())
            {
                Console.Write("Please make sure the input array has unique elements.");
                Console.ReadKey();
                return;
            }
            var validPairs  = FindUnique3SumPairs(S.OrderBy(ele=>ele).ToArray());
            if (validPairs.Count == 0)
            {
                Console.WriteLine("No pairs satisfying 3 sum criteria found");
            }
            else
            {
                Console.WriteLine("Valid triplets that add up to 0");
                foreach (var triplet in validPairs)
                {
                    Console.WriteLine(string.Format("( {0}, {1}, {2} )", triplet[0], triplet[1], triplet[2]));
                }
            }
            Console.ReadKey();
        }

        private static List<int[]> FindUnique3SumPairs(int[] S)
        {
            List <int[]> _3SumPairs = new List<int[]>();
            //create a hashmap of summations (a+b) => (a,b)
            Dictionary<int, List<Tuple<int, int>>> SummationLookup = CreateSummationLookUp(S);
            //for each element e in the array look up hashmap for (0-e)
            foreach (var e in S)
            {
                //if match found add (e,a,b) to the output.
                var difference = 0 - e;
                if (SummationLookup.ContainsKey (difference))
                {
                    foreach (var pair in SummationLookup[difference])
                    {
                        int[] componentElements = new int[3];
                        componentElements[0] = e;
                        componentElements[1] = pair.Item1;
                        componentElements[2] = pair.Item2;
                        _3SumPairs.Add(componentElements); 
                    }
                }
            }
            return _3SumPairs;
        }

        /// <summary>
        /// Create a map of all possible summations of 2 elements
        /// </summary>
        private static Dictionary<int, List<Tuple<int, int>>> CreateSummationLookUp(int[] sortedArray)
        {
            Dictionary<int, List<Tuple<int, int>>> SummationLookups = new Dictionary<int, List<Tuple<int, int>>>();
            for (int i = sortedArray.Length - 1; i > 0; i--)
            {
                for (int j= i-1; j >=0; j--)
                {
                    int sum = sortedArray[i] + sortedArray[j];
                    if (!SummationLookups.ContainsKey (sum))
                    {
                        SummationLookups.Add (sum, new List<Tuple<int,int>>());
                    }   
                    if (!SummationLookups.ContainsKey (-1 * sortedArray[j])) //to avoid redundant additions
                    {
                        SummationLookups[sum].Add(new Tuple<int, int>(sortedArray[j], sortedArray[i]));
                    }                
                }
            }
            return SummationLookups;
        }

        /// <summary>
        /// Various pre-build input sequences to test the algorithm
        /// </summary>
        private static int[] GiveInputArray(int inputNumber)
        {
            switch (inputNumber)
            {
                case 0: /*Duplicate elements*/
                    return new int[] {4,6,10,11,-11,2,4 };
                case 1: /*No pairs*/
                    return new int[] {1,2,3,4,5,6,7,8 };
                case 2: /*Multiple triplets, no overlapping numbers*/
                    return new int[] { -3, -7, 19,9,1,-8,14,-5,2,15,17,-4};
                case 3: /*Multiple triplets, overlapping numbers*/
                    return new int[] {15,-12,-7,-10,-8,-5,-3,9,4,3};
                default: return new int[] { 3, 5, 6, 2, -5, 33, 12 };
            }
        }
    }
}
