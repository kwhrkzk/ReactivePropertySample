using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModule
{
    public class BindingErrorListener : TraceListener
    {
        public void Listen(Action<string> logAction)
        {
            var listener = new BindingErrorListener() { logAction = logAction };

            PresentationTraceSources.AnimationSource.Listeners.Add(listener);
            PresentationTraceSources.DataBindingSource.Listeners.Add(listener);
            PresentationTraceSources.DependencyPropertySource.Listeners.Add(listener);
            PresentationTraceSources.DocumentsSource.Listeners.Add(listener);
            PresentationTraceSources.FreezableSource.Listeners.Add(listener);
            PresentationTraceSources.HwndHostSource.Listeners.Add(listener);
            PresentationTraceSources.MarkupSource.Listeners.Add(listener);
            PresentationTraceSources.NameScopeSource.Listeners.Add(listener);
            PresentationTraceSources.ResourceDictionarySource.Listeners.Add(listener);
            PresentationTraceSources.RoutedEventSource.Listeners.Add(listener);
            PresentationTraceSources.ShellSource.Listeners.Add(listener);
        }

        private Action<string> logAction;

        public override void Write(string message) => logAction(message);
        public override void WriteLine(string message) => logAction(message);
    }
}
