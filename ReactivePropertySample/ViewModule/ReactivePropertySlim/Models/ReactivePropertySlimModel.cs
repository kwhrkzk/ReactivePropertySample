using Domain.Services;
using Domain.ValueObjects;
using Prism.Events;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using ViewModule.MessageBroker;

namespace ViewModule.ReactivePropertySlim.Models
{
    public class ReactivePropertySlimModel : IDisposable
    {
        private IEventAggregator eventAggregator { get; }

        public ReactivePropertySlim<SampleName> SampleName { get; } = new ReactivePropertySlim<SampleName>(Domain.ValueObjects.SampleName.NullObject);

        public ReactivePropertySlimModel(IEventAggregator _eventAggregator)
        {
            eventAggregator = _eventAggregator;
        }

        public void SampleNameChange() 
            => eventAggregator.GetEvent<SampleNameChangeEvent>().Publish(new SampleNameChange(SampleName.Value, ViewName.Create("ReactivePropertySlimView")));

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
    }
}
