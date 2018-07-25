using Domain.ValueObjects;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace ViewModule.CombineLatestValuesAreAll.Models
{
    public class CombineLatestValuesAreAllModel : IDisposable
    {
        public Reactive.Bindings.ReactivePropertySlim<SampleName> SampleName { get; } = new Reactive.Bindings.ReactivePropertySlim<SampleName>(Domain.ValueObjects.SampleName.NullObject);
        public Reactive.Bindings.ReactivePropertySlim<ViewName> ViewName { get; } = new Reactive.Bindings.ReactivePropertySlim<ViewName>(Domain.ValueObjects.ViewName.NullObject);

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
