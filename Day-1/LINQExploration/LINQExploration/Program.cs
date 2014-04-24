using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQExploration
{
    public class Employee
    {
        [DisplayName("Emp Id")]
        [IgnoreDisplay]
        public int EmpId { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {

            var e = new Employee { EmpId = 1, FirstName = "Magesh", LastName = "Kuppan" };
            //Console.WriteLine(MyUtils.Format(e));
            Console.WriteLine(e.Format());
            Console.WriteLine();
            var products = new Products();
            products.Add(new Product { Id = 101, Name = "pen", Cost = 90, Units = 10, Category=1 });
            products.Add(new Product { Id = 104, Name = "ten", Cost = 30, Units = 80, Category = 2 });
            products.Add(new Product { Id = 109, Name = "den", Cost = 70, Units = 20, Category = 1 });
            products.Add(new Product { Id = 103, Name = "len", Cost = 50, Units = 70, Category = 2 });
            products.Add(new Product { Id = 107, Name = "ken", Cost = 60, Units = 30, Category = 1 });
            products.Add(new Product { Id = 102, Name = "zen", Cost = 40, Units = 40, Category = 2 });

            
            Console.WriteLine("Initial list..");
            /*
             * for(var i=0;i<products.Count;i++)
                Console.WriteLine(products.GetByIndex(i).ToString());
             **/
            foreach(var product in products)
                Console.WriteLine(MyUtils.Format(product));

            Console.WriteLine("Costly product list..");
            //var costlyProductSpec = new CostlyProductSpecificaion(40);
            //var costlyProducts = products.Search(Program.isCheapProduct);

            /*
            var costlyProducts = products.Search(delegate(Product p)
            {
                return p.Cost <= 30;
            });
             */

            //Lambda Expressions
            /*
            var costlyProducts = products.Search((p) =>
            {
                return p.Cost <= 30;
            });
            */
            var costlyProducts = products.Search(p => p.Cost <= 30);


            foreach (var product in costlyProducts)
                    Console.WriteLine(MyUtils.Format(product));

            Console.WriteLine("Min product id is ");
            Console.WriteLine(products.Min(p => p.Id));
            Console.WriteLine();

            Console.WriteLine("Max product id is ");
            Console.WriteLine(products.Max(p => p.Id));
            Console.WriteLine();

            Console.WriteLine("Average product cost is ");
            Console.WriteLine(products.Average(p => p.Cost));
            Console.WriteLine();

            Console.WriteLine("Products Category...");
            var categorizedProducts = products.GroupBy(p => p.Category);
            foreach (var group in categorizedProducts)
            {
                Console.WriteLine("Category = {0}",group.Key);
                foreach(var item in group.Value)
                    Console.WriteLine(item.Format());
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        public static bool isCheapProduct(Product p)
        {
            return p.Cost <= 30;
        }
    }

    public interface IProductSpecification
    {
        bool IsSatisfiedBy(Product product);
    }

    public class CostlyProductSpecificaion : IProductSpecification
    {
        decimal _cost = 0;
        public CostlyProductSpecificaion(decimal cost)
        {
            _cost = cost;
        }
        public bool IsSatisfiedBy(Product product)
        {
            return product.Cost > _cost;
        }
    }

    //public delegate bool ProductSpecificationDelegate(Product product);


    public class Products: IEnumerable, IEnumerator, IEnumerable<Product>, IEnumerator<Product>
    {
        private ArrayList list = new ArrayList();
        public void Add(Product p) {
            //do data validations here
            list.Add(p);
        }
        public int Count
        {
            get
            {
                return list.Count;
            }
        }
        public void Remove(Product p){
            list.Remove(p);
        }

        public Product GetByIndex(int index) {
            return (Product)list[index];
        }

        public Products Search(IProductSpecification productSpec)
        {
            var result = new Products();
            foreach(var item in list){
                var product = (Product)item;
                if (productSpec.IsSatisfiedBy(product))
                    result.Add(product);
            }
            return result;
        }

        //public Products Search(ProductSpecificationDelegate productSpec)
        //public Products Search(Func<Product,bool> productSpec)
        /* Moving this below to MyUtils
        public IEnumerable Search(Predicate<Product> productSpec)
        {
            //var result = new Products();
            foreach (var item in list)
            {
                var product = (Product)item;
                if (productSpec(product))
                    //result.Add(product);
                    yield return product;
            }
            //return result;
        }
         */

        int index = -1;
        public IEnumerator GetEnumerator()
        {
            return this;
        }

        public object Current
        {
            get { return (Product)list[index]; }
        }

        public bool MoveNext()
        {
            index++;
            if (index >= list.Count)
            {
                Reset();
                return false;
            }
            else
            {
                return true;
            }
        }

        public void Reset()
        {
            index = -1;
        }




        Product IEnumerator<Product>.Current
        {
            get { return (Product)list[index] ; }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        IEnumerator<Product> IEnumerable<Product>.GetEnumerator()
        {
            return this;
        }
    }

    /*
    public delegate int IntFieldSelectorDelegate(Product p);
    public delegate decimal DecimalFieldSelector(Product p);
    */

    //public delegate TResult FieldSelector<T, TResult>(T t);

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int Units { get; set; }
        public int Category { get; set; }
        /*public override string ToString()
        {
            return string.Format("Id = {0}\tName = {1}\tCost = {2}\tUnits = {3}", this.Id, this.Name, this.Cost, this.Units);
        }*/
    }

    public static class MyUtils
    {
        public static string Format(this object o) {
            var t = o.GetType();
            var props = t.GetProperties();
            var result = string.Empty;
            foreach (var pInfo in props){
                if (pInfo.GetCustomAttributes(typeof(IgnoreDisplay),true).Length == 0){
                    var displayNameAttributes = pInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    var displayName = pInfo.Name;
                    if (displayNameAttributes.Length > 0){
                        displayName = ((DisplayNameAttribute) displayNameAttributes[0]).Name;
                    }
                    result += displayName + " = " + pInfo.GetValue(o, null) + "\t";
                }
            }
            return result;
        }

        public static int Min<T>(this IEnumerable list, Func<T, int> fieldSelector)
        {
            var result = int.MaxValue;
            foreach (var item in list)
            {
                var product = (T)item;
                var pValue = fieldSelector(product);
                if (pValue < result) result = pValue;

            }
            return result;
        }

        public static int Max<T>(this IEnumerable list, Func<T, int> fieldSelector)
        {
            var result = int.MinValue;
            foreach (var item in list)
            {
                var product = (T)item;
                var pValue = fieldSelector(product);
                if (pValue > result) result = pValue;

            }
            return result;
        }

        public static Decimal Average<T>(this IEnumerable list, Func<T, Decimal> fieldSelector)
        {
            var total = (decimal)0;
            var count = 0;
            foreach (var item in list)
            {
                var product = (T)item;
                var pValue = fieldSelector(product);
                total += pValue;
                count++;
            }
            return total / count;
        }
        public static IEnumerable<T> Search<T>(this IEnumerable<T> list, Predicate<T> productSpec)
        {
            //var result = new Products();
            foreach (var item in list)
            {
                var product = (T)item;
                if (productSpec(product))
                    //result.Add(product);
                    yield return product;
            }
            //return result;
        }
        public static IDictionary<TKey, IList<T>> GroupBy<T, TKey>(this IEnumerable<T> list, Func<T, TKey> keySelector) {
            var result = new Dictionary<TKey, IList<T>>();
            foreach (var item in list) {
                var key = keySelector(item);
                if (!result.ContainsKey(key))
                    result[key] = new List<T>();
                result[key].Add(item);
            }
            return result;
        }
    }

    
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayNameAttribute : Attribute
    {
        string _name = string.Empty;
        public DisplayNameAttribute(string name)
        {
            _name = name;
        }
        public string Name
        {
            get
            {
                return _name;
            }
        }

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreDisplay : Attribute { }

}
