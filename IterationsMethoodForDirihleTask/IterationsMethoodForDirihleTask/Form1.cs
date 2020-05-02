using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IterationsMethoodForDirihleTask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label15.Visible = false;
            textBox6.Visible = false;
            checkBox1.Visible = false;
            label5.Visible = false;
            textBox5.Visible = false;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int NMAX = Convert.ToInt32(textBox1.Text);
            double EPSMAX = Convert.ToDouble(textBox2.Text);

            int n = Convert.ToInt32(textBox3.Text);
            int m = Convert.ToInt32(textBox4.Text);
            int k = Convert.ToInt32(textBox5.Text);

            double[,] Vtest = new double[m + 1, n + 1]; //для шага n
            double[,] rtest = new double[m - 1, n - 1];

            double[,] V2test = new double[2 * m + 1, 2 * n + 1]; // для шага 2n
            double[,] r2test = new double[2 * m - 1, 2 * n - 1];

            MainTask tmain = new MainTask(0, 1, 0, 2, n, m, NMAX, EPSMAX, Vtest, rtest); //для сетки n
            MainTask t2main = new MainTask(0, 1, 0, 2, 2 * n, 2 * m, NMAX, EPSMAX, V2test, r2test); //для сетки n

            double omega = 0, omega2 = 0;

            if (comboBox3.SelectedIndex == 1)
            {
                tmain.ChebyshevMethood(k);
                t2main.ChebyshevMethood(k);
            }
            else if (comboBox3.SelectedIndex == 0)
            {
                tmain.MinMethood();
                t2main.MinMethood();
            }
            else
            {
                omega = Convert.ToDouble(textBox6.Text);
                omega2 = Convert.ToDouble(textBox6.Text);
                if (checkBox1.Checked)
                {
                    omega = tmain.OptimumOmega();
                    omega2 = t2main.OptimumOmega();
                    textBox6.Text = Convert.ToString(omega);
                }
                tmain.Relaxation(omega);
                t2main.Relaxation(omega2);
            }
            //заполнение таблицы, таблица - координатная плоскость
            if (comboBox1.SelectedIndex == 0)
            {
                dataGridView1.RowCount = n + 1;
                dataGridView1.ColumnCount = m + 1;
                for (int j = 0; j < m + 1; j++)
                    for (int i = 0; i < n + 1; i++)
                        dataGridView1.Rows[n - j].Cells[i].Value = tmain.V[j, i];
                label10.Text = "Для решения на сетке n потребовалось N = " + Convert.ToString(tmain.N) + " итераций";
                label9.Text = "Для решения на сетке 2n потребовалось N = " + Convert.ToString(t2main.N) + " итераций";
                label11.Text = " = " + Convert.ToString(tmain.GetEpsMax(t2main));
                label12.Text = "Достигнутая погрешность итерационного метода eps = " + Convert.ToString(tmain.eps);
                label28.Text = " в точке x = " + Convert.ToString(tmain.xmax) + " y = " + Convert.ToString(tmain.ymax);
                label30.Text = "Невязка по норме бесконечность: " + Convert.ToString(tmain.GetResidual());
                if (comboBox3.SelectedIndex == 2)
                {
                    label10.Text += " (" + Convert.ToString(omega) + ")";
                    label9.Text += " (" + Convert.ToString(omega2) + ")";
                }
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                dataGridView1.RowCount = 2 * n + 1;
                dataGridView1.ColumnCount = 2 * m + 1;
                for (int j = 0; j < 2 * m + 1; j++)
                    for (int i = 0; i < 2 * n + 1; i++)
                        dataGridView1.Rows[2 * n - j].Cells[i].Value = t2main.V[j, i];
                label10.Text = "Для решения  на сетке n потребовалось N = " + Convert.ToString(tmain.N) + " итераций";
                label9.Text = "Для решения на сетке 2n потребовалось N = " + Convert.ToString(t2main.N) + " итераций";
                label11.Text = " = " + Convert.ToString(tmain.GetEpsMax(t2main));
                label12.Text = "Достигнутая погрешность итерационного метода eps = " + Convert.ToString(tmain.eps);
                label28.Text = " в точке x = " + Convert.ToString(tmain.xmax) + " y = " + Convert.ToString(tmain.ymax);
                label30.Text = "Невязка по норме бесконечность: " + Convert.ToString(tmain.GetResidual());
                if (comboBox3.SelectedIndex == 2)
                {
                    label10.Text += " (" + Convert.ToString(omega) + ")";
                    label9.Text += " (" + Convert.ToString(omega2) + ")";
                }
            }
            else
            {
                dataGridView1.RowCount = n + 1;
                dataGridView1.ColumnCount = m + 1;
                for (int j = 0; j < m + 1; j++)
                    for (int i = 0; i < n + 1; i++)
                        dataGridView1.Rows[n - j].Cells[i].Value = Math.Abs(tmain.V[j, i] - t2main.V[2 * j, 2 * i]);
                label10.Text = "Для решения на сетке n потребовалось N = " + Convert.ToString(tmain.N) + " итераций";
                label9.Text = "Для решения на сетке 2n потребовалось N = " + Convert.ToString(t2main.N) + " итераций";
                label11.Text = " = " + Convert.ToString(tmain.GetEpsMax(t2main));
                label12.Text = "Достигнутая погрешность итерационного метода eps = " + Convert.ToString(tmain.eps);
                label28.Text = " в точке x = " + Convert.ToString(tmain.xmax) + " y = " + Convert.ToString(tmain.ymax);
                label30.Text = "Невязка по норме бесконечность: " + Convert.ToString(tmain.GetResidual());
                if (comboBox3.SelectedIndex == 2)
                {
                    label10.Text += " (" + Convert.ToString(omega) + ")";
                    label9.Text += " (" + Convert.ToString(omega2) + ")";
                }
            }

            pictureBox2.Image = Image.FromFile("Z:/projectvs/IterationsMethoodForDirihleTask/IterationsMethoodForDirihleTask/Resources/е2.png");
            pictureBox2.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int NMAX = Convert.ToInt32(textBox1.Text);
            double EPSMAX = Convert.ToDouble(textBox2.Text);

            int n = Convert.ToInt32(textBox3.Text);
            int m = Convert.ToInt32(textBox4.Text);
            int param = Convert.ToInt32(textBox5.Text);

            double h = 1.0 / n;
            double k = 2.0 / m;

            double[,] Vtest = new double[m + 1, n + 1];
            double[,] rtest = new double[m - 1, n - 1];

            TestTask tmain = new TestTask(0, 1, 0, 2, n, m, NMAX, EPSMAX, Vtest, rtest); //для сетки n
            //заполнение таблицы, таблица - координатная плоскость
            if (comboBox2.SelectedIndex == 0)
            {
                dataGridView1.RowCount = n + 1;
                dataGridView1.ColumnCount = m + 1;
                for (int j = 0; j < m + 1; j++)
                    for (int i = 0; i < n + 1; i++)
                        dataGridView1.Rows[n - j].Cells[i].Value = tmain.ftest(i * h, j * k);
                label10.Text = ""; label9.Text = "В таблице показаны точные значения тестовой задачи";
                label11.Text = ""; label12.Text = ""; label28.Text = ""; label30.Text = "";
                pictureBox2.Visible = false;
            }
            else if (comboBox2.SelectedIndex == 1)
            {
                if (comboBox3.SelectedIndex == 1)
                    tmain.ChebyshevMethood(param);
                else if (comboBox3.SelectedIndex == 0)
                    tmain.MinMethood();
                else
                {
                    double omega = Convert.ToDouble(textBox6.Text);
                    if (checkBox1.Checked)
                    {
                        omega = tmain.OptimumOmega();
                        textBox6.Text = Convert.ToString(omega);
                    }
                    tmain.Relaxation(omega);
                }
                dataGridView1.RowCount = n + 1;
                dataGridView1.ColumnCount = m + 1;
                for (int j = 0; j < m + 1; j++)
                    for (int i = 0; i < n + 1; i++)
                        dataGridView1.Rows[n - j].Cells[i].Value = tmain.V[j, i];
                label10.Text = "Для решения потребовалось N = " + Convert.ToString(tmain.N) + " итераций";
                label9.Text = "";
                label11.Text = " = " + Convert.ToString(tmain.GetEpsMax());
                label12.Text = "Достигнутая погрешность итерационного метода eps = " + Convert.ToString(tmain.eps);
                label28.Text = " в точке x = " + Convert.ToString(tmain.xmax) + " y = " + Convert.ToString(tmain.ymax);
                label30.Text = "Невязка по норме бесконечность: " + Convert.ToString(tmain.GetResidual());
                pictureBox2.Visible = true;
            }
            else
            {
                if (comboBox3.SelectedIndex == 1)
                    tmain.ChebyshevMethood(param);
                else if (comboBox3.SelectedIndex == 0)
                    tmain.MinMethood();
                else
                {
                    double omega = Convert.ToDouble(textBox6.Text);
                    if (checkBox1.Checked)
                    {
                        omega = tmain.OptimumOmega();
                        textBox6.Text = Convert.ToString(omega);
                    }
                    tmain.Relaxation(omega);
                }
                dataGridView1.RowCount = n + 1;
                dataGridView1.ColumnCount = m + 1;
                for (int j = 0; j < m + 1; j++)
                    for (int i = 0; i < n + 1; i++)
                        dataGridView1.Rows[n - j].Cells[i].Value = Math.Abs(tmain.V[j, i] - tmain.ftest(i * h, j * k));
                label10.Text = "Для решения потребовалось N = " + Convert.ToString(tmain.N) + " итераций";
                label9.Text = "";
                label11.Text = " = " + Convert.ToString(tmain.GetEpsMax());
                label12.Text = "Достигнутая погрешность итерационного метода eps = " + Convert.ToString(tmain.eps);
                label28.Text = " в точке x = " + Convert.ToString(tmain.xmax) + " y = " + Convert.ToString(tmain.ymax);
                label30.Text = "Невязка по норме бесконечность: " + Convert.ToString(tmain.GetResidual());
                pictureBox2.Visible = true;
            }


            pictureBox2.Image = Image.FromFile("Z:/projectvs/IterationsMethoodForDirihleTask/IterationsMethoodForDirihleTask/Resources/е1.png");
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                label15.Visible = false;
                textBox6.Visible = false;
                checkBox1.Visible = false;
                label5.Visible = false;
                textBox5.Visible = false;
            }
            else if (comboBox3.SelectedIndex == 1)
            {
                label15.Visible = false;
                textBox6.Visible = false;
                checkBox1.Visible = false;
                label5.Visible = true;
                textBox5.Visible = true;
            }
            else
            {
                label15.Visible = true;
                textBox6.Visible = true;
                checkBox1.Visible = true;
                label5.Visible = false;
                textBox5.Visible = false;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int NMAX = Convert.ToInt32(textBox12.Text);
            double EPSMAX = Convert.ToDouble(textBox11.Text);

            int n = Convert.ToInt32(textBox8.Text);
            int m = Convert.ToInt32(textBox9.Text);

            double h = 1.0 / n;
            double k = 2.0 / m;

            double[,] V = new double[m + 1, n + 1];

            MineTask tmain = new MineTask(0, 1, 0, 2, n, m, NMAX, EPSMAX, V); //для сетки n
            //заполнение таблицы
            if (comboBox5.SelectedIndex == 0)
            {
                dataGridView2.Rows.Clear();
                dataGridView2.RowCount = n + 1;
                dataGridView2.ColumnCount = m + 1;
                for (int j = 0; j < m + 1; j++)
                    for (int i = 0; i < n + 1; i++)
                    {
                        if ((m * 0.25 <= j && j <= 3 * m * 0.25) && (n * 0.25 <= i && i <= 3 * n * 0.25))
                        {
                            dataGridView2.Rows[n - j].Cells[i].Value = 0;
                            dataGridView2.Rows[n - j].Cells[i].Style.BackColor = Color.Green;
                        }
                        else if ((m * 0.5 <= j && j <= 3 * m * 0.25) && (3 * n * 0.25 <= i))
                        {
                            dataGridView2.Rows[n - j].Cells[i].Value = 0;
                            dataGridView2.Rows[n - j].Cells[i].Style.BackColor = Color.Green;
                        }
                        else if (3 * m * 0.25 <= j && i >= n * 0.5)
                        {
                            dataGridView2.Rows[n - j].Cells[i].Value = 0;
                            dataGridView2.Rows[n - j].Cells[i].Style.BackColor = Color.Green;
                        }
                        else
                            dataGridView2.Rows[n - j].Cells[i].Value = tmain.ftest(i * h, j * k);
                    }
                label22.Text = ""; label23.Text = "В таблице показаны точные значения тестовой задачи";
                label11.Text = ""; label27.Text = ""; label29.Text = ""; label31.Text = "";
                pictureBox4.Visible = false;
            }
            else if (comboBox5.SelectedIndex == 1)
            {
                double omega = Convert.ToDouble(textBox7.Text);
                if (checkBox2.Checked)
                {
                    omega = tmain.OptimumOmega();
                    textBox7.Text = Convert.ToString(omega);
                }
                tmain.Relaxation(omega);
                dataGridView2.Rows.Clear();
                dataGridView2.RowCount = n + 1;
                dataGridView2.ColumnCount = m + 1;
                for (int j = 0; j < m + 1; j++)
                    for (int i = 0; i < n + 1; i++)
                    {
                        if ((m * 0.25 < j && j < 3 * m * 0.25) && (n * 0.25 < i && i < 3 * n * 0.25))
                            dataGridView2.Rows[n - j].Cells[i].Style.BackColor = Color.Green;
                        else if ((m * 0.5 < j && j < 3 * m * 0.25) && (3 * n * 0.25 <= i))
                            dataGridView2.Rows[n - j].Cells[i].Style.BackColor = Color.Green;
                        else if (3 * m * 0.25 <= j && i > n * 0.5)
                            dataGridView2.Rows[n - j].Cells[i].Style.BackColor = Color.Green;
                        dataGridView2.Rows[n - j].Cells[i].Value = tmain.V[j, i];
                    }
                label23.Text = "Для решения потребовалось N = " + Convert.ToString(tmain.N) + " итераций";
                label27.Text = " = " + Convert.ToString(tmain.GetEpsMax());
                label22.Text = "Достигнутая погрешность итерационного метода eps = " + Convert.ToString(tmain.eps);
                label29.Text = " в точке x = " + Convert.ToString(tmain.xmax) + " y = " + Convert.ToString(tmain.ymax);
                label31.Text = "Невязка по норме бесконечность: " + Convert.ToString(tmain.GetResidual());
                pictureBox4.Visible = true;
            }
            else
            {
                double omega = Convert.ToDouble(textBox7.Text);
                if (checkBox2.Checked)
                {
                    omega = tmain.OptimumOmega();
                    textBox7.Text = Convert.ToString(omega);
                }
                tmain.Relaxation(omega);
                dataGridView2.Rows.Clear();
                dataGridView2.RowCount = n + 1;
                dataGridView2.ColumnCount = m + 1;
                for (int j = 0; j < m + 1; j++)
                    for (int i = 0; i < n + 1; i++)
                    {
                        if ((m * 0.25 < j && j < 3 * m * 0.25) && (n * 0.25 < i && i < 3 * n * 0.25))
                            dataGridView2.Rows[n - j].Cells[i].Style.BackColor = Color.Green;
                        else if ((m * 0.5 < j && j < 3 * m * 0.25) && (3 * n * 0.25 <= i))
                            dataGridView2.Rows[n - j].Cells[i].Style.BackColor = Color.Green;
                        else if (3 * m * 0.25 <= j && i > n * 0.5)
                            dataGridView2.Rows[n - j].Cells[i].Style.BackColor = Color.Green;
                        else dataGridView2.Rows[n - j].Cells[i].Value = Math.Abs(tmain.V[j, i] - tmain.ftest(i * h, j * k));
                    }
                label23.Text = "Для решения потребовалось N = " + Convert.ToString(tmain.N) + " итераций";
                label27.Text = " = " + Convert.ToString(tmain.GetEpsMax());
                label22.Text = "Достигнутая погрешность итерационного метода eps = " + Convert.ToString(tmain.eps);
                label29.Text = " в точке x = " + Convert.ToString(tmain.xmax) + " y = " + Convert.ToString(tmain.ymax);
                label31.Text = "Невязка по норме бесконечность: " + Convert.ToString(tmain.GetResidual());
                pictureBox4.Visible = true;
            }

        }
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}