using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using Microsoft.VisualStudio.Threading;
using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Threading.Tasks;
using vs_commitizen.Settings;
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

            gitService = GetService<IGitExt>();
            gitService.PropertyChanged += GitService_PropertyChanged;

            this.IsVisible = gitService.ActiveRepositories.Count > 0;
        }

        private async void GitService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            await this.UpdateIsVisibleAsync();
        }

        private async System.Threading.Tasks.Task UpdateIsVisibleAsync()
        {
            if (VsTaskLibraryHelper.ServiceInstance == null)
                return;

            var joinableTaskFactory = ThreadHelper.JoinableTaskFactory;
            if (joinableTaskFactory == null)
                return;

            try
            {
                await joinableTaskFactory.SwitchToMainThreadAsync();
                this.IsVisible = this.gitService?.ActiveRepositories.Count > 0;
                await TaskScheduler.Default;
            }
            catch (Exception ex)
            {
                OutputPaneWriter.Print($"UpdateIsVisibleAsync: {ex}");
            }
        }

        public override void Execute()
        {
            var teamExplorer = GetService<ITeamExplorer>();
            teamExplorer?.NavigateToPage(Guid.Parse(VsCommitizenPage.PageId), null);
        }
    }
}
