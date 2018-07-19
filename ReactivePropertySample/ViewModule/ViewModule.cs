using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModule.BooleanNotifier.Views;
using ViewModule.BusyNotifier.Views;
using ViewModule.CountNotifier.Views;
using ViewModule.MessageBroker.Views;

namespace ViewModule
{
    public class ViewModule : IModule
    {
        private IUnityContainer Container { get; }

        public ViewModule(IUnityContainer _container) => Container = _container;

        public void Initialize()
        {
            Container.RegisterTypeForNavigation<BooleanNotifierView>(nameof(BooleanNotifierView));
            Container.RegisterTypeForNavigation<BusyNotifierView>(nameof(BusyNotifierView));
            Container.RegisterTypeForNavigation<CountNotifierView>(nameof(CountNotifierView));
            Container.RegisterTypeForNavigation<MessageBrokerView>(nameof(MessageBrokerView));

            Container.RegisterType<BindingErrorListener>(new ContainerControlledLifetimeManager());

            Container.Resolve<BindingErrorListener>().Listen(m => System.Windows.MessageBox.Show(m));
        }
    }
}
