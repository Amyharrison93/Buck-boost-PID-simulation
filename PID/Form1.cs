using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PID
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Vin.Text = "24";
            Ind.Text = "0.0006";
            Cap.Text = "0.00033";
            Load.Text = "7";
            Vout.Text = "70";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();

            chart1.Series.Add("Ton * 100");
            chart1.Series["Ton * 100"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series.Add("Vout");
            chart1.Series["Vout"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series.Add("Toff * 100");
            chart1.Series["Toff * 100"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            double
                vin = Convert.ToDouble(Vin.Text),
                ind = Convert.ToDouble(Ind.Text),
                cap = Convert.ToDouble(Cap.Text),
                load = Convert.ToDouble(Load.Text),
                vout = Convert.ToDouble(Vout.Text),
                ton, toff, freq = 0.000033, tontoff,
                Noise = 0, posNeg, setpoint = vout, error=0,
                Prop, Inte, Dir,  errSig = 0, PID, errPrev;

            Random noise = new Random();

            for (int i = 0; i < 999; i++)
            {
                tontoff = vout / vin;
                ton = tontoff / (1 + tontoff);
                toff = ton - freq;

                chart1.Series["Ton * 100"].Points.AddXY(i, (ton*100));
                chart1.Series["Vout"].Points.AddXY(i, vout);
                chart1.Series["Toff * 100"].Points.AddXY(i, toff*100);

                posNeg = noise.Next(0, 101);
                if((posNeg > 50) && ((vout/10) >1))
                {
                    Noise = -(noise.Next(0, Convert.ToInt32(vout / 10)));
                }
                else if ((vout / 10) > 1)
                {
                    Noise = noise.Next(0, Convert.ToInt32(vout / 10));
                }
                errPrev = error;
                vout += Noise;
                error = vout - setpoint;
                errSig += error;
                Prop = 0.75*-error;
                if (i == 0)
                {
                    Inte = 0.08 * (errSig);
                }
                else
                {
                    Inte = 0.4 * (errSig / i);
                }
                Dir = 0.13 * (errPrev - error);
                PID = Prop + Inte + Dir;
                vout += PID;
            }
        }
    }
}
