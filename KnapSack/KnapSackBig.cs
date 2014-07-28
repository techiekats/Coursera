using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnapSack
{
    class KnapSackBig
    {
        uint[] itemValues;
        uint[] itemWeights;
        uint numberOfItems;
        uint weightLimit;
        //uint[][] matrix;
        SqlConnection conn;
        public KnapSackBig(uint numberOfValues, uint maxSize, uint[] itemValues, uint[] itemWeights)
        {
            this.numberOfItems = numberOfValues;
            this.weightLimit = maxSize;
            this.itemValues = itemValues;
            this.itemWeights = itemWeights;
        }
        public uint KS()
        {
            //LoadTable();

            conn = new SqlConnection("Data Source=.\\sqlexpress;Initial Catalog=knapsack;Integrated Security=True;Pooling=False;Connection Timeout=1800000;");
            conn.Open();
            uint solution = Solve();
            return solution;
        }
        private void LoadTable ()
        {
            conn = new SqlConnection("Data Source=.\\sqlexpress;Initial Catalog=knapsack;Integrated Security=True;Pooling=False;Connection Timeout=1800000;");
            conn.Open();
            Console.WriteLine(conn.ConnectionTimeout);
            string commandText = string.Format("exec [dbo].[InitMatrix] @rows={0}, @cols={1}", numberOfItems, weightLimit);
            SqlCommand cmd = new SqlCommand(commandText, conn);
            cmd.ExecuteNonQuery();
            Console.WriteLine("loaded table");
        }
        private uint Get (uint row, uint col)
        {
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
            return uint.Parse(new SqlCommand(string.Format("select CellValue from matrix where rowno={0} and colno={1}", row, col), conn).ExecuteScalar().ToString());
            //return matrix [row][col];
        }
        private void Set (uint row, uint col, uint val)
        {
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
            new SqlCommand(string.Format("update matrix set cellvalue={0} where rowno={1} and colno={2}", val, row, col), conn).ExecuteNonQuery();
            //matrix[row][col] = val;
        }
        private uint Solve()
        {
            for (uint i = 0; i < numberOfItems; i++)
            {
                Console.WriteLine(string.Format("i={0}", i));

                for (uint j = 1; j <= weightLimit; j++)
                {
                    if (i == 0)
                    {
                        //matrix[i][j - 1] = itemWeights[i] <= j ? itemValues[i] : 0;
                        Set(i, j - 1, itemWeights[i] <= j ? itemValues[i] : 0);
                        continue;
                    }
                    if (j < itemWeights[i])
                    {
                        //matrix[i][j - 1] = matrix[i - 1][j - 1];
                        Set(i, j - 1, Get(i - 1, j - 1));
                        continue;
                    }
                    uint candidate1 = Get(i - 1, j - 1); //matrix[i - 1][j - 1]; //take prev solution
                    uint baseValue = (j == itemWeights[i]) ? 0 : Get(i-1, j-itemWeights[i]-1); //matrix[i - 1][j - itemWeights[i] - 1];
                    uint candidate2 = baseValue + itemValues[i]; //include current item
                    Set(i, j - 1, Math.Max(candidate1, candidate2));
                    //matrix[i][j - 1] = Math.Max(candidate1, candidate2);
                }
            }
            return Get(numberOfItems - 1, weightLimit - 1);//matrix[numberOfItems - 1][weightLimit - 1];
        }
    }
}
