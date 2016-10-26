using Caliburn.Micro;
using WinAppV2.ViewModels;

namespace WinAppV2
{
    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
            //     DisplayRootViewFor<InProgressViewModel>();
        }
    }
}