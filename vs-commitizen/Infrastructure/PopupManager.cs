using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace vs_commitizen.Infrastructure
{

    public interface IPopupManager
    {
        void Show(string message);
        void Show(string message, string title);
        bool Confirm(string message, string title);
    }

    public class PopupManager : IPopupManager
    {
        private readonly IServiceProvider serviceProvider;

        public PopupManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public virtual void Show(string message)
        {
            VsShellUtilities.ShowMessageBox(serviceProvider, message, string.Empty, OLEMSGICON.OLEMSGICON_WARNING, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        public virtual void Show(string message, string title)
        {
            VsShellUtilities.ShowMessageBox(serviceProvider, message, title, OLEMSGICON.OLEMSGICON_WARNING, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
        }

        public virtual bool Confirm(string message, string title)
        {
            var res = VsShellUtilities.ShowMessageBox(serviceProvider, message, title, OLEMSGICON.OLEMSGICON_QUERY, OLEMSGBUTTON.OLEMSGBUTTON_YESNO, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            return res == (int)VSConstants.MessageBoxResult.IDYES;
        }
    }
}
