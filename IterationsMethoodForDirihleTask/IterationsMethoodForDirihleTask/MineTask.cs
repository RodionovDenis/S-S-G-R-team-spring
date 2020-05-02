using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IterationsMethoodForDirihleTask
{
    class MineTask
    {
        //границы прямоугольника
        private double a;
        private double b;
        private double c;
        private double d;

        private int n; //разбиений по Х
        private int m; //разбиений по У

        private double h; //шаг по X 
        private double k; //шаг по У

        private int Nmax; //максимальное количество шагов
        private double Epsmax; //точность итерационного метода

        public double[,] V; // для решения

        public int N = 0; //количество проведенных шагов
        public double eps = 0; //погрешность метода 

        public MineTask(double a_, double b_, double c_, double d_,
            int n_, int m_, int Nmax_, double Epsmax_, double[,] V_)
        {
            a = a_; b = b_; c = c_; d = d_;
            n = n_; m = m_; Nmax = Nmax_; Epsmax = Epsmax_;

            V = V_; //прямоугольник

            h = (b - a) / (double)n;
            k = (d - c) / (double)m;
        }
        public double m1(double y)
        {
            return 1;
        }
        public double m2(double y)
        {
            return Math.Exp(Math.Sin(Math.PI * y) * Math.Sin(Math.PI * y));
        }
        public double m3(double x)
        {
            return 1;
        }
        public double m4(double x)
        {
            return Math.Exp(Math.Sin(Math.PI * x * 2) * Math.Sin(Math.PI * x * 2));
        }
        public double m5(double y) 
        {
            return Math.Exp(Math.Sin(Math.PI * (a+n *0.5*h) * y) * Math.Sin(Math.PI * (a + n * 0.5*h) * y));
        }
        public double m6(double x)
        {
            return Math.Exp(Math.Sin(Math.PI * x *(c+ 3 * m * 0.25*k)) * Math.Sin(Math.PI * x * (c+3 * m * 0.25*k)));
        }
        public double m7(double y)
        {
            return Math.Exp(Math.Sin(Math.PI * (a+n * 0.25*h) * y) * Math.Sin(Math.PI * (a+n * 0.25*h) * y));
        }
        public double m8(double x)
        {
            return Math.Exp(Math.Sin(Math.PI * x* (c+ m * 0.25*k)) * Math.Sin(Math.PI * x * (c+m * 0.25*k)));
        }
        public double m9(double y)
        {
            return Math.Exp(Math.Sin(Math.PI * (a+ 3 * n * 0.25*h) * y) * Math.Sin(Math.PI * (a+3 * n * 0.25*h) * y));
        }
        public double m10(double x)
        {
            return Math.Exp(Math.Sin(Math.PI * x * (c + m * 0.5 * k)) * Math.Sin(Math.PI * x * (c + m * 0.5 * k)));
        }
        public double f(double x, double y)
        {
            return 1.0 / 2.0 * Math.PI * Math.PI * (x * x + y * y) * Math.Exp(Math.Sin(Math.PI * x * y) * Math.Sin(Math.PI * x * y)) * (-4 * Math.Cos(2 * Math.PI * x * y) + Math.Cos(4 * Math.PI * x * y) - 1);
        }
        public double ftest(double x, double y)
        {
            return Math.Exp(Math.Sin(Math.PI * x * y) * Math.Sin(Math.PI * x * y));
        }
        private void BorderConditions() //граничные условия
        {
            for(int j = 0; j < m+1; j++)
                for(int i = 0; i < n+1; i++)
                {
                    if (j == 0)
                        V[j, i] = m3(a + i * h);
                    else if (j == m * 0.25 && (n * 0.25 <= i && i <= 3 * n * 0.25))
                        V[j, i] = m8(a + i * h);
                    else if (j == 3 * m * 0.25 && (n * 0.25 <= i && i <= n * 0.5))
                        V[j, i] = m6(a + i * h);
                    else if (j == 0.5 * m && 3 * n * 0.25 <= i)
                        V[j, i] = m10(a + i * h);
                    else if (j == m && i <= n * 0.5)
                        V[j, i] = m4(a + i * h);
                    else if (i == 0)
                        V[j, i] = m1(c + j * k);
                    else if (i == n * 0.25 && (m * 0.25 <= j && j <= 3 * m * 0.25))
                        V[j, i] = m7(c + j * k);
                    else if (i == n * 0.5 && 3 * m * 0.25 <= j)
                        V[j, i] = m5(c + j * k);
                    else if (i == 3 * n * 0.25 && (m * 0.25 <= j && j <= m * 0.5))
                        V[j, i] = m9(c + j * k);
                    else if (i == n && j <= m * 0.5)
                        V[j, i] = m2(c + j * k);
                    else V[j, i] = 0;
                }
        }
        public void Relaxation(double omega)
        {
            BorderConditions();

            double ai = (1.0 / (h * h));
            double bi = (1.0 / (k * k));
            double ci = -2.0 * (ai + bi);

            bool flag = false;

            do
            {
                N++;
                eps = 0;
                for (int i = 1; i < m; i++)
                {
                    for (int j = 1; j < n; j++)
                    {
                        if ((m * 0.25 <= j && j <= 3 * m * 0.25) && (n * 0.25 <= i && i <= 3 * n * 0.25))
                            continue;
                        else if ((m * 0.5 <= j && j <= 3 * m * 0.25) && (3 * n * 0.25 <= i))
                            continue;
                        else if (3 * m * 0.25 <= j && i >= n * 0.5)
                            continue;
                        double u_old = V[i, j];
                        double u_new = -omega * (bi * (V[i + 1, j] + V[i - 1, j]) + ai * (V[i, j + 1] + V[i, j - 1]));
                        u_new = u_new + (1 - omega) * ci * V[i, j] - omega * f(a + h * j, c + k * i);
                        u_new = u_new / ci;
                        double eps_cur = Math.Abs(u_old - u_new);
                        if (eps_cur > eps)
                        {
                            eps = eps_cur;
                        }
                        V[i, j] = u_new;
                    }
                }
                if ((eps < Epsmax) || (N >= Nmax))
                {
                    flag = true;
                }
            } while (!flag);
        }
        public double OptimumOmega()
        {
            return 2.0 / (1.0 + 2 * Math.Abs(Math.Sin(Math.PI * h / 2.0)));
        }
        public double GetEpsMax()
        {
            double errormax = 0;
            for (int j = 0; j < m+1; j++)
                for (int i = 0; i < n+1; i++)
                {
                    if ((m * 0.25 <= j && j <= 3 * m * 0.25) && (n * 0.25 <= i && i <= 3 * n * 0.25))
                        continue;
                    else if ((m * 0.5 <= j && j <= 3 * m * 0.25) && (3 * n * 0.25 <= i))
                        continue;
                    else if (3 * m * 0.25 <= j && i >= n * 0.5)
                        continue;
                    double value = Math.Abs(this.V[j, i] - ftest(a + i * h, c + j * k));
                    if (value > errormax)
                        errormax = value;
                }
            return errormax;
        }

    }
}
