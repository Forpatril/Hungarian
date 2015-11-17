using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hungarian
{
    static class Hungarian
    {
        private struct item
        {
            public int i;
            public int j;
        }

        private static void C_out(int[,] c, int[] sz, int[] sm, bool[] mc, bool[] mr)
        {
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {
                    if (mc[j])
                        Console.ForegroundColor = ConsoleColor.Red;
                    if (mr[i])
                        Console.ForegroundColor = ConsoleColor.Green;
                    if (sz[j] == i)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    if (sm[j] == i)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("{0,3:N0}", c[i, j]);
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine();
            }
            Console.ReadKey();
            Console.WriteLine();
        }

        public static int[] Assign(int[,] c, bool t_out)
        {
            int[] siz;
            int a = c.GetLength(0);
            int t = c.GetLength(1);
            bool[] marked_c = new bool[t];
            bool[] marked_r = new bool[a];
            int[] sm = new int[t];
            item[] l_chain = new item[a * t];
            item z;
            siz = new int[t];
            for (int i = 0; i < t; i++)
            {
                siz[i] = -1;
                sm[i] = -1;
                l_chain[i].i = -1;
                l_chain[i].j = -1;
                marked_c[i] = false;
                marked_r[i] = false;
            }
            if (t_out)
            {
                Console.WriteLine("Начальная матрица");
                C_out(c, siz, sm, marked_c, marked_r);
            }
            int k = Prep(c, siz);
            if (t_out)
            {
                Console.WriteLine("Начальная СНН");
                C_out(c, siz, sm, marked_c, marked_r);
            }
            bool rc;
            while (k < t)
            {
                for (int i = 0; i < t; i++)
                {
                    if (siz[i] != -1)
                        marked_c[i] = true;
                }
                if (t_out)
                {
                    Console.WriteLine("Выделение столбцов");
                    C_out(c, siz, sm, marked_c, marked_r);
                }
                do
                {
                    z = FindZero(c, marked_c, marked_r);
                    if ((z.i == -1) && (z.j == -1))
                    {
                        z = MakeZero(c, marked_c, marked_r);
                    }
                    sm[z.j] = z.i;
                    if (t_out)
                    {
                        Console.WriteLine("Выделение найденного нулевого элемента");
                        C_out(c, siz, sm, marked_c, marked_r);
                    }
                    rc = RowContained(siz, z.i);
                    if (rc)
                    {
                        marked_r[z.i] = true;
                        marked_c[GetCol(siz, z.i)] = false;
                        if (t_out)
                        {
                            Console.WriteLine("Выделение строк");
                            C_out(c, siz, sm, marked_c, marked_r);
                        }
                    }
                    else
                    {
                        int l = 0;
                        l_chain[l].i = z.i;
                        l_chain[l++].j = z.j;
                        while (siz[l_chain[l - 1].j] != -1)
                        {
                            l_chain[l].i = siz[l_chain[l - 1].j];
                            l_chain[l].j = l_chain[l - 1].j;
                            l++;
                            l_chain[l].i = l_chain[l - 1].i;
                            l_chain[l].j = GetCol(sm, l_chain[l - 1].i);
                            l++;
                        }
                        for (int i = 1; i < l; i += 2)
                        {
                            siz[l_chain[i].j] = -1;
                        }
                        for (int i = 0; i < l; i += 2)
                        {
                            siz[l_chain[i].j] = l_chain[i].i;
                        }
                        k = 0;
                        for (int i = 0; i < siz.Length; i++)
                        {
                            if (siz[i] != -1)
                            {
                                k++;
                            }
                            sm[i] = -1;
                        }
                        for (int i = 0; i < marked_c.Length; i++)
                        {
                            marked_c[i] = false;
                            marked_r[i] = false;
                        }
                        if (t_out)
                        {
                            Console.WriteLine("Замена в L-цепочке. Построение новой СНН");
                            C_out(c, siz, sm, marked_c, marked_r);
                        }
                    }
                } while (rc);
            }
            return siz;
        }

        private static int GetCol(int[] siz, int row)
        {
            int c = new int();
            for (int i = 0; i < siz.Length; i++)
            {
                if (siz[i] == row)
                {
                    c = i;
                }
            }
            return c;
        }

        private static item MakeZero(int[,] c, bool[] marked_c, bool[] marked_r)
        {
            int h = int.MaxValue;
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {
                    if ((!marked_c[j]) && (!marked_r[i]) && (c[i, j] < h))
                    {
                        h = c[i, j];
                    }
                }
            }
            for (int i = 0; i < c.GetLength(0); i++)
            {
                if (!marked_r[i])
                    for (int j = 0; j < c.GetLength(1); j++)
                    {
                        c[i, j] -= h;
                    }
            }
            for (int j = 0; j < c.GetLength(0); j++)
            {
                if (marked_c[j])
                    for (int i = 0; i < c.GetLength(1); i++)
                    {
                        c[i, j] += h;
                    }
            }
            return FindZero(c, marked_c, marked_r);
        }

        private static item FindZero(int[,] c, bool[] marked_c, bool[] marked_r)
        {
            item z = new item();
            z.i = -1;
            z.j = -1;
            for (int i = 0; i < c.GetLength(0) && z.i == -1; i++)
                for (int j = 0; j < c.GetLength(1) && z.i == -1; j++)
                {
                    if ((!marked_c[j]) && (!marked_r[i]) && (c[i, j] == 0))
                    {
                        z.i = i;
                        z.j = j;
                    }
                }
            return z;
        }

        private static int Min(int[] m)
        {
            int min = int.MaxValue;
            for (int i = 0; i < m.Length; i++)
            {
                if (m[i] < min)
                    min = m[i];
            }
            return min;
        }

        private static int[] GetColumn(int[,] c, int col_n)
        {
            int[] col = new int[c.GetLength(0)];
            for (int i = 0; i < c.GetLength(0); i++)
            {
                col[i] = c[i, col_n];
            }
            return col;
        }

        private static int[] GetRow(int[,] c, int row_n)
        {
            int[] row = new int[c.GetLength(1)];
            for (int i = 0; i < c.GetLength(1); i++)
            {
                row[i] = c[row_n, i];
            }
            return row;
        }

        private static bool RowContained(int[] siz, int r)
        {
            bool c = false;
            foreach (int i in siz)
                if (i == r)
                    c = true;
            return c;
        }

        private static int Prep(int[,] c, int[] siz)
        {
            int k = 0;
            for (int j = 0; j < c.GetLength(1); j++)
            {
                int min = Min(GetColumn(c, j));
                for (int i = 0; i < c.GetLength(0); i++)
                {
                    c[i, j] -= min;
                }
                //c_out(c);
            }

            for (int i = 0; i < c.GetLength(0); i++)
            {
                int min = Min(GetRow(c, i));
                for (int j = 0; j < c.GetLength(1); j++)
                {
                    c[i, j] -= min;
                }
            }
            for (int j = 0; j < c.GetLength(1); j++)
            {
                for (int i = 0; i < c.GetLength(0); i++)
                {
                    if ((c[i, j] == 0) && (!RowContained(siz, i)) && (siz[j] == -1))
                    {
                        siz[j] = i;
                        k++;
                    }
                }
            }
            return k;
        }
    }
}
