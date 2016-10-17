using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNP1.DataHelper
{
    public static class MyCore
    {
        static IWindsorContainer _Container = null;

        static MyCore()
        {
            _Container = new WindsorContainer();
        }

        public static IWindsorContainer Container
        {
            get
            {
                return _Container;
            }
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return Container.Resolve<T>(name);
        }


    }
}
