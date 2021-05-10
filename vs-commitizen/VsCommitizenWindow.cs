using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using vs_commitizen.Settings;

namespace vs_commitizen
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("b1cf150e-823d-43a0-9077-070e056b888e")]
    public class VsCommitizenWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VsCommitizenWindow"/> class.
        /// </summary>
        public VsCommitizenWindow() : base(null)
        {
            Caption = "VsCommitizen";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            var vsCommitizenView = IoC.GetInstance<IVsCommitizenView>();
            vsCommitizenView.SetTeamExplorerMode(false);

            base.Content = vsCommitizenView;
        }
    }
}
