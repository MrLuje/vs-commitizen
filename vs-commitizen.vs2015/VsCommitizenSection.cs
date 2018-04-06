using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.MVVM;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Media;
using vs_commitizen.Settings;
using vs_commitizen.vs;
using vs_commitizen.vs.Models;
using vs_commitizen.vs.Settings;

namespace vs_commitizen.vs2015
{
    [TeamExplorerSection(VsCommitizenSection.SectionId, VsCommitizenPage.PageId, 30)]
    public class VsCommitizenSection : TeamExplorerBaseSection, ITeamExplorerSectionCommandProvider
    {
        private const string SectionId = "50948F33-9223-4E8C-A8A5-37A6225AE4E2";
        private TeamExplorerSectionCommand teamExplorerSectionCommand;

        public VsCommitizenView CommitizenSection => this.SectionContent as VsCommitizenView;

        public ITeamExplorerSectionCommand[] SectionCommands => new ITeamExplorerSectionCommand[] { teamExplorerSectionCommand };

        public VsCommitizenSection()
        {
            this.Title = "Details";
            this.IsVisible = true;
            this.IsExpanded = true;
            this.IsBusy = false;
            this.SectionContent = new VsCommitizenView();
        }

        private void ExecuteOpenSettings()
        {
            var package = IoC.GetInstance<IVsPackage>();
            var showOptionPageMethod = package.GetType().GetMethod("ShowOptionPage", BindingFlags.Public | BindingFlags.Instance);
            showOptionPageMethod.Invoke(package, new[] { typeof(SettingsGeneral) });
        }

        public override void Initialize(object sender, SectionInitializeEventArgs e)
        {
            base.Initialize(sender, e);

            var openSettingsCommand = new RelayCommand(ExecuteOpenSettings);
            this.teamExplorerSectionCommand = new TeamExplorerSectionCommand(openSettingsCommand, "Open options", WpfUtil.SharedResources["Home_SettingsBrush"] as DrawingBrush);
        }

        public override void Loaded(object sender, SectionLoadedEventArgs e)
        {
            var teamExplorer = GetService<ITeamExplorer>();
            var page = teamExplorer.CurrentPage as TeamExplorerBasePage;

            this.CommitizenSection.ViewModel.ProceedExecuted += (s, autoCommit) =>
            {
                AddNavigationValue(NavigationDataType.CommitData, new NavigationCommitModel
                {
                    AutoCommit = autoCommit,
                    Comment = CommitizenSection.ViewModel.GetComment()
                });

                teamExplorer.NavigateToPage(Guid.Parse(TeamExplorerPageIds.GitChanges), null);
            };
        }
    }
}