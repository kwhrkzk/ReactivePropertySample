using Reactive.Bindings.Extensions;
using Reactive.Bindings.Notifiers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ReactivePropertySample
{
    public class SampleTreeViewAdapterList : List<SampleTreeViewAdapter>, IDisposable
    {
        public Subject<SampleTreeViewAdapter> IsSelected { get; } = new Subject<SampleTreeViewAdapter>();

        public SampleTreeViewAdapter FirstItem => allItemRec(this).First(item => item.CanView);

        public SampleTreeViewAdapterList(IEnumerable<SampleTreeViewAdapter> _list)
            : base(_list)
        {
            allItemRec(this)
                .Where(item => item.CanView)
                .ToList().ForEach(onNextSelectedItem);
        }

        private IEnumerable<SampleTreeViewAdapter> allItemRec(IEnumerable<SampleTreeViewAdapter> item)
            => (item.Any()) ? item.Concat(item.SelectMany(x => allItemRec(x.Children))) : item;

        private void onNextSelectedItem(SampleTreeViewAdapter item)
            => item.IsSelected.Where(b => b).Subscribe(b => IsSelected.OnNext(item)).AddTo(DisposeCollection);

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
