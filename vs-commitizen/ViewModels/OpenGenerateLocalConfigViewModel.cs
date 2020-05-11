using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vs_commitizen.Infrastructure;
using vs_commitizen.Settings;

namespace vs_commitizen.ViewModels
{
    public class OpenGenerateLocalConfigViewModel
    {
        private readonly IConfigFileProvider configFileProvider;
        private readonly IFileAccessor fileAccessor;
        private readonly IPopupManager popupManager;

        public OpenGenerateLocalConfigViewModel()
        {
            configFileProvider = IoC.GetInstance<IConfigFileProvider>();
            fileAccessor = IoC.GetInstance<IFileAccessor>();
            popupManager = IoC.GetInstance<IPopupManager>();

        }

        public async Task<bool> IsCommandEnabledAsync()
        {
            var localConfigPath = await this.configFileProvider.TryGetLocalConfigAsync();

            return !string.IsNullOrWhiteSpace(localConfigPath);
        }

        public async Task ExecuteAsync(Func<string, Task> openFunc)
        {
            try
            {
                var localConfigPath = await this.configFileProvider.TryGetLocalConfigAsync();
                if (string.IsNullOrWhiteSpace(localConfigPath)) return;

                if (fileAccessor.Exists(localConfigPath))
                {
                    await openFunc(localConfigPath);
                }
                else
                {
                    if (popupManager.Confirm("There is no local configuration file yet, do you want to create it for this repository ?", "Init local config file"))
                    {
                        fileAccessor.CopyFile(ConfigFileProvider.ConfigPathUserProfile, localConfigPath);
                        await openFunc(localConfigPath);
                    }
                }
            }
            catch (Exception ex)
            {
                popupManager.Show(ex.ToString(), "An error occured");
            }
        }
    }
}
