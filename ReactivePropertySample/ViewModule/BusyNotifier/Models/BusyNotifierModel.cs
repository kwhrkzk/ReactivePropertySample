using Domain.Services;
using Domain.ValueObjects;
using Reactive.Bindings;
using Reactive.Bindings.Notifiers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ViewModule.BusyNotifier.Models
{
    public class BusyNotifierModel : IDisposable
    {
        private ITakeLongTime takeLongTime { get; }

        public ReactivePropertySlim<int> Task1Counter { get; } = new ReactivePropertySlim<int>(0);
        public ReactivePropertySlim<int> Task2Counter { get; } = new ReactivePropertySlim<int>(0);

        public BusyNotifierModel(ITakeLongTime _takeLongTime)
        {
            takeLongTime = _takeLongTime;
        }

        public void TakeLongTimeTask1()
        {
            Task1Counter.Value = 5;
            Enumerable.Range(0, ExecutionTime.Create(5).Second).Reverse().ToList().ForEach(i => {
                takeLongTime.Execute(ExecutionTime.Create(1));
                Task1Counter.Value = i;
            });
        }

        public void TakeLongTimeTask2()
        {
            Task2Counter.Value = 7;
            Enumerable.Range(0, ExecutionTime.Create(7).Second).Reverse().ToList().ForEach(i => {
                takeLongTime.Execute(ExecutionTime.Create(1));
                Task2Counter.Value = i;
            });
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
