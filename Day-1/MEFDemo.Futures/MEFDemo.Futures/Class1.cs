using MEFDemo.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MEFDemo.Futures
{
    [Export(typeof(IDataDestination))]
    public class XmlDestination : IDataDestination
    {
        public void Write(IEnumerable<Product> list)
        {
            new XElement("Products",
                list.Select(p => new XElement("Product"
                    , new XAttribute("Id", p.Id)
                    , new XElement("Name", p.Name)
                    , new XElement("Cost", p.Cost)
                    , new XElement("Units", p.Units)))
            ).Save("products.xml");
        }
    }
}
