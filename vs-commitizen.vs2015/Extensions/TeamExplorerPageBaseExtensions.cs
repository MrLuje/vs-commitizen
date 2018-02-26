using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs_commitizen.vs.Extensions
{
    public static class TeamExplorerPageBaseExtensions
    {
        public static T GetExtensibilityService<T>(this TeamExplorerPageBase page)
        {
            return (T)page.GetExtensibilityService(typeof(T));
        }
    }
}
