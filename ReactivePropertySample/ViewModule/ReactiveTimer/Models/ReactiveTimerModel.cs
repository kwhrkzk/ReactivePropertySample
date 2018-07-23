using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModule.ReactiveTimer.Models
{
    public enum TimerStatus
    {
        STOP,
        PAUSE,
        START
    }

    public class ReactiveTimerModel : IDisposable
    {
        public Reactive.Bindings.ReactiveTimer ReactiveTimer { get; } = new Reactive.Bindings.ReactiveTimer(TimeSpan.FromSeconds(1));
        public Reactive.Bindings.ReactivePropertySlim<TimerStatus> Status { get; } = new Reactive.Bindings.ReactivePropertySlim<TimerStatus>(TimerStatus.STOP);

        public IObservable<bool> CanStart() => Status.Select(status => TimerStatus.STOP.Equals(status) || TimerStatus.PAUSE.Equals(status));
        public void Start()
        {
            Status.Value = TimerStatus.START;
            ReactiveTimer.Start();
        }

        public IObservable<bool> CanPause() => Status.Select(status => TimerStatus.START.Equals(status));
        public void Pause()
        {
            Status.Value = TimerStatus.PAUSE;
            ReactiveTimer.Stop();
        }

        public IObservable<bool> ChangeStop() => Status.Select(status => TimerStatus.STOP.Equals(status)).Where(b => b);
        public IObservable<bool> CanStop() => Status.Select(status => TimerStatus.START.Equals(status) || TimerStatus.PAUSE.Equals(status));
        public void Stop()
        {
            Status.Value = TimerStatus.STOP;
            ReactiveTimer.Reset();
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
