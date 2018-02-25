using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vs_commitizen.vs;
using vs_commitizen.vs2017;

namespace vs_commitizen.vs2015
{
    [TeamExplorerNavigationItem(NavigationItemId, 310, TargetPageId = VsCommitizenPage.PageId)]
    public class VsCommitizenNavigationItem : TeamExplorerBaseNavigationItem
    {
        public const string NavigationItemId = "E3396357-5CFC-47A4-AECA-E52C894EBBDD";

        [ImportingConstructor]
        public VsCommitizenNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.Text = "VsCommitizen";
            this.Image = VSPackage.Git_icon_svg;
        }

        public override void Execute()
        {
            var teamExplorer = GetService<ITeamExplorer>();
            teamExplorer?.NavigateToPage(Guid.Parse(VsCommitizenPage.PageId), null);
        }
    }
}
