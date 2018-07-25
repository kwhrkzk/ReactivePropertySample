using Domain.ValueObjects;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using ViewModule.CombineLatestValuesAreAll.Models;

namespace ViewModule.CombineLatestValuesAreAll.ViewModels
{
    public class CombineLatestValuesAreAllViewModel : IConfirmNavigationRequest, IRegionMemberLifetime, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool KeepAlive => false;

        public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("CombineLatestValuesAreAll");

        public InteractionRequest<Confirmation> ConfirmationRequest { get; } = new InteractionRequest<Confirmation>();
        public InteractionRequest<Notification> NotificationRequest { get; } = new InteractionRequest<Notification>();

        public ReactiveProperty<string> SampleNameInput { get; }
        public ReactiveProperty<string> ViewNameInput { get; }
        public ReactiveCommand ExecuteCommand { get; } = new ReactiveCommand();

        public ReactiveProperty<string> SampleNameInput2 { get; }
        public ReactiveProperty<string> ViewNameInput2 { get; }
        public ReactiveCommand ExecuteCommand2 { get; } = new ReactiveCommand();

        public CombineLatestValuesAreAllModel Model { get; }

        public CombineLatestValuesAreAllViewModel(CombineLatestValuesAreAllModel _model)
        {
            Model = _model.AddTo(DisposeCollection);

            #region
            SampleNameInput = 
                Model.SampleName
                .Select(n => n.Name)
                .ToReactiveProperty(null, Reactive.Bindings.ReactivePropertyMode.Default | Reactive.Bindings.ReactivePropertyMode.IgnoreInitialValidationError)
                .SetValidateNotifyError(new Func<string, string>(sampleNameValidate))
                .AddTo(DisposeCollection);

            ViewNameInput = 
                Model.ViewName
                .Select(n => n.Name)
                .ToReactiveProperty(null, Reactive.Bindings.ReactivePropertyMode.Default | Reactive.Bindings.ReactivePropertyMode.IgnoreInitialValidationError)
                .SetValidateNotifyError(new Func<string, string>(viewNameValidate))
                .AddTo(DisposeCollection);

            ExecuteCommand = new ReactiveCommand().AddTo(DisposeCollection);

            ExecuteCommand
                .ObserveOnUIDispatcher()
                .Where(confirmation)
                .Where(validate)
                .Subscribe(_ => {
                }).AddTo(DisposeCollection);
            #endregion

            #region
            SampleNameInput2 =
                Model.SampleName
                .Select(n => n.Name)
                .ToReactiveProperty()
                .SetValidateNotifyError(new Func<string, string>(sampleNameValidate))
                .AddTo(DisposeCollection);

            ViewNameInput2 =
                Model.ViewName
                .Select(n => n.Name)
                .ToReactiveProperty()
                .SetValidateNotifyError(new Func<string, string>(viewNameValidate))
                .AddTo(DisposeCollection);

            ExecuteCommand2 =
                canExecuteParameters2()
                .Select(r => r.ObserveHasErrors)
                .CombineLatestValuesAreAllFalse()
                .ToReactiveCommand()
                .AddTo(DisposeCollection);

            ExecuteCommand2
                .ObserveOnUIDispatcher()
                .Where(confirmation)
                .Subscribe(_ => {
                }).AddTo(DisposeCollection);
            #endregion
        }

        private IEnumerable<ReactiveProperty<string>> canExecuteParameters()
        {
            yield return SampleNameInput;
            yield return ViewNameInput;
        }

        private IEnumerable<ReactiveProperty<string>> canExecuteParameters2()
        {
            yield return SampleNameInput2;
            yield return ViewNameInput2;
        }

        private string sampleNameValidate(string str)
        {
            try
            {
                Model.SampleName.Value = SampleName.Create(str);
                return null;
            }
            catch (Exception ex)
            {
                Model.SampleName.Value = SampleName.NullObject;
                return ex.Message;
            }
        }

        private string viewNameValidate(string str)
        {
            try
            {
                Model.ViewName.Value = ViewName.Create(str);
                return null;
            }
            catch (Exception ex)
            {
                Model.ViewName.Value = ViewName.NullObject;
                return ex.Message;
            }
        }

        private bool validate<T>(T _)
        {
            forceValidate();

            var hasNotErros = 
                canExecuteParameters()
                .Select(r => r.HasErrors)
                .All(b => !b);

            if (hasNotErros == false)
                NotificationRequest.Raise(new Notification { Title = "エラー", Content = "入力値エラー" });

            return hasNotErros;
        }

        private void forceValidate() => canExecuteParameters().ToList().ForEach(item => item.ForceValidate());

        private bool confirmation<T>(T _)
        {
            bool ret = false;
            ConfirmationRequest.Raise(new Confirmation { Title = "確認", Content = "何もしないですけど良いですか？", Confirmed = false }, c => ret = c.Confirmed);
            return ret;
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
