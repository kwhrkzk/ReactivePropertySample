using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ReactivePropertySample
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            // マネージコード内で例外がスローされると最初に必ず発生する（.NET 4.0より）
            //AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

            // バックグラウンドタスク内で処理されなかったら発生する（.NET 4.0より）
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            // 例外が処理されなかったら発生する（.NET 1.0より）
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var boot = new Bootstrapper();
            boot.Run();
        }

        //private void CurrentDomain_FirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        //{
        //    string errorMember = e.Exception.TargetSite.Name;
        //    string errorMessage = e.Exception.Message;
        //    string message = string.Format(@"例外が{0}で発生。プログラムは継続します。エラーメッセージ：{1}", errorMember, errorMessage);

        //    MessageBox.Show(message, "FirstChanceException", MessageBoxButton.OK, MessageBoxImage.Information);
        //    Container.Resolve<ILoggingService>().Logger.Fatal(message);
        //}

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMember = e.Exception.TargetSite.Name;
            string errorMessage = e.Exception.Message;
            string message = string.Format(@"例外が{0}で発生。プログラムを継続しますか？エラーメッセージ：{1}", errorMember, errorMessage);

            MessageBoxResult result = MessageBox.Show(message, "DispatcherUnhandledException", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                e.Handled = true;
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            string errorMember = e.Exception.InnerException.TargetSite.Name;
            string errorMessage = e.Exception.InnerException.Message;
            string message = string.Format(@"例外がバックグラウンドタスクの{0}で発生。プログラムを継続しますか？エラーメッセージ：{1}", errorMember, errorMessage);

            MessageBoxResult result = MessageBox.Show(message, "UnobservedTaskException", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
                e.SetObserved();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;
            if (exception == null)
            {
                MessageBox.Show("System.Exceptionとして扱えない例外");
                return;
            }

            string errorMember = exception.TargetSite.Name;
            string errorMessage = exception.Message;
            string message = string.Format(@"例外が{0}で発生。プログラムは終了します。エラーメッセージ：{1}", errorMember, errorMessage);

            MessageBox.Show(message, "UnhandledException", MessageBoxButton.OK, MessageBoxImage.Stop);
            Environment.Exit(0);
        }
    }
}
