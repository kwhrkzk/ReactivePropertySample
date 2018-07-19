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

namespace ViewModule
{
    public class ViewModule : IModule
    {
        private IUnityContainer Container { get; }

        public ViewModule(IUnityContainer _container) => Container = _container;

        public void Initialize()
        {
            Container.RegisterTypeForNavigation<BooleanNotifierView>(nameof(BooleanNotifierView));

            Container.RegisterType<BindingErrorListener>(new ContainerControlledLifetimeManager());

            Container.Resolve<BindingErrorListener>().Listen(m => System.Windows.MessageBox.Show(m));
        }
    }
}
