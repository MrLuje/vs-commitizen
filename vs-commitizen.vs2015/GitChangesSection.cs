using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using vs_commitizen.vs;

namespace vs_commitizen.vs2015
{
    [TeamExplorerSection(GitChangeSection.SectionId, TeamExplorerPageIds.GitChanges, 30)]
    public class GitChangeSection : TeamExplorerBaseSection
    {
        private const string SectionId = "18850AAF-B79F-4522-A7E5-93843AA153DA";

        public VsCommitizenView CommitizenSection => this.SectionContent as VsCommitizenView;

        public GitChangeSection()
        {
            this.Title = "Commit";
            this.IsVisible = false;
            this.IsExpanded = false;
            this.IsBusy = false;
        }

        public override void Loaded(object sender, SectionLoadedEventArgs e)
        {
            var teamExplorer = GetService<ITeamExplorer>();
            var page = teamExplorer.CurrentPage as TeamExplorerPageBase;
            //page.PropertyChanged += TeamExplorerPageBasePropertyChanged;

            var service = GetService<TeamExplorerViewModel>();
            var pages = new List<ITeamExplorerPage>();
            pages.Add(service.CurrentPage);
            pages.AddRange(service.UndockedPages);

            var commitPage = pages.FirstOrDefault(p => p.GetId() == Guid.Parse(TeamExplorerPageIds.GitChanges));
            var view = commitPage.PageContent as UserControl;
            var labeledTextBox = view.FindName("commentTextBox") as LabeledTextBox;

            var commitButton = view.FindName("commitButton") as Button;
            var commitGrid = commitButton.Parent as Grid;

            var commitCzButton = new Button();
            commitCzButton.Content = "CommitCz";
            commitCzButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            commitCzButton.Margin = new System.Windows.Thickness(12, 0, 0, 0);
            commitCzButton.Click += (s, re) => teamExplorer.NavigateToPage(Guid.Parse(VsCommitizenPage.PageId), null);
            commitGrid.Children.Add(commitCzButton);

            // Place the button on the right of the Commit button
            Grid.SetColumn(commitCzButton, 1);
        }
    }
}