using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using System.ComponentModel;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using vs_commitizen.vs;
using vs_commitizen.vs.Models;

namespace vs_commitizen.vs2015
{
    public partial class GitChangeSection : TeamExplorerBaseSection
    {
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