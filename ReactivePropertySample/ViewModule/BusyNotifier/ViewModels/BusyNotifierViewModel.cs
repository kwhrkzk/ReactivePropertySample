using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModule.BusyNotifier.Models;

namespace ViewModule.BusyNotifier.ViewModels
{
    public class BusyNotifierViewModel : IConfirmNavigationRequest, IRegionMemberLifetime, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool KeepAlive => false;

        public Reactive.Bindings.Notifiers.BusyNotifier BusyNotifier { get; } = new Reactive.Bindings.Notifiers.BusyNotifier();
        public ReactiveProperty<string> BusyNotifierStatus { get; }
        public ReactiveCommand TakeLongTimeCommand { get; }
        public ReactiveCommand OtherTakeLongTimeCommand { get; }

        public BusyNotifierModel Model { get; }

        public BusyNotifierViewModel(BusyNotifierModel _model)
        {
            Model = _model;

            BusyNotifierStatus = BusyNotifier.Select(item => item ? "処理中" : "処理していない").ToReactiveProperty().AddTo(DisposeCollection);

            TakeLongTimeCommand = BusyNotifier.Select(b => !b).ToReactiveCommand().AddTo(DisposeCollection);
            TakeLongTimeCommand.Subscribe(TakeLongTimeAsync).AddTo(DisposeCollection);

            OtherTakeLongTimeCommand = BusyNotifier.Select(b => !b).ToReactiveCommand().AddTo(DisposeCollection);
            OtherTakeLongTimeCommand.Subscribe(TakeLongTimeAsync).AddTo(DisposeCollection);
            OtherTakeLongTimeCommand.Subscribe(OtherTakeLongTimeAsync).AddTo(DisposeCollection);
        }

        private async void TakeLongTimeAsync()
        {
            using (this.BusyNotifier.ProcessStart())
            {
                await Task.Run(() => Model.TakeLongTimeTask1());
            }
        }

        private async void OtherTakeLongTimeAsync()
        {
            using (BusyNotifier.ProcessStart())
            {
                await Task.Run(() => Model.TakeLongTimeTask2());
            }
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

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback) => continuationCallback(true);
        public void OnNavigatedTo(NavigationContext navigationContext) { }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) => Dispose();
    }
}
