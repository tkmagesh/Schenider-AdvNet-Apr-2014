using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFDemoClient.ServiceProxies;

namespace WCFDemoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculator = new CalculatorClient();
            Console.WriteLine(calculator.Add(100,200));
            Console.WriteLine(calculator.Sutract(100,200));
            Console.WriteLine(calculator.Multiply(100,200));
            Console.WriteLine(calculator.Divide(100,200));
            Console.ReadLine();
        }
    }
}
