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
		private Dictionary<List<int>, int> control;

		public int pointsX { get; private set; }
		public int pointsY { get; private set; }

		private List<int> w;
		private List<int> x;
		private double t;
		private double s;
		private int y;

		public Form1()
		{
			pointsX = 3;
			pointsY = 4;

			InitializeComponent();
			CreateFormElements(pointsX, pointsY);

			Educate();
		}

		private void SetControlVector()
		{
			control = new Dictionary<List<int>, int>(){
				{(new int[] {1,1,1,1,0,1,1,0,1,1,1,1}).ToList<int>(), 1},
				{(new int[] {0,1,1,0,1,1,0,1,0,1,1,1}).ToList<int>(), 0},

				{(new int[] {1,1,1,1,0,1,1,1,0,1,1,1}).ToList<int>(), 1},
				{(new int[] {1,1,1,0,1,1,0,0,1,1,1,1}).ToList<int>(), 0},

				{(new int[] {0,1,0,0,1,0,1,1,0,0,1,0}).ToList<int>(), 1},
				{(new int[] {0,1,1,0,1,1,0,0,1,1,1,0}).ToList<int>(), 0},

				{(new int[] {1,1,1,1,0,0,1,1,1,1,1,1}).ToList<int>(), 1},
				{(new int[] {1,1,1,0,1,1,1,1,0,1,0,0}).ToList<int>(), 0},

				{(new int[] {0,1,1,0,1,1,1,1,0,1,1,0}).ToList<int>(), 1},
				{(new int[] {1,1,1,1,0,1,1,1,1,1,1,0}).ToList<int>(), 0},
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
			s = 0;

			SetX();

			for (int i = 0; i < pointsX * pointsY; i++)
			{
				s += w[i] * x[i];
			}

			if (s >= t)
			{
				result_tb.Text = "EVEN";
			}
			else
			{
				result_tb.Text = "UNEVEN";
			}
		}

		private void SetX()
		{
			for (int i = 0; i < pointsX * pointsY; i++)
			{
				if ((template.Controls[i] as CheckBox).Checked)
					x[i] = 1;
				else
					x[i] = 0;
			}
		}

		private int Educate()
		{
			bool wrong_answ;
			int count = 0;
			Step1();

			do
			{
				count++;
				wrong_answ = false;
				foreach (var number in control)
				{
					Step2(number.Key);
					do
					{
						y = Step3();
						if (y != number.Value)
						{
							wrong_answ = true;
							switch (y)
							{
								case 0:
									Step4_0();
									break;
								case 1:
									Step4_1();
									break;
								default:
									new ArgumentOutOfRangeException("Step 3 returns wrong param y");
									break;
							}
						}
					} while (y != number.Value);
				}
			} while (wrong_answ);
			return count;
		}

		/// <summary>
		/// initializaton of w[i] and t
		/// </summary>
		private void Step1()
		{
			SetControlVector();
			Random rand = new Random();

			w = new List<int>();
			for (int i = 0; i < pointsX * pointsY; i++)
			{
				w.Add(rand.Next(-5, 5));
			}

			t = 0;
		}

		/// <summary>
		/// set x as a sequence of 0s and 1s
		/// </summary>
		private void Step2(List<int> _x)
		{
			x = _x;
		}

		/// <summary>
		/// Sum weights
		/// </summary>
		private int Step3()
		{
			s = 0;
			for (int i = 0; i < pointsX * pointsY; i++)
			{
				s += w[i] * x[i];
			}

			if (s >= t)
			{
				return 1;
			}
			return 0;
		}

		private void Step4_0()
		{
			for (int i = 0; i < pointsX * pointsY; i++)
			{
				if (x[i] == 1)
				{
					w[i] += x[i];
				}
			}
		}

		private void Step4_1()
		{
			for (int i = 0; i < pointsX * pointsY; i++)
			{
				if (x[i] == 1)
				{
					w[i] -= x[i];
				}
			}
		}
	}
}
