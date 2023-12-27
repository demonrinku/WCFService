//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace ZAQConsoleClient
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            const int callsPerSecond = 2;
//            const int millisecondsDelay = 100 ;

//            while (true)
//            {
//                Console.WriteLine("while loop");
//                for (int i = 0; i < callsPerSecond; i++)
//                {
//                    Console.WriteLine("for loop");
//                    Task.Factory.StartNew(() =>
//                    {
//                        MakeServiceCall();
//                    });

//                    Thread.Sleep(millisecondsDelay);
//                }
//            }
//        }

//        static void MakeServiceCall()
//        {
//            Console.WriteLine("function loop");
//            try
//            {
//                using (ServiceReference1.ZAQClient client = new ServiceReference1.ZAQClient()) // Create your WCF client proxy
//                {
//                    var result = client.GetUser();
//                    Console.WriteLine($"Received data: {result.ToString()}");
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error: {ex.Message}");
//            }
//        }
//    }
//}


using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZAQConsoleClient
{
    class Program
    {
        static void Main()
        {
            // Run the loop infinitely
            while (true)
            {
                // Start 30 threads to make calls concurrently
                Parallel.For(0, 30, i =>
                {
                    Console.WriteLine(i);
                    Thread thread = new Thread(() =>
                    {
                        Console.WriteLine(1);
                        try
                        {
                            // Create a WCF client proxy
                            var client = new ServiceReference1.ZAQClient();

                            var result = client.GetUser();
                            // Printing the data received
                            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} - Data received: {result.Username}, {result.Password}, {result.Department}");
                            
                            // Close the client after all calls
                            client.Close();
                        }
                        catch (Exception ex)
                        {
                            // Handle exceptions
                            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} - Exception: {ex.Message}");
                        }
                    });
                    // Start the thread
                    thread.Start();
                });
                // Wait for a second before starting the next loop iteration
                Thread.Sleep(1000 / 30); // 1000 milliseconds / 30 times = 33.3 milliseconds per loop iteration
            }
        }
    }
}

