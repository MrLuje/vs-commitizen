using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs_commitizen.Settings
{
    public static class Bootstrap
    {
        public static void InitExtension()
        {
            IoC.Container.Configure(c => c.AddRegistry<ExtensionRegistry>());
        }
    }
}
