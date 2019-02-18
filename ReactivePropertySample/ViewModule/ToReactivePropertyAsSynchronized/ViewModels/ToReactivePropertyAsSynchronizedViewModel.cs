using Domain.ValueObjects;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using ViewModule.ToReactivePropertyAsSynchronized.Models;

namespace ViewModule.ToReactivePropertyAsSynchronized.ViewModels
{
    public class ToReactivePropertyAsSynchronizedViewModel : IConfirmNavigationRequest, IRegionMemberLifetime, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        private IRegionNavigationService RegionNavigationService { get; set; }

        public bool KeepAlive => false;

        public Reactive.Bindings.Notifiers.BooleanNotifier InProgress { get; } = new Reactive.Bindings.Notifiers.BooleanNotifier(false);
        public ReactivePropertySlim<string> ProgressMessage { get; } = new ReactivePropertySlim<string>("");

        public ReactiveProperty<string> IgnoreValidationErrorValueTrueInput { get; }
        public ReactiveCommand ShowCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<string> IgnoreValidationErrorValueFalseInput { get; }
        public ReactiveCommand Show2Command { get; } = new ReactiveCommand();

        public ToReactivePropertyAsSynchronizedModel Model { get; }

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

        public ToReactivePropertyAsSynchronizedViewModel(ToReactivePropertyAsSynchronizedModel _model)
        {
            Model = _model.AddTo(DisposeCollection);

            IgnoreValidationErrorValueTrueInput =
                Model.IgnoreValidationErrorValueTrue.ToReactivePropertyAsSynchronized(
                    x => x.Value,
                    x => x.Name,
                    x => String.IsNullOrEmpty(x) ? ViewName.NullObject : ViewName.Create(x),
                    Reactive.Bindings.ReactivePropertyMode.Default | Reactive.Bindings.ReactivePropertyMode.IgnoreInitialValidationError,
                    true
                ).SetValidateNotifyError(x => String.IsNullOrEmpty(x) ? "何か入力してください。" : null)
                .AddTo(DisposeCollection);

            ShowCommand.Subscribe(() => System.Windows.MessageBox.Show(Model.IgnoreValidationErrorValueTrue.Value.Name)).AddTo(DisposeCollection);

            IgnoreValidationErrorValueFalseInput =
                Model.IgnoreValidationErrorValueFalse.ToReactivePropertyAsSynchronized(
                    x => x.Value,
                    x => x.Name,
                    x => String.IsNullOrEmpty(x) ? ViewName.NullObject : ViewName.Create(x),
                    Reactive.Bindings.ReactivePropertyMode.Default | Reactive.Bindings.ReactivePropertyMode.IgnoreInitialValidationError,
                    false
                ).SetValidateNotifyError(x => String.IsNullOrEmpty(x) ? "何か入力してください。" : null)
                .AddTo(DisposeCollection);

            Show2Command.Subscribe(() => System.Windows.MessageBox.Show(Model.IgnoreValidationErrorValueFalse.Value.Name)).AddTo(DisposeCollection);

        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback) => continuationCallback(true);
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            RegionNavigationService = navigationContext.NavigationService;

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            InProgress.TurnOn();
            ProgressMessage.Value = "初期情報取得中";
            try
            {
                await Model.InitializeAsync();
            }
            finally
            {
                InProgress.TurnOff();
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) => DisposeCollection.Dispose();
    }
}
