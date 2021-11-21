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

                switch (productMajorPart)
                {
                    case int v when v >= 17:
                        i.Assembly("vs-commitizen.vs2022");
                        break;
                    case int v when v >= 16:
                        i.Assembly("vs-commitizen.vs2019");
                        break;
                }

                i.WithDefaultConventions();
                i.LookForRegistries();
            });

            this.For<IUserSettings>().Use<UserSettings>();

#if !TESTS
            this.For<IServiceProvider>().Use(ServiceProvider.GlobalProvider);
#endif
        }
    }
}
