using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAN_XXXIII_Dejan_Prodanovic
{
    class Program
    {
        //delegate for functions that will be forwarded as parametars to threads
        public delegate void ThredMethod();

        static int[,] matrix = new int[100, 100];
        //time of execution of firs thread
        static long thread1Time;
        //time of execution of firs thread
        static long thread2Time;
        static void Main(string[] args)
        {
            
            for (int i = 0; i < 100; i++)
            {
                matrix[i, i] = 1;
            }

            //array of threads
            Thread[] threads = new Thread[4];
            ThredMethod[] methods = { ThreadMethod1, ThreadMethod2, ThreadMethod3, ThreadMethod4 };

            for (int i = 0; i < 4; i++)
            {
                threads[i] = new Thread(new ThreadStart(methods[i]));

                //we create thread name based on a thread index
                if (i % 2 == 0)
                {
                    threads[i].Name = String.Format("THREAD_{0}", i + 1);
                    Console.WriteLine("Kreiran je tred {0}", threads[i].Name);
                }
                else
                {
                    threads[i].Name = String.Format("THREAD_{0}{0}", i + 1, i + 1);
                    Console.WriteLine("Kreiran je tred {0}", threads[i].Name);

                }

            }
            threads[0].Start();
            threads[1].Start();

            //first two threads have to finish theri job before we start another two threads
            threads[0].Join();
            threads[1].Join();
            Console.WriteLine("\nTred {0} je zavrsio upis u fajl FileByThread_1.txt", threads[0].Name);
            Console.WriteLine("Tred {0} je zavrsio upis u fajl FileByThread_22.txt", threads[1].Name);

            Console.WriteLine("Ukupno vreme trajanja tredova {0} i {1} iznosi {2} ms\n\n",threads[0].Name,
                threads[1].Name, thread1Time+ thread1Time);
            threads[2].Start();
            threads[3].Start();

            Console.ReadLine();
        }


        /// <summary>
        /// method that executed by first thread
        /// it writes content of matrix in file FileByThread_1.txt
        /// </summary>
        static void ThreadMethod1()
        {
            StringBuilder rowOfMatrix = new StringBuilder();
            if (File.Exists("../../FileByThread_1.txt"))
            {
                System.IO.File.WriteAllText(@"../../FileByThread_1.txt", string.Empty);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            StreamWriter sw = File.AppendText("../../FileByThread_1.txt");
            for (int i = 0; i < 100; i++)
            {
                rowOfMatrix.Clear(); 
                for (int j = 0; j < 100; j++)
                {
                    if (i == j)
                    {
                        sw.Write(" "+ matrix[i,j] + " ");
                    }
                    else
                    {
                        if (j==0)
                        {
                            sw.Write(" " + matrix[i, j]);

                        }
                        else
                        {
                            sw.Write(matrix[i, j]);
                        }
                       
                    }
                    
                }
                sw.WriteLine();
              
            }
            sw.Close();
            watch.Stop();
            thread1Time = watch.ElapsedMilliseconds;
              
        }
        /// <summary>
        /// method executed by second thred
        /// it writes 1000 random odd numbers in file FileByThread_22.txt
        /// </summary>
        static void ThreadMethod2()
        {
            StringBuilder toFile = new StringBuilder();
            Random rnd = new Random();
            if (File.Exists("../../FileByThread_22.txt"))
            {
                System.IO.File.WriteAllText(@"../../FileByThread_22.txt", string.Empty);
            }

            var watch = System.Diagnostics.Stopwatch.StartNew();
            StreamWriter sw = File.AppendText("../../FileByThread_22.txt");
            for (int i = 0; i < 1000; i++)
            {
             int randomOddNumber = GenerateOddRandomNumber(0, 10000, rnd);
                
             sw.WriteLine(randomOddNumber);
                 
            }
            sw.Close();
            watch.Stop();
            thread2Time = watch.ElapsedMilliseconds;

        }

        /// <summary>
        /// method executed by third thread 
        /// it reads matrix from file FileByThread_1.txt and prints it in console
        /// </summary>
        static void ThreadMethod3()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            using (StreamReader sr = new StreamReader("../../FileByThread_1.txt"))
            {
                string line;
                
                while ((line = sr.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
            watch.Stop();


        }
        /// <summary>
        /// method executed by fourth thread 
        /// it reads numbers from file FileByThread_22.txt, calculates their sum
        /// and prints it in a console
        /// </summary>
        static void ThreadMethod4()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            int sum = 0;
            using (StreamReader sr = new StreamReader("../../FileByThread_22.txt"))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                      sum  += Int32.Parse(line);
                }
            }
            watch.Stop();
             Console.WriteLine("\nSuma brojeva iz fajla FileByThread_22.txt iznosi: {0}\n", sum);
        }

        private static int GenerateOddRandomNumber(int min, int max,Random random)
        {
            
            int ans = random.Next(min, max);
            if (ans % 2 != 0) return ans;
            else
            {
                if (ans + 1 <= max)
                    return ans + 1;
                else if (ans - 1 >= min)
                    return ans - 1;
                else return 1;
            }
        }

        
    }
}
