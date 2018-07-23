using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModule.ReactiveTimer.Models;

namespace ViewModule.ReactiveTimer.ViewModels
{
    public class ReactiveTimerViewModel : IConfirmNavigationRequest, IRegionMemberLifetime, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool KeepAlive => false;

        public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("ReactiveTimer");

        public ReadOnlyReactivePropertySlim<long> ReadOnlyReactiveTimer { get; }
        public ReactiveCommand StartCommand { get; }
        public ReactiveCommand PauseCommand { get; }
        public ReactiveCommand StopCommand { get; }

        public ReactiveTimerModel Model { get; }

        public ReactiveTimerViewModel(ReactiveTimerModel _model)
        {
            Model = _model.AddTo(DisposeCollection);

            ReadOnlyReactiveTimer = 
                Observable.Merge(
                    Model.ReactiveTimer, 
                    Model.ChangeStop().Select(_ => (long)0)
                ).ToReadOnlyReactivePropertySlim()
                .AddTo(DisposeCollection);

            StartCommand = Model.CanStart().ToReactiveCommand().AddTo(DisposeCollection);
            StartCommand.Subscribe(start).AddTo(DisposeCollection);

            PauseCommand = Model.CanPause().ToReactiveCommand().AddTo(DisposeCollection);
            PauseCommand.Subscribe(pause).AddTo(DisposeCollection);

            StopCommand = Model.CanStop().ToReactiveCommand().AddTo(DisposeCollection);
            StopCommand.Subscribe(stop).AddTo(DisposeCollection);
        }

        private void start() => Model.Start();
        private void pause() => Model.Pause();
        private void stop() => Model.Stop();

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

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback) => continuationCallback(true);
        public void OnNavigatedTo(NavigationContext navigationContext) { }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) => Dispose();
    }
}
