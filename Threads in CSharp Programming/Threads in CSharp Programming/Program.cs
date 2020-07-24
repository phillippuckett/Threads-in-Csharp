//Based on the YouTube tutorial provided by Derek Banas
//C# Tutorial 16 Threads https://www.youtube.com/watch?v=hOVSKuFTUiI

using System;
using System.Security.Cryptography.X509Certificates;
// 1 :  INSERT "using System.Threading;"//
using System.Threading;
using System.Threading.Channels;
using System.Transactions;

namespace Threads_in_CSharp_Programming
{
    class Program
    {
        static void Main(string[] args)
        {
            //2.0 : THREADS (SEE 2.1 ON LINE 92) //  
            //When ran using Par1() in conjunction with the following syntax will cause the Console to alternate between both the '0' and the '1' in the inout fields
            Thread a = new Thread(Par1);
            a.Start();

            for (int i = 0; i < 1000; i++)
            {
                Console.Write(0);
            }




            //3.0 : SLEEP)//
            //How to slow down a thread, in this example we are slowing it down by 1000ms
            int num = 1;
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(num);
                Thread.Sleep(1000);
                num++;
             }
            Console.WriteLine("Thread Ends");




            //4.1 : LOCK (SEE 4.0 ON LINE 106)//
            BankAcct acct = new BankAcct(10);
            Thread[] threadsArray = new Thread[15];
            Thread.CurrentThread.Name = "main";

            for (int i = 0; i < 15; i++)
            {
                //Create values to push into threadArray
                Thread b = new Thread(new ThreadStart(acct.IssuedWithdraw));
                b.Name = i.ToString();
                threadsArray[i] = b;
            }
            for (int i = 0; i < 15; i++)
            {
                //Chekc to see if the thread is alive yet -which it shouldn't be
                Console.WriteLine($"Thread {threadsArray[i].Name} Alive : {threadsArray[i].IsAlive}");
                
                //start the thread here
                threadsArray[i].Start();

                //Check again, this time it should be alive
                Console.WriteLine($"Thread {threadsArray[i].Name} Alive : {threadsArray[i].IsAlive}");
            }
            //Getting hte priority of a thread
            Console.WriteLine($"Current priority : {Thread.CurrentThread}");

            Console.WriteLine($"Thread {Thread.CurrentThread.Name} Ending");




            //5.0 : PASSING ARGS THROUGH A THREAD//
            Thread c = new Thread(() => CountTo(10));
            c.Start();

            new Thread(() => {CountTo(5); CountTo(6);}).Start();

            Console.ReadLine();
        }

        static void CountTo(int maxNum)
        {
            for (int i = 0; i < maxNum; i++)
            {
                Console.WriteLine(i);
            }
        }

        // 2.1 : THREADS (SEE 2.0 ON LINE 17)//
        static void Par1()
        {
            Thread a = new Thread(Par1);
            a.Start();

            for (int i = 0; i < 1000; i++)
            {
                Console.Write(1);
            }
        }
    }


    //4.0 : LOCK (SEE 4.1 ON LINE 44)//
    //To prevent users from taking out more money than what they have in their bank account
    class BankAcct
    {
        private Object acctLock = new object();
        double Balance { set; get; }

        public BankAcct(double bal)
        {
            Balance = bal;
        }

        public double Withdraw(double amt)
        {
            if (Balance - amt < 0)
            {
                Console.WriteLine($"Sorry ${Balance} in Account");
                return Balance;
            }
            lock (acctLock)
            {
                if (Balance >= amt)
                {
                    Console.WriteLine("Removed {0} and {1} left in Account", amt, (Balance - amt));
                    Balance -= amt;
                }
                return Balance;
            }
        }
        public void IssuedWithdraw()
        {
            Withdraw(1);
        }
    }
}

