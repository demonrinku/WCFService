using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace PerformanceCounterExample
{
    class Program
    {
        static void Main()
        {
            string customFolderPath = "E:\\Performance Counters C#"; //location of folder here

            string logFolderPath = Path.Combine(customFolderPath, "PerformanceLogs2");
            Directory.CreateDirectory(logFolderPath);

            string serviceCategoryName = "ServiceModelService 4.0.0.0";
            string serviceInstanceName = "ZAQ@http:||localhost:8333|";
            string[] serviceCounters = {
                "Calls",
                "Calls Per Second",
                "Calls Outstanding",
                "Calls Duration",
                "Percent of Max Concurrent Calls"
            };

            string locksAndThreadsCategoryName = ".NET CLR LocksAndThreads";
            string locksAndThreadsInstanceName = "ZAQConsoleClient";
            string[] locksAndThreadsCounters = {
                "# of current logical Threads",
                "# of current physical Threads",
                "# of current recognized threads",
                "Current Queue Length",
                "Queue Length / sec",
                "Queue Length Peak"
            };

            string logFilePath = Path.Combine(logFolderPath, "PerformanceLog.csv");

            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, false))
                {
                    writer.WriteLine("Timestamp,Calls,Calls Per Second,Calls Outstanding,Calls Duration,Percent of Max Concurrent Calls,# of current logical Threads,# of current physical Threads,# of current recognized threads,Current Queue Length,Queue Length / sec,Queue Length Peak");

                    for (int i = 0; i < 3600; i++)
                    {
                        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        writer.Write($"{timestamp},");

                        LogPerformanceCounters(writer, serviceCategoryName, serviceInstanceName, serviceCounters, timestamp);
                        LogPerformanceCounters(writer, locksAndThreadsCategoryName, locksAndThreadsInstanceName, locksAndThreadsCounters, timestamp);
                        writer.WriteLine();

                        Thread.Sleep(1000); // Capture values every second
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        static void LogPerformanceCounters(StreamWriter writer, string categoryName, string instanceName, string[] counters, string timestamp)
        {
            try
            {
                PerformanceCounterCategory category = new PerformanceCounterCategory(categoryName);
                var CountersValueList = new List<object>();

                if (category.InstanceExists(instanceName))
                {
                    foreach (var counterName in counters)
                    {
                        PerformanceCounter counterval = new PerformanceCounter(categoryName, counterName, instanceName);
                        var counterValue = counterval.NextValue();
                        CountersValueList.Add(counterValue);
                        //writer.WriteLine($"{timestamp},{categoryName},{instanceName},{counterName},{counterValue}");
                        //writer.WriteLine("Extra");
                    }
                    foreach (var counterValue in CountersValueList)
                    {
                        writer.Write($"{counterValue},");
                    }

                }
                else
                {
                    writer.WriteLine($"{timestamp},{categoryName},{instanceName},Instance not found,0");
                }
            }
            catch (Exception ex)
            {
                writer.WriteLine($"{timestamp},{categoryName},{instanceName},Error: {ex.Message},0");
            }
        }
    }
}
