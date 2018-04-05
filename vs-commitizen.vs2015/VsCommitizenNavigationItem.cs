using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using System;
using System.ComponentModel.Composition;
using System.Drawing;
using vs_commitizen.vs;

namespace vs_commitizen.vs2015
{
    [TeamExplorerNavigationItem(NavigationItemId, 310, TargetPageId = VsCommitizenPage.PageId)]
    public class VsCommitizenNavigationItem : TeamExplorerBaseNavigationItem
    {
        public const string NavigationItemId = "E3396357-5CFC-47A4-AECA-E52C894EBBDD";

        private readonly IGitExt gitService;

        [ImportingConstructor]
        public VsCommitizenNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.Text = "VsCommitizen";
            this.Image = VSPackage.Git_icon_svg;
            this.ArgbColor = Color.Red.ToArgb();
            this.IsVisible = false;

            gitService = GetService<IGitExt>();
            gitService.PropertyChanged += GitService_PropertyChanged;
        }

        private void GitService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.UpdateIsVisible();
        }

        private void UpdateIsVisible()
        {
            this.IsVisible = this.gitService?.ActiveRepositories.Count > 0;
        }

        public override void Execute()
        {
            var teamExplorer = GetService<ITeamExplorer>();
            teamExplorer?.NavigateToPage(Guid.Parse(VsCommitizenPage.PageId), null);
        }

        public override void Invalidate()
        {
            this.UpdateIsVisible();
        }
    }
}
