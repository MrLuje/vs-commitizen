using System;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace vs_commitizen.Settings
{
    public static class OutputPaneWriter
    {
        private static Guid panelGuild = new Guid("5BB96421-E33D-40DA-9E8D-C657B7E94C70");

        static IVsOutputWindowPane GetPane()
        {
            var outputWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            if (outputWindow == null) return null;

            IVsOutputWindowPane pane;

            if (ErrorHandler.Failed(outputWindow.GetPane(ref panelGuild, out pane)) &&
                (ErrorHandler.Succeeded(outputWindow.CreatePane(ref panelGuild, "vs-commitizen", 1, 1))))
            {
                outputWindow.GetPane(ref panelGuild, out pane);
            }

            ErrorHandler.ThrowOnFailure(pane.Activate());

            return pane;
        }

        public static void Print(string message)
        {
            var pane = GetPane();

            ErrorHandler.ThrowOnFailure(pane.OutputString(message + Environment.NewLine));
        }

        public static void Print(string str, Exception ex)
        {
            var pane = GetPane();

            ErrorHandler.ThrowOnFailure(pane.OutputString(str + "\r\n\r\n" + ex));
        }
    }
}