using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using vs_commitizen.Commands;
using vs_commitizen.Settings;
using vs_commitizen.vs.Settings;

namespace vs_commitizen
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuids.guidVsCommitizenPackageString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(SettingsGeneral), "vs-commitizen", "General", 101, 106, true)]

#pragma warning disable VSSDK004 // Use BackgroundLoad flag in ProvideAutoLoad attribute for asynchronous auto load.
    [ProvideAutoLoad(UIContextGuid, PackageAutoLoadFlags.BackgroundLoad)]
#pragma warning restore VSSDK004 // Use BackgroundLoad flag in ProvideAutoLoad attribute for asynchronous auto load.
    [ProvideUIContextRule(UIContextGuid,
        name: "Test auto load",
        expression: "(RepositoryOpen | SingleProject | MultipleProjects)",
        termNames: new[] { "RepositoryOpen", "SingleProject", "MultipleProjects" },
        termValues: new[] { VSConstants.UICONTEXT.RepositoryOpen_string, VSConstants.UICONTEXT.SolutionHasSingleProject_string, VSConstants.UICONTEXT.SolutionHasMultipleProjects_string })]

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class VsCommitizenPackage : AsyncPackage
    {
        public const string UIContextGuid = "D08D84F8-03D8-418E-A03A-1015ADD01A6F";


        /// <summary>
        /// Initializes a new instance of the <see cref="VsCommitizenPackage"/> class.
        /// </summary>
        public VsCommitizenPackage()
        {
            // Inside this method you can place any initialization code that does not require
            // any Visual Studio service because at this point the package object is created but
            // not sited yet inside Visual Studio environment. The place to do all the other
            // initialization is the Initialize method.

            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, $"Entering constructor for: {0}", this.ToString()));
        }

        #region Package Members

        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            Bootstrap.InitExtension(this);

            await OpenGenerateUserProfileConfig.InitializeAsync(this);
            await OpenGenerateLocalConfig.InitializeAsync(this);
            await base.InitializeAsync(cancellationToken, progress);
        }

        #endregion
    }
}
