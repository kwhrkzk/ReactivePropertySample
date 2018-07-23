using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class SampleNameNullObject : SampleName
    {
        internal SampleNameNullObject() : this("") {}
        internal SampleNameNullObject(string _name) : base(_name) {}
        public override bool IsNullObject => true;
        public override bool IsNotNullObject => false;
    }

    public class SampleName : ValueObject
    {
        public static SampleName NullObject => new SampleNameNullObject();
        public static SampleName Create(string _name)
        {
            if (String.IsNullOrEmpty(_name))
                throw new ArgumentException("String.IsNullOrEmpty", nameof(_name));

            return new SampleName(_name);
        }

        public string Name { get; }

        public ViewName ToViewName() => ViewName.Create(Name + "View");

        public SampleName(string _name) => Name = _name;
        public virtual bool IsNullObject => false;
        public virtual bool IsNotNullObject => true;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
        }

        public override string ToString() => Name;
    }
}
