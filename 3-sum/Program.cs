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
            int [] S = new int [] {3,5,6,2,-5,33,12};
            Console.WriteLine("Checking uniqueness...");

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
                    Console.WriteLine(string.Format("{ {0}, {1}, {2} }", triplet[0], triplet[1], triplet[2]));
                }
            }
            Console.ReadKey();
        }

        private static List<int[]> FindUnique3SumPairs(int[] S)
        {
            List <int[]> _3SumPairs = new List<int[]>();
            //create a hashmap of summations (a+b) => (a,b)
            Dictionary<int, Tuple<int, int>> SummationLookup = CreateSummationLookUp(S);
            //for each element e in the array look up hashmap for (0-e)
            foreach (var e in S)
            {
                //if match found add (e,a,b) to the output.
                var difference = 0 - e;
                if (SummationLookup.ContainsKey (difference))
                {
                    int[] componentElements = new int[3];
                    componentElements[0] = e;
                    componentElements[1] = SummationLookup[difference].Item1;
                    componentElements[2] = SummationLookup[difference].Item2;
                    _3SumPairs.Add(componentElements);
                }
            }
            return _3SumPairs;
            //see if only half the elements need to be scanned
        }

        /// <summary>
        /// Create a map of all possible summations of 2 elements
        /// </summary>
        /// <param name="sortedArray"></param>
        /// <returns></returns>
        private static Dictionary<int, Tuple<int, int>> CreateSummationLookUp(int[] sortedArray)
        {
            Dictionary<int, Tuple<int, int>> SummationLookups = new Dictionary<int, Tuple<int, int>>();
            for (int i = 0; i < sortedArray.Length - 1; i++)
            {
                for (int j= i+1; j < sortedArray.Length; j++)
                {
                    SummationLookups.Add(sortedArray[i] + sortedArray[j], new Tuple<int, int>(sortedArray[i], sortedArray[j]));
                }
            }
            return SummationLookups;
        }
        
    }
}
