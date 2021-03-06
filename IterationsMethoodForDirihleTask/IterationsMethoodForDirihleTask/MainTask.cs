﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IterationsMethoodForDirihleTask
{
    class MainTask
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
        public double[,] r; //для невязки

        private double l1; //минимальное собственное число
        private double ln; //максимальное собственное число 

        public int N = 0; //количество проведенных шагов
        public double eps = 0; //погрешность метода 

        public double xmax = 0; //максимальная ошибка в точке Х
        public double ymax = 0; //максимальная ошибка в точке У

        public MainTask(double a_, double b_, double c_, double d_,
            int n_, int m_, int Nmax_, double Epsmax_, double [,] V_, double [,] r_)
        {
            a = a_; b = b_; c = c_; d = d_;
            n = n_; m = m_; Nmax = Nmax_; Epsmax = Epsmax_;

            V = V_; //прямоугольник
            r = r_; //невязка

            h = (b - a) / (double)n;
            k = (d - c) / (double)m;

            //собственные числа
            l1 = 4d / (h * h) * Math.Sin(Math.PI / (2d * n)) * Math.Sin(Math.PI / (2d * n)) +
                4d / (k * k) * Math.Sin(Math.PI / (2d * m)) * Math.Sin(Math.PI / (2d * m));
            ln = 4d / (h * h) * Math.Sin(Math.PI * (n - 1) / (2d * n)) * Math.Sin(Math.PI * (n - 1) / (2d * n)) +
                4d / (k * k) * Math.Sin(Math.PI * (m - 1) / (2d * m)) * Math.Sin(Math.PI * (m - 1) / (2d * m));
        }
        public double m1(double y)
        {
            return Math.Sin(Math.PI * y) * Math.Sin(Math.PI * y);
        }
        public double m2(double y)
        {
            return Math.Abs(Math.Exp(Math.Sin(Math.PI * y)) - 1);
        }
        public double m3(double x)
        {
            return x * (1 - x);
        }
        public double m4(double x)
        {
            return x * (1 - x) * Math.Exp(x);
        }
        public double f(double x, double y)
        {
            return Math.Abs(x - y);
        }
        private double ScalarMult(double [,] x, double [,] y)
        {
            int len1 = x.GetLength(0);
            int len2 = x.GetLength(1);
            double sum = 0;
            for (int i = 0; i < len1; i++)
                for (int j = 0; j < len2; j++)
                    sum += x[i, j] * y[i, j];
            return sum;
        }
        private double ReturnParametrs(double [,]rs)
        {
            double h2 = 1d / (h * h);
            double k2 = 1d / (k * k);
            double A = -2 * (h2 + k2);

            double[,] Ars = new double[m - 1, n - 1];

            Ars[0, 0] = A * rs[0, 0] + h2 * rs[0, 1] + k2 * rs[1, 0];//левый нижний граничный 
            Ars[0, n - 2] = A * rs[0, n - 2] + h2 * rs[0, n - 3] + k2 * rs[1, n - 2];//правый нижний граничный
            Ars[m - 2, 0] = A * rs[m - 2, 0] + h2 * rs[m - 2, 1] + k2 * rs[m - 3, 0];//левый верхний граничный
            Ars[m - 2, n - 2] = A * rs[m - 2, n - 2] + h2 * rs[m - 2, n - 3] + k2 * rs[m - 3, n - 2];//правый верхний граничный
            for (int i = 0; i < m - 1; i++)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    if (i != 0 && j != 0 && i != m - 2 && j != n - 2)//центр
                        Ars[i, j] = A * rs[i, j] + h2 * rs[i, j + 1] + h2 * rs[i, j - 1] + k2 * rs[i + 1, j] + k2 * rs[i - 1, j];
                    else if (i == 0 && j != n - 2 && j != 0)//по низу
                    {
                        Ars[i, j] = A * rs[i, j] + h2 * rs[i, j + 1] + h2 * rs[i, j - 1] + k2 * rs[i + 1, j];
                    }
                    else if (j == 0 && i != m - 2 && i != 0)//слева
                    {
                        Ars[i, j] = A * rs[i, j] + h2 * rs[i, j + 1] + k2 * rs[i + 1, j] + k2 * rs[i - 1, j];
                    }
                    else if (j == n - 2 && i != m - 2 && i != 0)//справа
                    {
                        Ars[i, j] = A * rs[i, j] + h2 * rs[i, j - 1] + k2 * rs[i + 1, j] + k2 * rs[i - 1, j];
                    }
                    else if (i == m - 2 && j != 0 && j != n - 2)//сверху
                    {
                        Ars[i, j] = A * rs[i, j] + h2 * rs[i, j + 1] + h2 * rs[i, j - 1] + k2 * rs[i - 1, j];
                    }
                }
            }
            return ScalarMult(Ars, rs) / ScalarMult(Ars, Ars);
        }
        private void BorderConditions() //граничные условия
        {

            for (int i = 0; i < n + 1; i++)
            {
                V[0, i] = m3(a + i * h);
            }
            for (int i = 0; i < n + 1; i++)
            {
                V[m, i] = m4(a + i * h);
            }
            for (int j = 0; j < m + 1; j++)
            {
                V[j, 0] = m1(c + j * k);
            }
            for (int j = 0; j < m + 1; j++)
            {
                V[j, n] = m2(c + j * k);
            }
            for (int j = 1; j < m; j++)
                for (int i = 1; i < n; i++)
                    V[j, i] = 0; //заполнение нулями, начальное приближение
        }
        private double ReturnParametrs(int param, int s)
        {
            double a = 1d / ((l1 + ln) / 2d + (ln - l1) / 2d * Math.Cos(Math.PI / (2d * param) * (1 + 2 * s)));
            return a;
        }
        public void ChebyshevMethood(int param)
        {
            BorderConditions();
            double[] ts = new double[param];
            for (int i = 0; i < param; i++)
                ts[i] = ReturnParametrs(param, i);

            double h2 =  1d / (h * h);
            double k2 =  1d / (k * k);
            double A = - 2 * (h2 + k2);

            double V_old; double error; 
            //итерация метода
            while (N < Nmax)
            {
                int p = 0;
                while (p < param) //одна итерация метода Чебышева
                {
                    N++;
                    double accuracy = 0;
                    for (int j = 1; j < m; j++)
                        for (int i = 1; i < n; i++)
                            r[j-1, i-1] = V[j, i] * A + h2 * (V[j, i + 1] + V[j, i - 1]) + k2 * (V[j + 1, i] + V[j - 1, i]) + f(a + i * h, c + j * k); //невязка на текущей итерации
                    for (int j = 1; j < m; j++)
                        for (int i = 1; i < n; i++)
                        {
                            V_old = V[j, i];
                            V[j, i] = V[j, i] + ts[p] * r[j-1, i-1]; //обновление компонент
                            error = Math.Abs(V_old - V[j, i]);
                            if(error > accuracy)
                                accuracy = error;
                        }
                    p++;
                    eps = accuracy;
                    if (eps <= Epsmax)
                        break;
                }
                if (eps <= Epsmax)
                    break;
            }
            //выход
        }
        public void MinMethood()
        {
            BorderConditions();

            double h2 = 1d / (h * h);
            double k2 = 1d / (k * k);
            double A = -2 * (h2 + k2);

            bool flag = false;
            do
            {
                N++;
                eps = 0;
                for (int i = 1; i < m; i++)//считаем невязку
                {
                    for (int j = 1; j < n; j++)
                    {
                        r[i - 1, j - 1] = A * V[i, j] + h2 * (V[i, j + 1] + V[i, j - 1]) + k2 * (V[i + 1, j] + V[i - 1, j]) + f(a + j * h, c + i * k);
                    }
                }
                double tau = ReturnParametrs(r);
                //расчет нового вектора 
                for (int i = 1; i < m; i++)
                    for (int j = 1; j < n; j++)
                    {
                        double v_old = V[i, j];
                        double v_new = V[i, j] - (tau * r[i - 1, j - 1]);
                        double eps_cur = Math.Abs(v_old - v_new);
                        if (eps_cur > eps)
                        {
                            eps = eps_cur;
                        }
                        V[i, j] = v_new;
                    }
                if ((eps < Epsmax) || (N >= Nmax))
                {
                    flag = true;
                }
            } while (!flag);
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
                eps= 0;
                for (int i = 1; i < m; i++)
                {
                    for (int j = 1; j < n; j++)
                    {
                        double u_old = V[i, j];
                        double u_new = -omega * (bi * (V[i + 1, j] + V[i - 1, j]) + ai * (V[i, j + 1] + V[i, j - 1]));
                        u_new = u_new + (1 - omega) * ci * V[i,j] - omega * f(h * j, k * i);
                        u_new = u_new / ci;
                        double eps_cur = Math.Abs(u_old - u_new);
                        if (eps_cur > eps)
                        {
                            eps = eps_cur;
                        }
                        V[i,j] = u_new; 
                    }
                }
                if ((eps < Epsmax) || (N >= Nmax))
                {
                    flag = true;
                }
            } while (!flag);
        }
        public double GetEpsMax(MainTask maintask)
        {
            double errormax = 0;
            for(int j = 1; j < m; j++)
                for(int i = 1; i< n; i++)
                {
                    double value = Math.Abs(this.V[j, i] - maintask.V[2 * j, 2 * i]);
                    if (value > errormax)
                    {
                        errormax = value;
                        xmax = a + i * h;
                        ymax = c + j * k;
                    }
                }
            return errormax;
        }
        public double OptimumOmega()
        {
            return 2.0 / (1.0 + 2 * Math.Abs(Math.Sin(Math.PI * h / 2.0)));
        }
        public double GetResidual()
        {
            double h2 = 1d / (h * h);
            double k2 = 1d / (k * k);
            double A = -2 * (h2 + k2);
            double rmax = 0; //максимальная невязка
            for (int j = 1; j < m; j++)
                for (int i = 1; i < n; i++)
                {
                    double accuracy = Math.Abs(A * V[j, i] + h2 * (V[j, i - 1] + V[j, i + 1]) + k2 * (V[j - 1, i] + V[j + 1, i]) + f(a + i * h, c + j * k));
                    if (accuracy > rmax)
                        rmax = accuracy;
                }
            return rmax;
        }
    }
}