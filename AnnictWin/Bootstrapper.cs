using Autofac;
using Prism.Autofac;
using AnnictWin.Views;
using System.Windows;

namespace AnnictWin
{
    class Bootstrapper : AutofacBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
