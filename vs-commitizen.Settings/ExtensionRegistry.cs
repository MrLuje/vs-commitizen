using Microsoft.VisualStudio.Shell;
using StructureMap;
using System;

namespace vs_commitizen.Settings
{
    public class ExtensionRegistry : Registry
    {
        public ExtensionRegistry()
        {
            this.Scan(i =>
            {
                i.TheCallingAssembly();
                i.WithDefaultConventions();
            });

            this.For<IServiceProvider>().Use(ServiceProvider.GlobalProvider);
        }
    }
}
