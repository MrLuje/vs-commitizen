using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer.Framework;
using Microsoft.VisualStudio.ProjectSystem.VS.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using vs_commitizen.vs;
using vs_commitizen.vs.Models;

namespace vs_commitizen.vs2015
{
    /// <summary>
    /// Add a CommitCz button on the GitChanges page
    /// </summary>
    [TeamExplorerSection(GitChangeSection.SectionId, TeamExplorerPageIds.GitChanges, 30)]
    public class GitChangeSection : TeamExplorerBaseSection
    {
        private const string SectionId = "18850AAF-B79F-4522-A7E5-93843AA153DA";
        private Button commitButton;

        public VsCommitizenView CommitizenSection => this.SectionContent as VsCommitizenView;

        public GitChangeSection()
        {
            this.IsVisible = false;
            this.IsExpanded = false;
            this.IsBusy = false;
        }

        public override void Loaded(object sender, SectionLoadedEventArgs e)
        {
            var teamExplorer = GetService<ITeamExplorer>();
            var page = teamExplorer.CurrentPage as TeamExplorerPageBase;
            page.PropertyChanged += TeamExplorerPageBasePropertyChanged;

            var service = GetService<TeamExplorerViewModel>();
            var pages = new List<ITeamExplorerPage>();
            pages.Add(service.CurrentPage);
            pages.AddRange(service.UndockedPages);

            var commitPage = pages.FirstOrDefault(p => p.GetId() == Guid.Parse(TeamExplorerPageIds.GitChanges));
            var view = commitPage.PageContent as UserControl;
            var labeledTextBox = view.FindName("commentTextBox") as LabeledTextBox;
            var commitTextBox = labeledTextBox.FindName("textBox") as TextBox;

            commitButton = view.FindName("commitButton") as Button;
            var commitGrid = commitButton.Parent as Grid;

            var commitCzButton = createCommitCzButton(teamExplorer);
            AppendButtonToPage(commitGrid, commitCzButton);
        }

        private void AppendButtonToPage(Grid commitGrid, Button commitCzButton)
        {
            if (isVs2019())
                commitGrid.Children.OfType<WrapPanel>().First().Children.Insert(0, commitCzButton);
            else
            {
                commitCzButton.Margin = new Thickness(12, 0, 0, 0);
                commitGrid.Children.Insert(1, commitCzButton);
                Grid.SetColumn(commitCzButton, 1);
            }
        }

        private static Button createCommitCzButton(ITeamExplorer teamExplorer)
        {
            var commitCzButton = new Button();
            commitCzButton.Content = "CommitCz";
            commitCzButton.HorizontalAlignment = HorizontalAlignment.Center;
            commitCzButton.Click += (s, re) => teamExplorer.NavigateToPage(Guid.Parse(VsCommitizenPage.PageId), null);
            return commitCzButton;
        }

        private bool isVs2019()
        {
            var vsAppId = GetService<IVsAppId>();
            vsAppId.GetProperty((int)VSAPropID.VSAPROPID_ProductSemanticVersion, out var semanticVersionObj);

            return (semanticVersionObj?.ToString().StartsWith("16.")).GetValueOrDefault(false);
        }

        private void TeamExplorerPageBasePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var page = (TeamExplorerPageBase)sender;

            // Wait for section to be not busy
            if (page.IsBusy)
                return;

            var commitData = PopNavigationValue<NavigationCommitModel>(NavigationDataType.CommitData);
            if (commitData == null) return;

            var model = page.Model;
            var commentProperty = model.GetType().GetProperty("Comment");

            commentProperty.SetValue(model, commitData.Comment);

            if (!commitData.AutoCommit) return;

            var changesExt = GetService<Microsoft.TeamFoundation.Git.Controls.Extensibility.IChangesExt2>();
            var hasPendingChanges = changesExt.IncludedChanges.Count > 0 || changesExt.UntrackedFiles.Count > 0;

            if (!hasPendingChanges) return;

            var peer = new ButtonAutomationPeer(commitButton);
            var invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
            invokeProv.Invoke();
        }
    }
}