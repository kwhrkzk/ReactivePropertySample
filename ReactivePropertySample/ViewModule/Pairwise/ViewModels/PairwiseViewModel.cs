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
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ViewModule.Pairwise.ViewModels
{
    public class PairwiseViewModel : IConfirmNavigationRequest, IRegionMemberLifetime, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool KeepAlive => false;

        public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("Pairwise");

        public Subject<int> Stream { get; } = new Subject<int>();
        public ReadOnlyReactivePropertySlim<int> OldNumber { get; }
        public ReadOnlyReactivePropertySlim<int> CurrentNumber { get; }
        public ReadOnlyReactivePropertySlim<int> DiffNumber { get; }
        public ReactiveProperty<int> NumberInput { get; } = new ReactiveProperty<int>(RandomProvider.GetThreadRandom().Next());
        public ReactiveCommand CalcCommand { get; } = new ReactiveCommand();

        public PairwiseViewModel()
        {
            CurrentNumber = Stream.ToReadOnlyReactivePropertySlim().AddTo(DisposeCollection);
            OldNumber = Stream.Pairwise().Select(p => p.OldItem).ToReadOnlyReactivePropertySlim().AddTo(DisposeCollection);
            DiffNumber = Stream.Pairwise().Select(p => p.NewItem - p.OldItem).ToReadOnlyReactivePropertySlim().AddTo(DisposeCollection);

            CalcCommand.Subscribe(calc).AddTo(DisposeCollection);
        }

        private void calc()
        {
            Stream.OnNext(NumberInput.Value);
            NumberInput.Value = RandomProvider.GetThreadRandom().Next();
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
