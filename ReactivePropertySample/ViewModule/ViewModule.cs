using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Lifetime;
using ViewModule.BooleanNotifier.Views;
using ViewModule.BusyNotifier.Views;
using ViewModule.CombineLatestValuesAreAll.Views;
using ViewModule.CountNotifier.Views;
using ViewModule.MessageBroker.Views;
using ViewModule.Pairwise.Views;
using ViewModule.ReactivePropertyMode.Views;
using ViewModule.ReactivePropertySlim.Views;
using ViewModule.ReactiveTimer.Views;
using ViewModule.ScheduledNotifier.Views;
using ViewModule.ToReactivePropertyAsSynchronized.Views;

namespace ViewModule
{
    public class ViewModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<BooleanNotifierView>(nameof(BooleanNotifierView));
            containerRegistry.RegisterForNavigation<BusyNotifierView>(nameof(BusyNotifierView));
            containerRegistry.RegisterForNavigation<CountNotifierView>(nameof(CountNotifierView));
            containerRegistry.RegisterForNavigation<MessageBrokerView>(nameof(MessageBrokerView));
            containerRegistry.RegisterForNavigation<ScheduledNotifierView>(nameof(ScheduledNotifierView));
            containerRegistry.RegisterForNavigation<ReactivePropertySlimView>(nameof(ReactivePropertySlimView));
            containerRegistry.RegisterForNavigation<ReactiveTimerView>(nameof(ReactiveTimerView));
            containerRegistry.RegisterForNavigation<PairwiseView>(nameof(PairwiseView));
            containerRegistry.RegisterForNavigation<ReactivePropertyModeView>(nameof(ReactivePropertyModeView));
            containerRegistry.RegisterForNavigation<CombineLatestValuesAreAllView>(nameof(CombineLatestValuesAreAllView));
            containerRegistry.RegisterForNavigation<ToReactivePropertyAsSynchronizedView>(nameof(ToReactivePropertyAsSynchronizedView));

            containerRegistry.Register<BindingErrorListener>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.Resolve<BindingErrorListener>().Listen(m => System.Windows.MessageBox.Show(m));
        }
    }
}
