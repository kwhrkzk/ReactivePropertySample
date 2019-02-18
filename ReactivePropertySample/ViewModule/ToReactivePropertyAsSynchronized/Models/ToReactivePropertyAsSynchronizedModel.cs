using Domain.Services;
using Domain.ValueObjects;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace ViewModule.ToReactivePropertyAsSynchronized.Models
{
    public class ToReactivePropertyAsSynchronizedModel : IDisposable
    {
        private ITakeLongTime TakeLongTime { get; }

        public ReactivePropertySlim<ViewName> IgnoreValidationErrorValueTrue { get; } = new ReactivePropertySlim<ViewName>(Domain.ValueObjects.ViewName.NullObject);
        public ReactivePropertySlim<ViewName> IgnoreValidationErrorValueFalse { get; } = new ReactivePropertySlim<ViewName>(Domain.ValueObjects.ViewName.NullObject);

        public ToReactivePropertyAsSynchronizedModel(ITakeLongTime _takeLongTime)
        {
            TakeLongTime = _takeLongTime;
        }

        internal async Task InitializeAsync()
        {
            await Task.Run(() => TakeLongTime.Execute(ExecutionTime.Create(2)));
            IgnoreValidationErrorValueTrue.Value = Domain.ValueObjects.ViewName.Create("非同期で取得する初期値");
            IgnoreValidationErrorValueFalse.Value = Domain.ValueObjects.ViewName.Create("非同期で取得する初期値2");
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
