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

namespace numbers_distinguishing
{
	public partial class Form1 : Form
	{
		private System.Windows.Forms.TextBox result_tb;
		private System.Windows.Forms.GroupBox template;

		// control[number] -> Pair(seq of 0s and 1s, result)
		private Dictionary<List<double>, int> control;

		public int pointsX { get; private set; }
		public int pointsY { get; private set; }

		private Neuron n;

		public Form1()
		{
			pointsX = 3;
			pointsY = 4;

            n = new Neuron(pointsX * pointsY);

            InitializeComponent();
			CreateFormElements(pointsX, pointsY);

			int steps = Educate();
		}

		private void SetControlVector()
		{
			control = new Dictionary<List<double>, int>(){
				{(new double[] {1,1,1,1,0,1,1,0,1,1,1,1}).ToList<double>(), 1},
				{(new double[] {0,1,1,0,1,1,0,1,0,1,1,1}).ToList<double>(), 0},

				{(new double[] {1,1,1,1,0,1,1,1,0,1,1,1}).ToList<double>(), 1},
				{(new double[] {1,1,1,0,1,1,0,0,1,1,1,1}).ToList<double>(), 0},

				{(new double[] {0,1,0,0,1,0,1,1,0,0,1,0}).ToList<double>(), 1},
				{(new double[] {0,1,1,0,1,1,0,0,1,1,1,0}).ToList<double>(), 0},

				{(new double[] {1,1,1,1,0,0,1,1,1,1,1,1}).ToList<double>(), 1},
				{(new double[] {1,1,1,0,1,1,1,1,0,1,0,0}).ToList<double>(), 0},

				{(new double[] {0,1,1,0,1,1,1,1,0,1,1,0}).ToList<double>(), 1},
				{(new double[] {1,1,1,1,0,1,1,1,1,1,1,0}).ToList<double>(), 0},
			};
			//OpenFileDialog dialog = new OpenFileDialog();
			//dialog.DefaultExt = "txt";
			//dialog.Filter = "Text files (*.txt)";
			//dialog.InitialDirectory = Environment.CurrentDirectory;
			//dialog.ShowDialog();

			//StreamReader f;

			//if (dialog.FileName != null)
			//	f = new StreamReader(dialog.FileName);
			//else
			//	return;


		}

		private void CreateFormElements(int pointsX, int pointsY)
		{
			int x0 = 15, y0 = 15;
			int size = 20;

			template = new GroupBox
			{
				Location = new System.Drawing.Point(10, 10),
				Width = x0 * 2 + size * pointsX - x0 / 2,
				Height = y0 * 2 + size * pointsY - y0 / 2,
				Name = "template"
			};

			List<CheckBox> number = new List<CheckBox>();

			for (int i = 0; i < pointsY; i++)
			{
				for (int j = 0; j < pointsX; j++)
				{
					number.Add(new CheckBox
					{
						Location = new System.Drawing.Point(x0 + j * size, y0 + i * size),
						Width = size,
						Height = size
					});

					template.Controls.Add(number.Last());
				}
			}
			this.Controls.Add(template);

			Button check_btn = new Button
			{
				Text = "Check",
				Name = "check_btn",
				Width = template.Width,
				Location = new System.Drawing.Point(template.Location.X, template.Location.Y + template.Height + 10)
			};
			this.Controls.Add(check_btn);

			check_btn.Click += new System.EventHandler(this.Check_btn_click);

			result_tb = new TextBox
			{
				Name = "result_tb",
				Width = template.Width,
				ReadOnly = true,
				TextAlign = HorizontalAlignment.Center,
				Font = new System.Drawing.Font("Times New Roman", 10.0F),
				Location = new System.Drawing.Point(check_btn.Location.X, check_btn.Location.Y + check_btn.Height + 10),
			};
			this.Controls.Add(result_tb);

			this.Height = 80 + template.Height + check_btn.Height + result_tb.Height;
			this.Width = template.Width + 10;
		}

		private void Check_btn_click(object sender, EventArgs e)
		{
			n.SetX(template);

			if (n.Y == 1)
			{
				result_tb.Text = "EVEN";
			}
			else
			{
				result_tb.Text = "ODD";
			}
		}

		private int Educate()
		{
			bool wrong_answ;
			int count = 0;

            SetControlVector();

            n.SetW();
            n.Teta = 0.0;

            do
			{
				count++;
				wrong_answ = false;
				foreach (var number in control)
				{
					n.SetX(number.Key);
					do
					{
						if (n.Y != number.Value)
						{
							wrong_answ = true;
							switch (n.Y)
							{
								case 0:
									n.IncreaseW();
									break;
								case 1:
									n.DecreaseW();
									break;
								default:
									new ArgumentOutOfRangeException("Wrong param y");
									break;
							}
						}
					} while (n.Y != number.Value);
				}
			} while (wrong_answ);
			return count;
		}
	}
}
