using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFOrderProcessorClient.ServiceProxies;

namespace WCFOrderProcessorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            var service = new ServiceProxies.OrderProcessorClient();
            service.Process(new Order { ProductName = "pen", Units = 10, Cost = 50 });
            service.Process(new Order { ProductName = "ten", Units = 21, Cost = 20 });
            service.Process(new Order { ProductName = "len", Units = 16, Cost = 60 });
            service.Process(new Order { ProductName = "den", Units = 51, Cost = 30 });
            service.Process(new Order { ProductName = "hen", Units = 19, Cost = 40 });
            service.Process(new Order { ProductName = "zen", Units = 45, Cost = 10 });
            Console.WriteLine("Orders submitted for processing");
            Console.ReadLine();
        }
    }
}
