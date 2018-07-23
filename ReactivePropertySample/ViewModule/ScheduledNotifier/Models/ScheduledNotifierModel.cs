using Domain.Services;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace ViewModule.ScheduledNotifier.Models
{
    public class ScheduledNotifierModel : IDisposable
    {
        private ITakeLongTime takeLongTime { get; }

        public ScheduledNotifierModel(ITakeLongTime _takeLongTime)
        {
            takeLongTime = _takeLongTime;
        }

        public void TakeLongTime(IProgress<int> _progress)
        {
            _progress.Report(5);
            Enumerable.Range(0, ExecutionTime.Create(5).Second).Reverse().ToList().ForEach(i => {
                takeLongTime.Execute(ExecutionTime.Create(1));
                _progress.Report(i);
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
