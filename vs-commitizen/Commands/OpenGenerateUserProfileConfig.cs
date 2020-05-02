using System;
using System.ComponentModel.Design;
using System.IO;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using vs_commitizen.Infrastructure;
using Task = System.Threading.Tasks.Task;

namespace vs_commitizen.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class OpenGenerateUserProfileConfig
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 256;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = PackageGuids.OpenGenerateUserProfileConfigCmdSet;

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGenerateUserProfileConfig"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private OpenGenerateUserProfileConfig(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static OpenGenerateUserProfileConfig Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in Command1's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new OpenGenerateUserProfileConfig(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            _ = ProcessAsync();

        }

        private async Task ProcessAsync()
        {
            await OpenFileInEditorAsync(ConfigFileProvider.ConfigPathUserProfile);
        }

        private async Task OpenFileInEditorAsync(string localConfigPath)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var dte = await this.ServiceProvider.GetServiceAsync(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE)) as DTE2;
            if (dte == null) return;

            dte.MainWindow.Activate();
            var newWindow = dte.ItemOperations.OpenFile(localConfigPath);
            newWindow.Activate();
        }
    }
}