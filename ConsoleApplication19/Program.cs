using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication19
{
    class Program
    {
        static int score;

        void space_remover_backwards(int[] a)
        {
            int j;
            for (int i = 0; i < 3; i++)
            {
                if (a[i] == 0)
                {
                    for (j = i + 1; j < 3; j++)
                    {
                        if (a[j] != 0)
                        {
                            break;
                        }
                    }
                    a[i] = a[j];
                    a[j] = 0;
                }
            }
        }

        void space_remover_forwards(int[] a)
        {
            int j;
            for (int i = 3; i > 0; i--)
            {
                if (a[i] == 0)
                {
                    for (j = i - 1; j > 0; j--)
                    {
                        if (a[j] != 0)
                            break;
                    }
                    a[i] = a[j];
                    a[j] = 0;
                }
            }
        }

        void merger_forwards(int[] a)
        {
            space_remover_forwards(a);
            for (int i = 3; i > 0; i--)
            {
                if (a[i] == a[i - 1])
                {
                    a[i] += a[i - 1];
                    a[i - 1] = 0;
                    score += a[i];
                    space_remover_forwards(a);
                }
            }
            space_remover_forwards(a);
        }

        void merger_backwards(int[] a)
        {
            space_remover_backwards(a);
            for (int i = 0; i < 3; i++)
            {
                if (a[i] == a[i + 1])
                {
                    a[i] += a[i + 1];
                    a[i + 1] = 0;
                    score += a[i];
                    space_remover_backwards(a);
                }
            }
            space_remover_backwards(a);
        }

        int movement_commander(int[,] a)
        {
            int[] b = new int[4];
            int flag=0;
            char movement;
            do
            {
                movement = Console.ReadKey(true).KeyChar;
            } while (!(movement == 'w' || movement == 'W' || movement == 'D' || movement == 'd' || movement == 'A' || movement == 'a' || movement == 'S' || movement == 's'));
            if (movement == 'w' || movement == 'W')
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        b[j] = a[j, i];
                    }
                    new Program().merger_backwards(b);
                    for (int j = 0; j < 4; j++)
                    {
                        if(a[j,i]!=b[j])
                        {
                            a[j, i] = b[j];
                            flag=1;
                        }
                    }
                }
            }
            if (movement == 'S' || movement == 's')
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        b[j] = a[j, i];
                    }
                    new Program().merger_forwards(b);
                    for (int j = 0; j < 4; j++)
                    {
                        if(a[j,i]!=b[j])
                        {
                            a[j, i] = b[j];
                            flag=1;
                        }
                    }
                }
            }
            if (movement == 'A' || movement == 'a')
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        b[j] = a[i, j];
                    }
                    new Program().merger_backwards(b);
                    for (int j = 0; j < 4; j++)
                    {
                        if(a[i,j]!=b[j])
                        {
                            a[i,j] = b[j];
                            flag=1;
                        }
                    }
                }
            }
            if (movement == 'D' || movement == 'd')
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        b[j] = a[i,j];
                    }
                    new Program().merger_forwards(b);
                    for (int j = 0; j < 4; j++)
                    {
                        if(a[i,j]!=b[j])
                        {
                            a[i,j] = b[j];
                            flag=1;
                        }
                    }
                }
            }
            //for(int i=0;i<4;i++)
            //{
            //    for(int j=0;j<4;j++)
            //    {
            //        if(a[i,j]!=copy[i,j])
            //        {
            //            return 1;
            //        }
            //    }
            //}
            return flag;
        }

        void display_movements(int[,] a)
        {
            Console.WriteLine("  -----------------------------------------");
            Console.WriteLine(" |                                         |");
            Console.Write(" |");
            for (int i = 0; i < 4; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    if (a[i, k] == 0)
                    {
                        Console.Write("          ");
                    }
                    else
                    {
                        Console.Write("  ------- ");
                    }
                }
                Console.WriteLine(" |");
                Console.Write(" |");
                for (int k = 0; k < 4; k++)
                {
                    if (a[i, k] == 0)
                    {
                        Console.Write("          ");
                    }
                    else
                    {
                        Console.Write(" |       |");
                    }
                }
                Console.WriteLine(" |");
                Console.Write(" |");
                for (int k = 0; k < 4; k++)
                {
                    if (a[i, k] == 0)
                    {
                        Console.Write("          ");
                    }
                    else
                    {
                        Console.Write(" | {0,4}  |", a[i, k]);
                    }
                }
                Console.WriteLine(" |");
                Console.Write(" |");
                for (int k = 0; k < 4; k++)
                {
                    if (a[i, k] == 0)
                    {
                        Console.Write("          ");
                    }
                    else
                    {
                        Console.Write(" |       |");
                    }
                }
                Console.WriteLine(" |");
                Console.Write(" |");
                for (int k = 0; k < 4; k++)
                {
                    if (a[i, k] == 0)
                    {
                        Console.Write("          ");
                    }
                    else
                    {
                        Console.Write("  ------- ");
                    }
                }
                Console.WriteLine(" |");
                Console.Write(" |");
            }
            Console.WriteLine("                                         | ");
            Console.Write("  -----------------------------------------");
        }

        int is_turn_left(int [,] a)
        {
            for(int i=0;i<4;i++)
            {
                for(int j=0;j<4;j++)
                {
                    if(a[i,j]==0)
                    {
                        return 1;
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (a[i, j] == a[i, j + 1] || a[j, i] == a[j + 1, i])
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }

        void new_insert_position(int [,] a)
        {
            int b=0,c=0;
            Random intitializer = new Random();
            b = new Random().Next() % 4;
            for(int i=b;i<4;i++)
            {
                c = new Random().Next() % 4;
                for(int j=c;j<4;j++)
                {
                    if (a[i, j] == 0)
                    {
                        a[i, j] = new Random().Next() % 2 * 2 + 2;
                        goto x;
                    }
                }
                for(int j=0;j<c;j++)
                {
                    if (a[i, j] == 0)
                    {
                        a[i, j] = new Random().Next() % 2 * 2 + 2;
                        goto x;
                    }
                }
            }
            for (int i = 0; i < b; i++)
            {
                c = new Random().Next() % 4;
                for (int j = c; j < 4; j++)
                {
                    if (a[i, j] == 0)
                    {
                        a[i,j] = new Random().Next() % 2 * 2 + 2;
                        goto x;
                    }
                }
                for (int j = 0; j < c; j++)
                {
                    if (a[i, j] == 0)
                    {
                        a[i,j] = new Random().Next() % 2 * 2 + 2;
                        goto x;
                    }
                }
            }
           x:
                {
                
                }
        }

        void window_attributes()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetWindowSize(45,30);
            Console.Title = "2048 by KARTHIK M A M";
            Console.Clear();
            Console.Write("\n\n\tCONTROLS.....");
            Console.WriteLine("\n\n\n\n\n\t\t   [w]");
            Console.WriteLine("\t\t[A][S][D]");
            System.Threading.Thread.Sleep(5000);
        }

        static void Main(string[] args)
        {
            new Program().window_attributes();
            int x=0, y=0;
            int commander = 1;
            int counter = 0;
            int[,] a = new int[,] { {0,0,0,0} , {0,0,0,0} , {0,0,0,0} , {0,0,0,0} };
            x = new Random().Next() % 4;
            y = new Random().Next() % 4;
            a[x, y] = new Random().Next(0,1000) %2 * 2 + 2;
            x = x + 2;
            y = y + 2;
            if(x>3)
            {
                x = x - 3;
            }
            if(y>3)
            {
                y = y - 3;
            }
            a[x, y] = new Random().Next(1001,2000) % 2 * 2 + 2;
            Console.Clear();
            //Console.WriteLine("\n\n\n");
            Console.WriteLine("\n\n\n  (C) KARTHIK M A M         SCORE : {0,6}", score);
            Console.WriteLine("\n---------------------------------------------");
            new Program().display_movements(a);
            while((new Program().is_turn_left(a)==1))
            {
                commander = new Program().movement_commander(a);
                Console.Clear();
                //Console.WriteLine("\n\n\n\n\n\n\n");
                if (commander == 1)
                {
                    for (int i = 0; i < 4;i++ )
                    {
                        for(int j=0;j<4;j++)
                        {
                            if(a[i,j]==2048)
                            {
                                Console.Clear();
                                Console.WriteLine("\n\t   GAME OVER......YOU WON!!!");
                                Console.WriteLine("\n  (C) KARTHIK M A M         SCORE : {0,6}", score);
                                Console.WriteLine("\n---------------------------------------------");
                                new Program().display_movements(a);
                                System.Threading.Thread.Sleep(5000);
                                goto end;
                            }
                        }
                    }
                    Console.WriteLine("\n\n\n  (C) KARTHIK M A M         SCORE : {0,6}", score);
                    Console.WriteLine("\n---------------------------------------------");
                    new Program().display_movements(a);
                    System.Threading.Thread.Sleep(300);
                    Console.Clear();
                    new Program().new_insert_position(a);
                    counter++;
                }
                Console.WriteLine("\n\n\n  (C) KARTHIK M A M         SCORE : {0,6}", score);
                Console.WriteLine("\n---------------------------------------------");
                new Program().display_movements(a);
            }
            if (!(new Program().is_turn_left(a) == 1))
            {
                Console.Clear();
                Console.WriteLine("\n\t   GAME OVER........YOU LOST");
                Console.WriteLine("\n  (C) KARTHIK M A M         SCORE : {0,6}", score);
                Console.WriteLine("\n---------------------------------------------");
                new Program().display_movements(a);
                System.Threading.Thread.Sleep(5000);
            }
        end:
            {

            }
        }
    }
}
