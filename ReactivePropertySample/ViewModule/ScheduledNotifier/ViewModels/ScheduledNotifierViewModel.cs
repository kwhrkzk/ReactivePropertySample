﻿using Domain.ValueObjects;
using Prism.Interactivity.InteractionRequest;
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
using ViewModule.ScheduledNotifier.Models;

namespace ViewModule.ScheduledNotifier.ViewModels
{
    public class ScheduledNotifierViewModel : IConfirmNavigationRequest, IRegionMemberLifetime, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool KeepAlive => false;

        public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("ScheduledNotifier");

        public AsyncReactiveCommand TakeLongTimeCommand { get; } = new AsyncReactiveCommand();
        public Reactive.Bindings.Notifiers.BusyNotifier BusyNotifier { get; } = new Reactive.Bindings.Notifiers.BusyNotifier();
        public Reactive.Bindings.Notifiers.ScheduledNotifier<int> ScheduledNotifier { get; } = new Reactive.Bindings.Notifiers.ScheduledNotifier<int>();
        public ReadOnlyReactivePropertySlim<int> Progress { get; }

        public ScheduledNotifierModel Model { get; }

        public ScheduledNotifierViewModel(ScheduledNotifierModel _model)
        {
            Model = _model.AddTo(DisposeCollection);

            Progress = ScheduledNotifier.ToReadOnlyReactivePropertySlim().AddTo(DisposeCollection);

            TakeLongTimeCommand.Subscribe(TakeLongTimeAsync).AddTo(DisposeCollection);
        }

        private async Task TakeLongTimeAsync() => await Task.Run(() => Model.TakeLongTime(ScheduledNotifier));

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
        public void OnNavigatedTo(NavigationContext navigationContext) => Title.Value = (navigationContext.Parameters[nameof(Sample)] as Sample).SampleNameName;

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;
        public void OnNavigatedFrom(NavigationContext navigationContext) { }
    }
}
