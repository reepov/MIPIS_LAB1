using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MIPIS_LAB1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        public const double g = 9.8;
        public static double h = 0.5;
        public static int N = ((int)(150 / h)) + 1;
        public static double k = 0;
        public static double V = 4 / 3 * Math.PI * 0.1 * 0.1 * 0.1 / 8;
        public static double c = 0.47;
        public static int pch = 7200;
        public static double pv = 1.2041;
        public static double d = 0.1;
        //public static double k = - (c * pv * Math.PI * 0.25 * d * d) / (2 * pch * 4 * 0.333333 * Math.PI * Math.Pow(0.05, 3));
        public static int v0 = 1000;
        public static double a = Math.PI / 4;
        public static double[] x_0_1 = new double[N];
        public static double[] x_0_2 = new double[N];
        public static double[] y_0_1 = new double[N];
        public static double[] y_0_2 = new double[N];
        public static PublicArray[] arrays = new PublicArray[N];
        public PublicArray Func(double t, double x1, double x2, double v1, double v2) => new PublicArray(v1, v2, k * Math.Sqrt(v1 * v1 + v2 * v2) * v1, k * Math.Sqrt(v1 * v1 + v2 * v2) * v2 - g);
        private void button1_Click(object sender, EventArgs e)
        {
            x_0_1[0] = x_0_2[0] = 0;
            y_0_1[0] = v0 * Math.Cos(a);
            y_0_2[0] = v0 * Math.Sin(a);
            for (int i = 0; i < N - 1; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double koef = j == 3 ? 1 : j == 0 ? 0 : 0.5;
                    arrays[j] = h * Func(h * (i + koef), x_0_1[i] + (j == 0 ? 0 : arrays[j - 1].K * koef), x_0_2[i] + (j == 0 ? 0 : arrays[j - 1].L * koef), y_0_1[i] + (j == 0 ? 0 : arrays[j - 1].S * koef), y_0_2[i] + (j == 0 ? 0 : arrays[j - 1].Q * koef));
                }
                x_0_1[i + 1] = x_0_1[i] + (arrays[0].K + 2 * arrays[1].K + 2 * arrays[2].K + arrays[3].K) / 6;
                x_0_2[i + 1] = x_0_2[i] + (arrays[0].L + 2 * arrays[1].L + 2 * arrays[2].L + arrays[3].L) / 6;
                y_0_1[i + 1] = y_0_1[i] + (arrays[0].S + 2 * arrays[1].S + 2 * arrays[2].S + arrays[3].S) / 6;
                y_0_2[i + 1] = y_0_2[i] + (arrays[0].Q + 2 * arrays[1].Q + 2 * arrays[2].Q + arrays[3].Q) / 6;
            }
            chart1.Series[0].ChartType = SeriesChartType.Spline;
            chart1.Series[1].ChartType = SeriesChartType.Spline;
            chart2.Series[0].ChartType = SeriesChartType.Spline;
            double x = 0;
            double xx = 0;
            double Time = 0;
            double length = v0 * v0 / g;
            double height = v0 * v0 / 4 / g;
            for (int i = 0; i < N; i++)
            {
                if (i != 0 && i != N - 1) if (x_0_2[i] > x_0_2[i - 1] && x_0_2[i] > x_0_2[i + 1]) height = x_0_2[i];
                if(i != 0) if (x_0_2[i] < 0 && Time == 0) { length = x_0_1[i]; Time = i * h; }
                double y = x_0_1[i];
                chart1.Series[0].Points.AddXY(x, y);
                y = x_0_2[i];
                chart1.Series[1].Points.AddXY(x, y);
                x += h;
                xx = x_0_1[i];
                chart2.Series[0].Points.AddXY(xx, y);
            }
            label1.Text = $"Максимальная высота полета: {height}. Пройденное расстояние: {length}. Время полета: {Time}";
        }
    }
    public class PublicArray
    {
        public double K, L, S, Q;
        public PublicArray(double k, double l, double s, double q)
        {
            K = k;
            L = l;
            S = s;
            Q = q;
        }
        public PublicArray() { }
        public static PublicArray operator *(double h, PublicArray array) => new PublicArray(array.K * h, array.L * h, array.S * h, array.Q * h);
    }
}
