using Domain.ValueObjects;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.Notifiers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModule.MessageBroker;

namespace ReactivePropertySample
{
    public class SampleTreeViewAdapter : IDisposable
    {
        public static SampleTreeViewAdapter Create(Sample _sample, bool _canView, ReactiveCollection<SampleTreeViewAdapter> _children) => new SampleTreeViewAdapter(_sample, _canView, _children);
        public static SampleTreeViewAdapter Create(Sample _sample, bool _canView) => Create(_sample, _canView, null);

        public Sample Sample { get; }

        public ReactiveProperty<string> Name { get; }

        public ReactiveCollection<SampleTreeViewAdapter> Children { get; }

        public bool CanView { get; }
        public string ViewName => CanView ? Sample.ViewNameName : "";
        public ViewName SampleViewName => Sample.ViewName;

        public ReactivePropertySlim<bool> IsSelected { get; } = new ReactivePropertySlim<bool>(false);

        private SampleTreeViewAdapter(Sample _sample, bool _canView, ReactiveCollection<SampleTreeViewAdapter> _children)
        {
            Sample = _sample;
            CanView = _canView;
            Children = _children ?? new ReactiveCollection<SampleTreeViewAdapter>();
            Name = new ReactiveProperty<string>(Sample.SampleNameName).AddTo(DisposeCollection);
        }

        private CompositeDisposable DisposeCollection = new CompositeDisposable();
        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed")]
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeCollection.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose() => Dispose(true);
        #endregion
    }
}
