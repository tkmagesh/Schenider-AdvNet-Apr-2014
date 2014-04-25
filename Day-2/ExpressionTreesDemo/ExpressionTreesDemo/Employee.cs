using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExpressionTreesDemo
{
    public class Employee : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        public string FirstName
        {
            set {
                _firstName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(Name.Of<Employee>(e => e.FirstName)));
                PropertyChanged(this, new PropertyChangedEventArgs(Name.Of<Employee>(e => e.FullName)));
            }
            get
            {
                return _firstName;
            }
        }

        public string LastName {
            set
            {
                _lastName = value;
                PropertyChanged(this, new PropertyChangedEventArgs(Name.Of<Employee>(e => e.LastName)));
                PropertyChanged(this, new PropertyChangedEventArgs(Name.Of<Employee>(e => e.FullName)));
            }
            get
            {
                return _lastName;
            }
        }

        public string FullName {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate {};
    }

    public static class Name {
        public static string Of<T>(Expression<Func<T,object>> exp) {
            var memberExp = exp.Body as MemberExpression;
            return memberExp.Member.Name;
        }
    }
}
