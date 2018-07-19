using Domain.ValueObjects;
using Reactive.Bindings;
using Reactive.Bindings.Notifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactivePropertySample
{
    public class SampleTreeViewAdapter : ValueObject
    {
        public static SampleTreeViewAdapter Create(Sample _sample, bool _canView, List<SampleTreeViewAdapter> _children) => new SampleTreeViewAdapter(_sample, _canView, _children);
        public static SampleTreeViewAdapter Create(Sample _sample, bool _canView) => Create(_sample, _canView, null);

        private Sample sample { get; }

        public string Name => sample.Name;

        public List<SampleTreeViewAdapter> Children { get; }

        public bool CanView { get; }
        public string ViewName => Name + "View";

        public ReactivePropertySlim<bool> IsSelected { get; } = new ReactivePropertySlim<bool>(false);

        private SampleTreeViewAdapter(Sample _sample, bool _canView, List<SampleTreeViewAdapter> _children)
        {
            sample = _sample;
            CanView = _canView;
            Children = _children ?? new List<SampleTreeViewAdapter>();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return sample;
            yield return CanView;
            yield return Children;
        }
    }
}
