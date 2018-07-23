using Prism.Regions;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings.Notifiers;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;

namespace ViewModule.BooleanNotifier.ViewModels
{
    public class BooleanNotifierViewModel : IConfirmNavigationRequest, IRegionMemberLifetime, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool KeepAlive => false;

        public Reactive.Bindings.Notifiers.CountNotifier CountNotifier { get; } = new Reactive.Bindings.Notifiers.CountNotifier(999);
        public Reactive.Bindings.Notifiers.BooleanNotifier BooleanNotifier { get; } = new Reactive.Bindings.Notifiers.BooleanNotifier();
        public ReactiveCommand ToggleCommand { get; } = new ReactiveCommand();
        public ReactiveCommand ONCommand { get; } = new ReactiveCommand();
        public ReactiveCommand OFFCommand { get; } = new ReactiveCommand();

        public BooleanNotifierViewModel()
        {
            ToggleCommand.Subscribe(BooleanNotifier.SwitchValue).AddTo(DisposeCollection);
            ONCommand.Subscribe(BooleanNotifier.TurnOn).AddTo(DisposeCollection);
            OFFCommand.Subscribe(BooleanNotifier.TurnOff).AddTo(DisposeCollection);

            BooleanNotifier.Subscribe(_ => CountNotifier.Increment()).AddTo(DisposeCollection);
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
