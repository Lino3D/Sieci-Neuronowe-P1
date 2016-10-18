using Castle.MicroKernel.Registration;
using SNP1.DataHelper;
using SNP1.Models;
using SNP1.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MyCore.Container.Register(Component.For<IOutput>().ImplementedBy<ConsoleWriter>());

            ProgramController.InitializeSimpleNetwork();

            Console.ReadKey();
        }
    }
}
