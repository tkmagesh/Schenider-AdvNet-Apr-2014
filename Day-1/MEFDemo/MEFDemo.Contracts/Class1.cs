using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFDemo.Contracts
{
    public interface IDataSource
    {
        IEnumerable<Product> GetData();
    }

    public interface IDataDestination
    {
        void Write(IEnumerable<Product> list);
    }

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Cost { get; set; }

        public int Units { get; set; }
        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", this.Id, this.Name, this.Cost, this.Units);
        }
    }
}
