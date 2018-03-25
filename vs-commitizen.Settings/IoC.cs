using Microsoft.VisualStudio.Shell;
using StructureMap;
using System;

namespace vs_commitizen.Settings
{
    public static class IoC
    {
        public static Container Container;

        static IoC()
        {
            Container = new Container(c =>
            {
                c.Scan(i =>
                {
                    i.TheCallingAssembly();
                    i.WithDefaultConventions();
                });

                c.For<IServiceProvider>().Use(ServiceProvider.GlobalProvider);
            });
        }

        public static T GetInstance<T>()
        {
            return Container.GetInstance<T>();
        }
    }
}
