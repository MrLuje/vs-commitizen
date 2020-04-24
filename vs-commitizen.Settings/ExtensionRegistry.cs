using Microsoft.VisualStudio.Shell;
using StructureMap;
using System;
using System.IO;
using System.Reflection;
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
                i.AssembliesFromPath(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location));
                i.WithDefaultConventions();
                i.LookForRegistries();
            });

            this.For<IUserSettings>().Use<UserSettings>();
            this.For<IServiceProvider>().Use(ServiceProvider.GlobalProvider);
        }
    }
}
