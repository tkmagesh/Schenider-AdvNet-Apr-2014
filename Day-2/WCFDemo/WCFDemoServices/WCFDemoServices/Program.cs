using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFDemoServices
{
    class Program
    {
        static void Main(string[] args)
        {
            //var host = new ServiceHost(typeof(CalculatorService), new Uri[]{});
            var host = new ServiceHost(typeof(OrderService), new Uri[] { });
            foreach(var endpoint in host.Description.Endpoints)
                Console.WriteLine("{0}, {1}, {2}", endpoint.Binding.Name, endpoint.Address.Uri, endpoint.Contract.Name);
            Console.WriteLine("Service running.. Press ENTER to shutdown..!");

            host.Open();
            Console.ReadLine();
            host.Close();
        }
    }

    [ServiceContract]
    public interface ICalculator {
        [OperationContract]
        int Add(int Number1, int Number2);
        
        [OperationContract]
        int Sutract(int Number1, int Number2);
        
        [OperationContract]
        int Multiply(int Number1, int Number2);
        
        [OperationContract]
        int Divide(int Number1, int Number2);

    }

    [ServiceBehavior]
    public class CalculatorService : ICalculator
    {
        [OperationBehavior]
        public int Add(int Number1, int Number2)
        {
            return Number1 + Number2;
        }

        [OperationBehavior]
        public int Sutract(int Number1, int Number2)
        {
            return Number1 - Number2;
        }

        [OperationBehavior]
        public int Multiply(int Number1, int Number2)
        {
            return Number1 * Number2;
        }

        [OperationBehavior]
        public int Divide(int Number1, int Number2)
        {
            return Number1 / Number2;
        }
    }

    [ServiceContract]
    public interface IOrderProcessor{

        [OperationContract(IsOneWay=true)]
        void Process(Order order);
    }

    [DataContract]
    public class Order {

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public int Units { get; set; }

        [DataMember]
        public decimal Cost { get; set; }

        [DataMember]
        public decimal OrderValue { get; set; }

        
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},{4}",Id,ProductName,Units,Cost,OrderValue);
        }
        
    }

    [ServiceBehavior]
    public class OrderService : IOrderProcessor {

        [OperationBehavior]
        public void Process(Order order)
        {
            order.Id = Guid.NewGuid().ToString();
            order.OrderValue = order.Units * order.Cost;
            Console.WriteLine(order);
        }
    }

}
