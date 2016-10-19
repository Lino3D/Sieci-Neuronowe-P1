using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using SNP1.DataHelper;
using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }


}
