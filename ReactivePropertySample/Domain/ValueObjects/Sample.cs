using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class Sample : ValueObject
    {
        public static Sample Create(string _name) => new Sample(_name);
        public string Name { get; }

        private Sample(string _name) => Name = _name;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
        }
    }
}
