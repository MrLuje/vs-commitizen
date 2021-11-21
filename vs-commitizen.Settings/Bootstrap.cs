using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace vs_commitizen.Settings
{
    public static class Bootstrap
    {
        public static void InitExtension(Package package)
        {
            IoC.Container.Configure(c =>
            {
                //c.AddRegistry<ExtensionRegistry>();
                c.ForSingletonOf<IVsPackage>().Use(package);
            });
        }
    }
}
