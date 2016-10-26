using Castle.MicroKernel.Registration;
using SNP1.DataHelper;
using SNP1.Models.Interfaces;
using System.Windows;

namespace WinAppV2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MyCore.Container.Register(Component.For<IOutput>().ImplementedBy<WPFConsoleWriter>());
        }
    }
}