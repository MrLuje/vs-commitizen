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
            Container = new Container();
        }

        public static T GetInstance<T>()
        {
            return Container.GetInstance<T>();
        }
    }
}
