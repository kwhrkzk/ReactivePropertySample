﻿using Domain.ValueObjects;
using Prism.Events;
using Reactive.Bindings.Extensions;
using Reactive.Bindings.Notifiers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using ViewModule.MessageBroker;

namespace ReactivePropertySample
{
    public class SampleTreeViewAdapterList : List<SampleTreeViewAdapter>, IDisposable
    {
        public Subject<SampleTreeViewAdapter> IsSelected { get; } = new Subject<SampleTreeViewAdapter>();

        public SampleTreeViewAdapter FirstItem => allItemRec(this).First(item => item.CanView);

        public SampleTreeViewAdapterList(IEventAggregator eventAggregator, IEnumerable<SampleTreeViewAdapter> _list)
            : base(_list)
        {
            allItemRec(this)
                .Where(item => item.CanView)
                .ToList().ForEach(onNextSelectedItem);

            eventAggregator.GetEvent<SampleNameChangeEvent>()
                .Subscribe(item => {
                    var (ret, messageBrokerView) = getSampleTreeViewAdapter(item.ViewName);
                    if (ret)
                        messageBrokerView.Name.Value = item.SampleNameName;

                }).AddTo(DisposeCollection);

            MessageBroker.Default.ToObservable<SampleNameChange>()
                .Select(sampleNameChange => new { sampleNameChange, tpl = getSampleTreeViewAdapter(sampleNameChange.ViewName) })
                .Where(a => a.tpl.Item1)
                .Subscribe(a => a.tpl.Item2.Name.Value = a.sampleNameChange.SampleNameName)
                .AddTo(DisposeCollection);
        }

        private ValueTuple<bool, SampleTreeViewAdapter> getSampleTreeViewAdapter(ViewName _viewName)
        {
            var tmp = allItemRec(this).FirstOrDefault(x => _viewName.Equals(x.SampleViewName));
            return (tmp == null) ? (false, null) : (true, tmp);
        }

        private IEnumerable<SampleTreeViewAdapter> allItemRec(IEnumerable<SampleTreeViewAdapter> item)
            => (item.Any()) ? item.Concat(item.SelectMany(x => allItemRec(x.Children))) : item;

        private void onNextSelectedItem(SampleTreeViewAdapter item)
            => item.IsSelected.Where(b => b).Subscribe(b => IsSelected.OnNext(item)).AddTo(DisposeCollection);

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
