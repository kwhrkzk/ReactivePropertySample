using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class ViewNameNullObject : ViewName
    {
        internal ViewNameNullObject() : this("") { }
        internal ViewNameNullObject(string _name) : base(_name) { }
        public override bool IsNullObject => true;
        public override bool IsNotNullObject => false;
    }

    public class ViewName : ValueObject
    {
        public static ViewName NullObject => new ViewNameNullObject();
        public virtual bool IsNullObject => false;
        public virtual bool IsNotNullObject => true;

        public static ViewName Create(string _name)
        {
            if (String.IsNullOrEmpty(_name))
                throw new ArgumentException("String.IsNullOrEmpty", "ViewName");

            return new ViewName(_name);
        }

        public string Name { get; }

        public ViewName(string _name) => Name = _name;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
        }

        public override string ToString() => Name;
    }
}
