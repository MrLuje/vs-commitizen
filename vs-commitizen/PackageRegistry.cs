using StructureMap;
using vs_commitizen.Infrastructure;
using vs_commitizen.Settings;

namespace vs_commitizen
{
    public class PackageRegistry : Registry
    {
        public PackageRegistry()
        {
            this.For<IConfigFileProvider>().Use<ConfigFileProvider>().Singleton();
            this.For<IFileAccessor>().Use<FileAccessor>().Singleton();
        }
    }
}
