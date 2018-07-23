using Domain.ValueObjects;
using Prism.Events;
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

namespace ViewModule.MessageBroker.ViewModels
{
    public class MessageBrokerViewModel : IConfirmNavigationRequest, IRegionMemberLifetime, IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 0067

        public bool KeepAlive => false;

        public ReactivePropertySlim<string> Title { get; } = new ReactivePropertySlim<string>("MessageBroker");

        public ReactivePropertySlim<string> MessageBrokerName { get; } = new ReactivePropertySlim<string>("変更後の名前");
        public ReactiveCommand MessageBrokerCommand { get; } = new ReactiveCommand();
        public ReactiveCommand PubSubEventCommand { get; } = new ReactiveCommand();

        public MessageBrokerViewModel(IEventAggregator eventAggregator)
        {
            MessageBrokerCommand.Subscribe(_ => Reactive.Bindings.Notifiers.MessageBroker.Default.Publish<SampleNameChange>(new SampleNameChange(SampleName.Create(MessageBrokerName.Value), ViewName.Create("MessageBrokerView")))).AddTo(DisposeCollection);
            PubSubEventCommand.Subscribe(_ => eventAggregator.GetEvent<SampleNameChangeEvent>().Publish(new SampleNameChange(SampleName.Create(MessageBrokerName.Value), ViewName.Create("MessageBrokerView")))).AddTo(DisposeCollection);
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
