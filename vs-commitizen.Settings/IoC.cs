using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using StructureMap;
using System;

namespace vs_commitizen.Settings
{
    public static class IoC
    {
        static Lazy<Container> _container = new Lazy<Container>(() => new Container(c => c.AddRegistry<ExtensionRegistry>()), true);

        public static Container Container => _container.Value;

        public static T TryGetInstance<T>()
        {
            return Container.TryGetInstance<T>();
        }
        public static T GetInstance<T>()
        {
            return Container.GetInstance<T>();
        }
    }
}
