using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace numbers_distinguishing
{
    class Neuron
    {
        private List<double> X;
        public int Y
        {
            get
            {
                if (S >= Teta)
                    return 1;
                return 0;
            }
        }

        public int Entries
        {
            get
            {
                return X.Count;
            }
        }

        private List<double> W;

        public double Teta { get; set; }
        public double S
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < Entries; i++)
                {
                    sum += X[i] * W[i];
                }
                return sum;
            }
        }

        public Neuron(int entries)
        {
            X = new List<double>(entries);
            W = new List<double>(entries);

            for (int i = 0; i < entries; i++)
            {
                X.Add(0.0);
                W.Add(0.0);
            }
        }

        public void SetX(GroupBox template)
        {
            for (int i = 0; i < X.Count; i++)
            {
                if ((template.Controls[i] as CheckBox).Checked)
                    X[i] = 1;
                else
                    X[i] = 0;
            }
        }
        public void SetX(List<double> _x)
        {
            X = new List<double>(_x);
        }
        public void SetW()
        {
            Random rand = new Random();

            for (int i = 0; i < W.Count; i++)
            {
                W[i] = rand.Next(-5, 5) + rand.NextDouble();
            }
        }

        public void IncreaseW()
        {
            for (int i = 0; i < W.Count; i++)
            {
                if (X[i] > 0)
                {
                    W[i] += X[i];
                }
            }
        }
        public void DecreaseW()
        {
            for (int i = 0; i < W.Count; i++)
            {
                if (X[i] > 0)
                {
                    W[i] -= X[i];
                }
            }
        }
    }
}
