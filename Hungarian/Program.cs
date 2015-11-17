using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hungarian
{
    class Program
    {
        static int Menu(bool t_pr)
        {
            Console.WriteLine("1.Ввод матрицы");
            Console.WriteLine("2.Использование варианта 13");
            Console.WriteLine("3.Решение задачи минимизации");
            Console.WriteLine("4.Решение задачи максимизации");
            if (t_pr)
            {
                Console.WriteLine("5.Выключить печать промежуточных результатов");
            }
            else
            {
                Console.WriteLine("5.Включить печать промежуточных результатов");
            }
            Console.WriteLine("6.Выход");
            Console.Write(">");
            return Convert.ToInt32(Console.ReadLine());
        }

        static void Input(int[,] c, int l)
        {
            for (int i = 0; i < l; i++)
            {
                string[] str = Console.ReadLine().Split(' ');
                if (str.Length == 1)
                {
                    string st = str[0];
                    str = Console.ReadLine().Split('\t');
                }
                for (int j = 0; j < l; j++)
                {
                    c[i, j] = Convert.ToInt32(str[j]);
                }
            }
        }

        static void InvertM(int[,] c, int max)
        {
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {
                    c[i, j] *= -1;
                    c[i, j] += max;
                }
            }
        }

        static int Max(int[,] c)
        {
            int max = int.MinValue;
            for (int i = 0; i < c.GetLength(0); i++)
                for (int j = 0; j < c.GetLength(1); j++)
                    if (c[i, j] > max)
                    {
                        max = c[i, j];
                    }
            return max;
        }

        static void Main(string[] args)
        {
            bool test = false, ent = false;
            int[,] m = { { 10, 4, 9, 8, 5 }, { 9, 3, 5, 7, 8 }, { 2, 5, 8, 10, 5 }, { 4, 5, 7, 9, 3 }, { 8, 7, 10, 9, 6 } };
            int[,] n = new int[5, 5], e = new int[5, 5];
            int act, f = 0, l = 0;
            do
            {
                act = Menu(test);
                switch (act)
                {
                    case 1:
                        if (ent)
                        {
                            Console.Write("Использовать предыдущую матрицу?(Y/N):");
                            switch (Console.ReadLine().ToUpper())
                            {
                                case "N":
                                    Console.Write("Введите размерность матрицы:");
                                    l = Convert.ToInt32(Console.ReadLine());
                                    e = new int[l, l];
                                    n = new int[l, l];
                                    Input(e, l);
                                    for (int i = 0; i < l; i++)
                                        for (int j = 0; j < l; j++)
                                            n[i, j] = e[i, j];
                                    break;
                                case "Y":
                                    for (int i = 0; i < l; i++)
                                        for (int j = 0; j < l; j++)
                                            n[i, j] = e[i, j];
                                    break;
                            }
                        }
                        else
                        {
                            ent = true;
                            Console.Write("Введите размерность матрицы:");
                            l = Convert.ToInt32(Console.ReadLine());
                            e = new int[l, l];
                            n = new int[l, l];
                            Input(e, l);
                            for (int i = 0; i < l; i++)
                                for (int j = 0; j < l; j++)
                                    n[i, j] = e[i, j];
                        }
                        break;
                    case 2:
                        ent = false;
                        n = new int[5,5];
                        for (int i = 0; i < 5; i++)
                            for (int j = 0; j < 5; j++)
                                n[i, j] = m[i, j];
                        break;
                    case 3:
                        int[] r = Hungarian.Assign(n, test);
                        for (int i = 0; i < r.Length; i++)
                        {
                         //   Console.Write("{0,3:N0}", r[i]+1);
                            if (ent)
                            {
                                f += e[r[i], i];
                            }
                            else
                            {
                                f += m[r[i], i];
                            }
                        }
                        for (int i = 0; i < r.Length; i++)
                        {
                            for (int j = 0; j < r.Length; j++)
                            {
                                if (r[j] == i)
                                {
                                    Console.Write(" 1");
                                }
                                else
                                {
                                    Console.Write(" 0");
                                }
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                        Console.WriteLine(f);
                        Console.WriteLine();
                        Console.ReadKey();
                        f = 0;
                        break;
                    case 4:
                        InvertM(n, Max(n));
                        r = Hungarian.Assign(n, test);
                        for (int i = 0; i < r.Length; i++)
                        {
                        //    Console.Write("{0,3:N0}", r[i]+1);
                            if (ent)
                            {
                                f += e[r[i], i];
                            }
                            else
                            {
                                f += m[r[i], i];
                            }
                        }
                        for (int i = 0; i < r.Length; i++)
                        {
                            for (int j = 0; j < r.Length; j++)
                            {
                                if (r[j] == i)
                                {
                                    Console.Write(" 1");
                                }
                                else
                                {
                                    Console.Write(" 0");
                                }
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                        Console.WriteLine(f);
                        Console.WriteLine();
                        Console.ReadKey();
                        f = 0;
                        break;
                    case 5: test = !test; break;
                }
            } while (act != 6);
        }
    }
}
