using Castle.MicroKernel.Registration;
using SNP1.DataHelper;
using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            MyCore.Container.Register(Component.For<IOutput>().ImplementedBy<WPFWriter>());
        }

    }
}
