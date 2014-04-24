using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MEFDemo.Contracts;

namespace MEFDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var transformer = new DataTransformer();
            /*
            transformer.Source = new InMemorySource();
            transformer.Destination = new CSVDestination();
             */
            var assemblyCatalog = new AssemblyCatalog(Assembly.GetAssembly(typeof(Program)));
            var directoryCatalog = new DirectoryCatalog("transformers");
            var aggregateCatalog = new AggregateCatalog(assemblyCatalog, directoryCatalog);

            var container = new CompositionContainer(aggregateCatalog);
            container.ComposeParts(transformer);
            transformer.Transform();
            Console.WriteLine("Done");
            Console.ReadLine();
        }
               
    }

    public class DataTransformer
    {
        [Import(typeof(IDataSource))]
        public IDataSource Source { get; set; }

        [ImportMany(typeof(IDataDestination))]
        public IDataDestination[] Destinations { get; set; }
        public void Transform()
        {
            var data = Source.GetData();
            foreach(var destination in Destinations)
                destination.Write(data);
        }
    }

   

   

    [Export(typeof(IDataSource))]
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

    [Export(typeof(IDataDestination))]
    public class CSVDestination : IDataDestination
    {
        public void Write(IEnumerable<Product> list)
        {
            var sw = new StreamWriter("products.csv");
            foreach (var p in list)
                sw.WriteLine(p.ToString());
            sw.Close();
        }

    }

}
