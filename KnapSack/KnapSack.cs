﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnapSack
{
    class Knapsack
    {
        int[] itemValues;
        int[] itemWeights;
        int weightLimit;
        int numberOfItems;
        int[][] matrix;

        public Knapsack(int[] itemVals, int[] itemWts, int wtLimit, int numItems)
        {
            itemValues = itemVals;
            itemWeights = itemWts;
            weightLimit = wtLimit;
            numberOfItems = numItems;
            matrix = new int[numItems][];
        }

        public int SolveKnapSack ()
        {
           // return Solve(numberOfItems - 1, weightLimit);
            int soln = KS();
            //PrintSolution();

            return soln;
        }

        private int Solve(int index, int size)
        {
            int take, dontTake;
            take = dontTake = 0;
            matrix[index] = new int[weightLimit + 1];
            if (matrix[index][size] != 0)
            {
                return matrix[index][size];
            }

            if (index == 0)
            {
                if (itemWeights[0] <= size)
                {
                    matrix[index][size] = itemValues[0];
                    return itemValues[0];
                }
                else
                {
                    matrix[index][size] = 0;
                    return 0;
                }
            }
            if (itemWeights[index] <= size)
                take = itemWeights[index] + Solve(index - 1, size - itemWeights[index]);

            dontTake = Solve(index - 1, size);

            matrix[index][size] = Math.Max(take, dontTake);

            return matrix[index][size];
        }

        private int KS()
        {
            for (int i = 0; i < numberOfItems; i++)
            {
                matrix [i] = new int[weightLimit];
                for (int j = 1; j <= weightLimit; j++)
                {
                    if (i == 0)
                    {
                        matrix[i][j-1] = itemWeights[i] <= j ? itemValues[i] : 0;
                        continue;
                    }
                    if (j < itemWeights[i])
                    {
                        matrix[i][j-1] = matrix[i - 1][j-1];
                        continue;
                    }
                    int candidate1 = matrix[i-1][j-1]; //take prev solution
                    int baseValue = (j == itemWeights[i]) ? 0 : matrix[i - 1][j - itemWeights[i] - 1];
                    int candidate2 = baseValue + itemValues[i]; //include current item
                    matrix[i][j - 1] = Math.Max(candidate1, candidate2);  
                }
            }
            return matrix [numberOfItems-1][weightLimit-1];
        }

        private void PrintSolution()
        {
            for (int i = 0; i < numberOfItems; i++)
            {
                for (int j = 0; j < weightLimit; j++)
                {
                    Console.Write(matrix[i][j] + "\t");
                }
                Console.WriteLine();
            }
        }
    }
}
