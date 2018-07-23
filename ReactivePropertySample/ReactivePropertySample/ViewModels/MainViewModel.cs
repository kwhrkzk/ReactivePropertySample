using Domain.ValueObjects;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using ReactivePropertySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModule.BooleanNotifier.Views;

namespace ReactivePropertySample.ViewModels
{
    public class MainViewModel : IConfirmNavigationRequest, IRegionMemberLifetime, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool KeepAlive => true;

        private IRegionNavigationService RegionNavigationService { get; set; }
        private IRegionManager RegionManager { get; }

        public MainModel Model { get; }

        public MainViewModel(IRegionManager _regionManager, MainModel _model)
        {
            RegionManager = _regionManager;
            Model = _model.AddTo(DisposeCollection);

            Model.IsSelected.Skip(1).Subscribe(item =>
            {
                var param = new NavigationParameters();
                param.Add(nameof(Sample), item.Sample);

                RegionManager.RequestNavigate("ContentRegion", item.ViewName, param);
            }).AddTo(DisposeCollection);
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
        public void OnNavigatedTo(NavigationContext navigationContext) {
            RegionNavigationService = navigationContext.NavigationService;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
