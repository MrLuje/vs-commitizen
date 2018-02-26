using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
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

        private static readonly Guid TfsProviderGuid = new Guid("4CA58AB2-18FA-4F8D-95D4-32DDF27D184C");
        private static readonly Guid GitProviderGuid = new Guid("11b8e6d7-c08b-4385-b321-321078cdd1f8");
        private IVsGetScciProviderInterface vsRegisterScciProvider;

        [ImportingConstructor]
        public VsCommitizenNavigationItem([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.Text = "VsCommitizen";
            this.Image = VSPackage.Git_icon_svg;
            this.ArgbColor = Color.Red.ToArgb();
        }

        public override void Execute()
        {
            var teamExplorer = GetService<ITeamExplorer>();
            teamExplorer?.NavigateToPage(Guid.Parse(VsCommitizenPage.PageId), null);
        }

        public override void Invalidate()
        {
            base.Invalidate();

            var currentProviderGuid = Guid.Empty;
#pragma warning disable VSTHRD010 // Use VS services from UI thread
            vsRegisterScciProvider = vsRegisterScciProvider ?? GetService<IVsRegisterScciProvider>() as IVsGetScciProviderInterface;
            vsRegisterScciProvider?.GetSourceControlProviderID(out currentProviderGuid);
#pragma warning restore VSTHRD010 // Use VS services from UI thread

            this.IsVisible = currentProviderGuid == GitProviderGuid;
        }
    }
}
