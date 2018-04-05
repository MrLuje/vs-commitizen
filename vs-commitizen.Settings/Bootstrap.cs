using Microsoft.VisualStudio.Shell;

namespace vs_commitizen.Settings
{
    public static class Bootstrap
    {
        public static void InitExtension(Package package)
        {
            IoC.Container.Configure(c =>
            {
                c.AddRegistry<ExtensionRegistry>();
                c.ForSingletonOf<Package>().Use(package);
            });
        }
    }
}
