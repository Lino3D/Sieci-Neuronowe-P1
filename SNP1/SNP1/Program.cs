using Castle.MicroKernel.Registration;
using SNP1.DataHelper;
using SNP1.Models;
using SNP1.Models.Interfaces;
using System;

namespace SNP1
{
    internal class Program
    {
        
        private static void Main(string[] args)
        {
            MyCore.Container.Register(Component.For<IOutput>().ImplementedBy<ConsoleWriter>());
          
            ProgramController.InitializeSimpleNetwork();  
            
            Console.ReadKey();
        }
    }
}