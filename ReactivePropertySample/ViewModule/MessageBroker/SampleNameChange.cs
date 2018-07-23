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
        public SampleName SampleName { get; }
        public ViewName ViewName { get; }
        public string SampleNameName => SampleName.Name;

        public SampleNameChange(SampleName _sampleName, ViewName _viewName)
        {
            SampleName = _sampleName;
            ViewName = _viewName;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return SampleName;
            yield return ViewName;
        }
    }
}
