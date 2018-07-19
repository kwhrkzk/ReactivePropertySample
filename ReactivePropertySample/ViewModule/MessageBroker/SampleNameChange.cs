using Domain.ValueObjects;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModule.MessageBroker
{
    public class SampleNameChangeEvent : PubSubEvent<SampleNameChange>
    {
    }

    public class SampleNameChange : ValueObject
    {
        public string Name { get; }

        public SampleNameChange(string _name) => Name = _name;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
        }
    }
}
