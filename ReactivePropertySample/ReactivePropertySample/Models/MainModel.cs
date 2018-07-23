using Domain.ValueObjects;
using Prism.Events;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactivePropertySample.Models
{
    public class MainModel : IDisposable
    {
        public SampleTreeViewAdapterList TreeViewList { get; }

        public ReactiveProperty<SampleTreeViewAdapter> IsSelected { get; }

        public MainModel(IEventAggregator _eventAggregator)
        {
            TreeViewList = new SampleTreeViewAdapterList(
                _eventAggregator,
                new ReactiveCollection<SampleTreeViewAdapter>{SampleTreeViewAdapter.Create(Sample.Create("root"), false,
                    new ReactiveCollection<SampleTreeViewAdapter>{
                        SampleTreeViewAdapter.Create(Sample.Create("Notifiers"), false,
                            new ReactiveCollection<SampleTreeViewAdapter>{
                                SampleTreeViewAdapter.Create(Sample.Create("BooleanNotifier"), true),
                                SampleTreeViewAdapter.Create(Sample.Create("BusyNotifier"), true),
                                SampleTreeViewAdapter.Create(Sample.Create("CountNotifier"), true),
                                SampleTreeViewAdapter.Create(Sample.Create("MessageBroker"), true),
                                SampleTreeViewAdapter.Create(Sample.Create("ScheduledNotifier"), true),
                            }
                        ),
                        SampleTreeViewAdapter.Create(Sample.Create("ReactivePropertySlim"), true),
                        SampleTreeViewAdapter.Create(Sample.Create("ReactiveTimer"), true),
                        SampleTreeViewAdapter.Create(Sample.Create("AsyncReactiveCommand", "ScheduledNotifierView"), true)
                    })
                }
            );

            IsSelected = TreeViewList.IsSelected.Where(item => item != null).ToReactiveProperty().AddTo(DisposeCollection);
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
    }
}
