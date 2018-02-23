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
    [TeamExplorerSection(VsCommitizenSection.SectionId, TeamExplorerPageIds.GitChanges, 30)]
    public class VsCommitizenSection : TeamExplorerBaseSection
    {
        private const string SectionId = "50948F33-9223-4E8C-A8A5-37A6225AE4E2";

        public VsCommitizenView CommitizenSection => this.SectionContent as VsCommitizenView;

        public VsCommitizenSection()
        {
            this.Title = "Vs Commitizen";
            this.IsVisible = true;
            this.IsExpanded = false;
            this.IsBusy = false;
            this.SectionContent = new VsCommitizenView();
            //this.View.ParentSection = this;

            this.PropertyChanged += LazyAgileCoderSection_OnPropertyChanged;
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

            var commitButton = view.FindName("commitButton") as System.Windows.Controls.Button;
            var commitGrid = commitButton.Parent as Grid;

            var commitCzButton = new Button();
            commitCzButton.Content = "CommitCz";
            commitCzButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            commitCzButton.Margin = new System.Windows.Thickness(12, 0, 0, 0);
            commitCzButton.Click += (s, re) => this.IsExpanded = true;
            commitGrid.Children.Add(commitCzButton);
            Grid.SetColumn(commitCzButton, 1);
        }

        private void TeamExplorerPageBasePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var page = (TeamExplorerPageBase)sender;

            // Wait for section to be not busy
            if (page.IsBusy)
                return;

            //this.View.Loaded(sender, null);
        }

        private void LazyAgileCoderSection_OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            //if (propertyChangedEventArgs.PropertyName == "IsExpanded")
            //{
            //    Settings.Default.ExpandAgileCoderSection = this.IsExpanded;
            //    Settings.Default.Save();
            //}
        }

        public void ShowWarning(string message)
        {
            ShowNotification(message, NotificationType.Warning);
        }

        /// <summary>
        /// Get the view.
        /// </summary>
        protected VsCommitizenView View
        {
            get { return this.SectionContent as VsCommitizenView; }
        }

        public void ForceRefresh()
        {
            var teamExplorer = GetService<ITeamExplorer>();
            var page = teamExplorer?.NavigateToPage(new Guid(TeamExplorerPageIds.PendingChanges), null);
            page?.Refresh();
        }
    }
}