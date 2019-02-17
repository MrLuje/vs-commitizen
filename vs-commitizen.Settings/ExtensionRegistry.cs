using Microsoft.VisualStudio.Shell;
using StructureMap;
using System;
using vs_commitizen.vs.Settings;

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

            this.For<IUserSettings>().Use<UserSettings>();
            this.For<IServiceProvider>().Use(ServiceProvider.GlobalProvider);
        }
    }
}
