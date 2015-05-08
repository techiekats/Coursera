using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphPointsPlotter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader graphFile = new StreamReader("tsp.txt");
            var t1 = from line in graphFile.Lines()
                     let items = line.Split(' ')
                     //where items.Length > 1
                     select items.Length > 1 ? new { X = float.Parse(items[0]), Y = float.Parse(items[1])}
                        : new { X = float.MinValue, Y = float.MinValue};
            
            int i = 1;
            foreach (var p in t1)
            {
                if (p.X != float.MinValue)
                {
                    DataPoint dp = new DataPoint(p.X, p.Y);
                    dp.ToolTip = string.Format("({0},{1}):{2}", p.X, p.Y, i++);
                    chart1.Series["Series1"].Points.Add(dp);
                }
            }
            chart1.Series["Series1"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            chart1.Series["Series1"].Color = Color.Red;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
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
