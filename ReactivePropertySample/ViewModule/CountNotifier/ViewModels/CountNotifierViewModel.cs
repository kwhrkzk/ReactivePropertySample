using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.Notifiers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ViewModule.CountNotifier.ViewModels
{
    public class CountNotifierViewModel : IConfirmNavigationRequest, IRegionMemberLifetime, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool KeepAlive => false;

        public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("CountNotifier");

        public Reactive.Bindings.Notifiers.CountNotifier CountNotifier { get; } = new Reactive.Bindings.Notifiers.CountNotifier(50);
        public ReactiveProperty<string> CountNotifierStatus { get; }
        public ReactiveProperty<int> CountNotifierCount { get; }
        public ReactiveProperty<int> DecrementN { get; } = new ReactiveProperty<int>(7);
        public ReactiveProperty<int> IncrementN { get; } = new ReactiveProperty<int>(7);

        public ReactiveCommand Decrement1Command { get; } = new ReactiveCommand();
        public ReactiveCommand Decrement10Command { get; } = new ReactiveCommand();
        public ReactiveCommand DecrementNCommand { get; } = new ReactiveCommand();
        public ReactiveCommand Increment1Command { get; } = new ReactiveCommand();
        public ReactiveCommand Increment10Command { get; } = new ReactiveCommand();
        public ReactiveCommand IncrementNCommand { get; } = new ReactiveCommand();
        public ReactiveCommand MaxCommand { get; } = new ReactiveCommand();
        public ReactiveCommand EmptyCommand { get; } = new ReactiveCommand();

        private IDisposable beforeOperation;

        public CountNotifierViewModel()
        {
            CountNotifierStatus = CountNotifier.Select(item => Enum.GetName(typeof(CountChangedStatus), item)).ToReactiveProperty().AddTo(DisposeCollection);
            CountNotifierCount = CountNotifier.Select(item => CountNotifier.Count).ToReactiveProperty().AddTo(DisposeCollection);

            CountNotifier
                .ObserveOnUIDispatcher()
                .Where(item => CountChangedStatus.Max.Equals(item))
                .Where(_ => MessageBoxResult.Yes.Equals(MessageBox.Show("Maxになりましたが元に戻しますか？", "確認", MessageBoxButton.YesNo)))
                .Subscribe(_ => beforeOperation.Dispose())
                .AddTo(DisposeCollection);

            Decrement1Command.ObserveOnUIDispatcher().Subscribe(_ => CountNotifier.Decrement(1)).AddTo(DisposeCollection);
            Decrement10Command.ObserveOnUIDispatcher().Subscribe(_ => CountNotifier.Decrement(10)).AddTo(DisposeCollection);
            DecrementNCommand.ObserveOnUIDispatcher().Subscribe(_ => CountNotifier.Decrement(DecrementN.Value)).AddTo(DisposeCollection);
            Increment1Command.ObserveOnUIDispatcher().Subscribe(_ => beforeOperation = CountNotifier.Increment(1)).AddTo(DisposeCollection);
            Increment10Command.ObserveOnUIDispatcher().Subscribe(_ => beforeOperation = CountNotifier.Increment(10)).AddTo(DisposeCollection);
            IncrementNCommand.ObserveOnUIDispatcher().Subscribe(_ => beforeOperation = CountNotifier.Increment(IncrementN.Value)).AddTo(DisposeCollection);
            MaxCommand.ObserveOnUIDispatcher().Subscribe(_ => beforeOperation = CountNotifier.Increment(CountNotifier.Max)).AddTo(DisposeCollection);
            EmptyCommand.ObserveOnUIDispatcher().Subscribe(_ => CountNotifier.Decrement(CountNotifier.Max)).AddTo(DisposeCollection);
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
