using Microsoft.TeamFoundation.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vs_commitizen.vs;

namespace vs_commitizen.vs2015
{
    [TeamExplorerPage(PageId)]
    public class VsCommitizenPage : TeamExplorerBasePage
    {
        public const string PageId = "B6308048-63A0-4234-9E63-D2D72C4F878E";

        public VsCommitizenPage()
        {
            this.Title = "VsCommitizen";
        }
    }
}
