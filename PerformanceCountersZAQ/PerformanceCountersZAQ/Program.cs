using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;//added this package from nuget package manager to fetch the json data.

namespace PerformanceCounterExample
{
    class Program
    {
        static void Main()
        {
           //Application is starting.
            Console.WriteLine("Application has started at: " + DateTime.Now.ToString());
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------------------------------------------------------");
            Console.WriteLine();

            //Command to get the JSON data.
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

            var categories = configuration.GetSection("categories").GetChildren();

            //Datatype to store all the counter names and perfromance counters.
            List<string> CountersNames = new List<string>();
            List<PerformanceCounter> CounterValues = new List<PerformanceCounter>();

            foreach (var category in categories)
            {
                string CategoryName = category["CategoryName"];
                string InstanceName = category["InstanceName"];

                try
                {
                    //creating a new performancecountercategory class to get the data from perfrom tool.
                    PerformanceCounterCategory CategoryCheck = new PerformanceCounterCategory(CategoryName);

                    if (CategoryCheck.InstanceExists(InstanceName))
                    {
                        foreach (var categoryProperty in category.GetChildren())
                        {
                            foreach (var counter in categoryProperty.GetChildren())
                            {
                                CountersNames.Add(counter.Value);
                                PerformanceCounter counterValue = new PerformanceCounter(CategoryName, counter.Value, InstanceName);
                                CounterValues.Add(counterValue);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Application error - No instance is found with name: (" + InstanceName + ") in " + CategoryName+ ". Please check appsettings.json file to verify the instance name. ");
                        Console.ReadKey();
                        System.Environment.Exit(0);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Application error - " + ex.Message + " Please check appsettings.json file to verify the category. ");
                    Console.ReadKey();
                    System.Environment.Exit(0);
                }
            }

            //Defining the location of folder where log file is stored---it is taken from config file.
            string customFolderPath = configuration["FolderPath"];

            string logFolderPath = Path.Combine(customFolderPath, "PerformanceLogs");
            Directory.CreateDirectory(logFolderPath);

            Console.WriteLine("Location of Log file is:" + logFolderPath);
            Console.WriteLine();
            Console.WriteLine("---------------------------------------------------------------------------------------------------");
            Console.WriteLine();


            string format = "ddMMyyyy_hhmmsstt";
            string _filename = String.Format(DateTime.Now.ToString(format));

            //filename is unique for every execution that is in the format of datetime string.
            string filename = _filename + ".csv";
            string logFilePath = Path.Combine(logFolderPath, filename);

            try
            {
                //using streamwriter class to write the data in csv file.
                using (StreamWriter Writer = new StreamWriter(logFilePath, false))
                {

                    //the first row--
                    Writer.Write("TimeStamp,");

                    //writing all the counters in excel sheet as heading.
                    foreach(var countername in CountersNames)
                    {
                        Writer.Write(countername + ",");
                    }
                    Writer.WriteLine();//for entering the new line.
                    
                    //for subsequent rows--
                    while (true)
                    {
                        string Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        Writer.Write($"{Timestamp},");
                        Console.WriteLine("Data captured at: "+Timestamp);
                    
                        //getting all the data of performnace monitor like categoryname, instancename, counters from json file.
                        foreach(PerformanceCounter Countervalue in CounterValues)
                        {
                            var value = Countervalue.NextValue();   
                            Writer.Write(value + ",");
                        }
                        
                        Writer.WriteLine();
                        Console.WriteLine();
                        Thread.Sleep(5000);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
    }
}
