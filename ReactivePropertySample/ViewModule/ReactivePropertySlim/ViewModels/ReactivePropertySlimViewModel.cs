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
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModule.ReactivePropertySlim.Models;

namespace ViewModule.ReactivePropertySlim.ViewModels
{
    public class ReactivePropertySlimViewModel : IConfirmNavigationRequest, IRegionMemberLifetime, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool KeepAlive => false;

        public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("ReactivePropertySlim");

        public ReactiveProperty<string> SampleNameInput { get; }
        public ReactiveCommand SampleNameChangeCommand { get; } = new ReactiveCommand();

        public ReactivePropertySlimModel Model { get; }

        public ReactivePropertySlimViewModel(ReactivePropertySlimModel _model)
        {
            Model = _model.AddTo(DisposeCollection);

            SampleNameInput = Model.SampleName.Select(item => item.Name)
                .ToReactiveProperty("", Reactive.Bindings.ReactivePropertyMode.IgnoreInitialValidationError | Reactive.Bindings.ReactivePropertyMode.Default)
                .SetValidateNotifyError((Func<string, string>)(SampleNameInputValidate))
                .AddTo(DisposeCollection);

            SampleNameChangeCommand = Model.SampleName.Select(item => item.IsNotNullObject).ToReactiveCommand().AddTo(DisposeCollection);
            SampleNameChangeCommand.Subscribe(SampleNameChange).AddTo(DisposeCollection);
        }

        private string SampleNameInputValidate(string str)
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

        private void SampleNameChange() => Model.SampleNameChange();

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
