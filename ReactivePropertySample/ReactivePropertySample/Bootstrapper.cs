using Domain.Services;
using Instances.Services;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using ReactivePropertySample.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReactivePropertySample
{
    public class Bootstrapper : UnityBootstrapper
    {
        public void SetRegisterType(IUnityContainer container)
        {
            container.RegisterTypeForNavigation<MainView>(nameof(MainView));
            container.RegisterType<ITakeLongTime, TakeLongTime>(new ContainerControlledLifetimeManager());
        }

        protected override IUnityContainer CreateContainer()
        {
            var container = base.CreateContainer();
            SetRegisterType(container);

            return container;
        }

        protected override DependencyObject CreateShell()
        {
            // this.ContainerでUnityのコンテナが取得できるので
            // そこからShellを作成する
            var shell = this.Container.Resolve<Shell>();

            Container.RegisterInstance<Window>(shell);

            return shell;
        }

        // Shellを表示する
        protected override void InitializeShell()
        {
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();

            Container.Resolve<IRegionManager>().RequestNavigate("MainRegion", nameof(MainView));
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var catalog = (ModuleCatalog)this.ModuleCatalog;
            catalog.AddModule(typeof(ViewModule.ViewModule));
        }
    }
}
