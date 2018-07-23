using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class ViewName : ValueObject
    {
        public static ViewName Create(string _name)
        {
            if (String.IsNullOrEmpty(_name))
                throw new ArgumentException("String.IsNullOrEmpty", nameof(_name));

            return new ViewName(_name);
        }

        public string Name { get; }

        public ViewName(string _name) => Name = _name;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
        }
    }
}
