using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var transformer = new DataTransformer();
            transformer.Source = new InMemorySource();
            transformer.Destination = new CSVDestination();
            transformer.Transform();
            Console.WriteLine("Done");
        }

        public class DataTransformer {
            public IDataSource Source { get; set; }
            public IDataDestination Destination { get; set; }
            public void Transform()
            {
                var data = Source.GetData();
                Destination.Write(data);
            }
        }

        public class Product{
            public int Id { get; set; }

            public string Name { get; set; }

            public int Cost { get; set; }

            public int Units { get; set; }
            public override string ToString()
            {
                return string.Format("{0},{1},{2},{3}", this.Id, this.Name, this.Cost, this.Units);
            }
        }

        public interface IDataSource{
            IEnumerable<Product> GetData();
        }

        public interface IDataDestination {
            void Write(IEnumerable<Product> list);
        }

        public class InMemorySource : IDataSource
        {
            public IEnumerable<Product> GetData()
            {
                return new[]{
                    new Product{Id = 11, Name = "pen", Cost = 30, Units = 10},
                    new Product{Id = 18, Name = "hen", Cost = 80, Units = 70},
                    new Product{Id = 15, Name = "ten", Cost = 50, Units = 30},
                    new Product{Id = 17, Name = "den", Cost = 33, Units = 60},
                    new Product{Id = 13, Name = "len", Cost = 45, Units = 20},
                };
            }
        }

        public class CSVDestination : IDataDestination {
            public void Write(IEnumerable<Product> list)
            {
                var sw = new StreamWriter("products.csv");
                foreach(var p in list)
                    sw.WriteLine(p.ToString());
                sw.Close();
            }

        }


    }
}
