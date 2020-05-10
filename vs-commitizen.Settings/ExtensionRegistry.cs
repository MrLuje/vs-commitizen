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
                i.Assembly("vs-commitizen");

                var productMajorPart = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileVersionInfo.ProductMajorPart;
                if (productMajorPart == 14)
                {
                    i.Assembly("vs-commitizen.vs2015");
                }
                else if (productMajorPart == 15)
                {
                    i.Assembly("vs-commitizen.vs2017");
                }
                else if (productMajorPart >= 16)
                {
                    i.Assembly("vs-commitizen.vs2019");
                }

                i.WithDefaultConventions();
                i.LookForRegistries();
            });

            this.For<IUserSettings>().Use<UserSettings>();
            this.For<IServiceProvider>().Use(ServiceProvider.GlobalProvider);
        }
    }
}
