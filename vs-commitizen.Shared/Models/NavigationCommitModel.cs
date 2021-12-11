using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs_commitizen.vs.Models
{
    public class NavigationCommitModel
    {
        /// <summary>
        /// Should automatically trigger the commit after navigating to the Changes page
        /// </summary>
        public bool AutoCommit { get; set; }

        /// <summary>
        /// Comment of the commit
        /// </summary>
        public string Comment { get; set; }
    }
}
