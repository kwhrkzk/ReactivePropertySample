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

namespace ViewModule.ReactivePropertyMode.ViewModels
{
    public class ReactivePropertyModeViewModel : IConfirmNavigationRequest, IRegionMemberLifetime, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool KeepAlive => false;

        public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("ReactivePropertyMode");

        public ReactiveProperty<string> Default { get; }
        public ReactiveProperty<string> IgnoreInitialValidationError { get; }
        public ReadOnlyReactivePropertySlim<string> DefaultTextBlock { get; }
        public ReadOnlyReactivePropertySlim<string> IgnoreInitialValidationErrorTextBlock { get; }

        public ReactiveProperty<bool> DefaultBool { get; } = new ReactiveProperty<bool>(false);
        public ReactiveProperty<bool> RaiseLatestValueOnSubscribe { get; }
        public ReactiveProperty<bool> DistinctUntilChanged { get; }

        public ReactiveCommand TrueCommand { get; } = new ReactiveCommand();
        public ReactiveCommand FalseCommand { get; } = new ReactiveCommand();

        public ReactivePropertyModeViewModel()
        {
            #region Default | IgnoreInitialValidationError
            Default = new ReactiveProperty<string>(null, Reactive.Bindings.ReactivePropertyMode.Default).SetValidateNotifyError(new Func<string, string>(validate)).AddTo(DisposeCollection);

            IgnoreInitialValidationError = new ReactiveProperty<string>(null, Reactive.Bindings.ReactivePropertyMode.Default | Reactive.Bindings.ReactivePropertyMode.IgnoreInitialValidationError).SetValidateNotifyError(new Func<string, string>(validate)).AddTo(DisposeCollection);

            DefaultTextBlock = Default.ToReadOnlyReactivePropertySlim(null, Reactive.Bindings.ReactivePropertyMode.Default);

            IgnoreInitialValidationErrorTextBlock = IgnoreInitialValidationError.ToReadOnlyReactivePropertySlim(null, Reactive.Bindings.ReactivePropertyMode.IgnoreInitialValidationError);
            #endregion Default | IgnoreInitialValidationError

            #region RaiseLatestValueOnSubscribe DistinctUntilChanged
            RaiseLatestValueOnSubscribe = new ReactiveProperty<bool>(false, Reactive.Bindings.ReactivePropertyMode.RaiseLatestValueOnSubscribe).AddTo(DisposeCollection);
            DistinctUntilChanged = new ReactiveProperty<bool>(false, Reactive.Bindings.ReactivePropertyMode.DistinctUntilChanged).AddTo(DisposeCollection);

            TrueCommand.Subscribe(_ =>
            {
                RaiseLatestValueOnSubscribe.Value = true;
                DistinctUntilChanged.Value = true;
                DefaultBool.Value = true;
            });
            FalseCommand.Subscribe(_ =>
            {
                RaiseLatestValueOnSubscribe.Value = false;
                DistinctUntilChanged.Value = false;
                DefaultBool.Value = false;
            });
            DefaultBool.Subscribe(b => System.Windows.MessageBox.Show(b.ToString(), "DefaultBool"));
            RaiseLatestValueOnSubscribe.Subscribe(b => System.Windows.MessageBox.Show(b.ToString(), "RaiseLatestValueOnSubscribe"));
            DistinctUntilChanged.Subscribe(b => System.Windows.MessageBox.Show(b.ToString(), "DistinctUntilChanged"));
            #endregion RaiseLatestValueOnSubscribe DistinctUntilChanged
        }

        private string validate(string str)
        {
            try
            {
                if (String.IsNullOrEmpty(str))
                    throw new ArgumentException("String.IsNullOrEmpty", "入力値");

                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
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
