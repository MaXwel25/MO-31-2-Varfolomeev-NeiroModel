using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace MO_31_2_Varfolomeev_NeiroModel
{
    public partial class Form1 : Form
    {
        private double[] inputPixels;
        public Form1()
        {
            InitializeComponent();

            inputPixels = new double[15];
        }

        private void Changing_State_Pixel_Button_Click(object sender, EventArgs e)
        {
            if (((Button)sender).BackColor == Color.Black) //if button black
            {
                ((Button)sender).BackColor = Color.White; //replace color
                inputPixels[((Button)sender).TabIndex] = 1d; //replace index in massiv
            }
            else //if button white
            {
                ((Button)sender).BackColor = Color.Black;
                inputPixels[((Button)sender).TabIndex] = 0d;
            }
        }
        private void Button_SaveTrainSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "train.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n"; //new line

            File.AppendAllText(path, tmpStr);
        }

        private void Button_SaveTestSample_Click(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "test.txt";
            string tmpStr = numericUpDown_NecessaryOutput.Value.ToString();

            for (int i = 0; i < inputPixels.Length; i++)
            {
                tmpStr += " " + inputPixels[i].ToString();
            }
            tmpStr += "\n"; //new line

            File.AppendAllText(path, tmpStr);
        }

    
    }
}
