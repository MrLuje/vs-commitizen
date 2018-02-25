using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using System.ComponentModel;
using vs_commitizen.vs;

namespace vs_commitizen.vs2015
{
    [TeamExplorerSection(VsCommitizenSection.SectionId, VsCommitizenPage.PageId, 30)]
    public class VsCommitizenSection : TeamExplorerBaseSection
    {
        private const string SectionId = "50948F33-9223-4E8C-A8A5-37A6225AE4E2";
        
        public VsCommitizenView CommitizenSection => this.SectionContent as VsCommitizenView;

        public VsCommitizenSection()
        {
            this.Title = "Details";
            this.IsVisible = true;
            this.IsExpanded = true;
            this.IsBusy = false;
            this.SectionContent = new VsCommitizenView();

            this.PropertyChanged += Section_OnPropertyChanged;
        }

        public override void Loaded(object sender, SectionLoadedEventArgs e)
        {
            var teamExplorer = GetService<ITeamExplorer>();
            var page = teamExplorer.CurrentPage;
            page.PropertyChanged += TeamExplorerPageBasePropertyChanged;
        }

        private void TeamExplorerPageBasePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var page = (TeamExplorerPageBase)sender;

            // Wait for section to be not busy
            if (page.IsBusy)
                return;

            //this.View.Loaded(sender, null);
        }

        private void Section_OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
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
    }
}