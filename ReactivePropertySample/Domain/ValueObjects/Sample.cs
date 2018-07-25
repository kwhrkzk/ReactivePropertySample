using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class Sample : ValueObject
    {
        public static Sample Create(SampleName _sampleName, ViewName _viewName) => new Sample(_sampleName, _viewName);
        public static Sample Create(string _sampleName, string _viewName) => new Sample(_sampleName, _viewName);
        public static Sample Create(string _sampleName) => new Sample(_sampleName, _sampleName + "View");
        public SampleName SampleName { get; }
        public string SampleNameName => SampleName.Name;
        public ViewName ViewName { get; }
        public string ViewNameName => ViewName.Name;

        private Sample(SampleName _sampleName, ViewName _viewName)
        {
            SampleName = _sampleName;
            ViewName = _viewName;
        }

        private Sample(string _sampleName, string _viewName)
            : this(SampleName.Create(_sampleName), ViewName.Create(_viewName)) { }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return SampleName;
            yield return ViewName;
        }
    }
}
