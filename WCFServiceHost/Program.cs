using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace WCFServiceHost
{
    class Program
    {
        static void Main()
        {
            using(ServiceHost host= new ServiceHost(typeof(WCFService.ZAQ))) {
                host.Open();

                Console.WriteLine("The service is ready at " + DateTime.Now.ToString());
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                // Close the service
                host.Close();
            }
        }
    }
}
