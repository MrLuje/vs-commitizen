using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using vs_commitizen.Settings;
using IAsyncServiceProvider = Microsoft.VisualStudio.Shell.IAsyncServiceProvider;

namespace vs_commitizen.Infrastructure
{
    public class ConfigFileProvider : IConfigFileProvider
    {
        private readonly IFileAccessor fileAccessor;
        private const string CONFIGFILE_NAME = ".commitizen.json";
        private const string EXTENSION_FOLDER_NAME = "vs-commitizen";
        private object CachedCommitTypes;
        private readonly IServiceProvider serviceProvider;
        private readonly IPopupManager popupManager;

        public ConfigFileProvider(IServiceProvider serviceProvider, IFileAccessor fileAccessor, IPopupManager popupManager)
        {
            this.fileAccessor = fileAccessor;
            this.serviceProvider = serviceProvider;
            this.popupManager = popupManager;
        }

        public async Task<IList<T>> GetCommitTypesAsync<T>() where T : class
        {
            if (CachedCommitTypes != null) return (IList<T>)CachedCommitTypes;

            try
            {
                var configStr = await GetConfigAsync();
                T[] commitTypes = JsonConvert.DeserializeObject<T[]>(configStr).Where(c => !string.IsNullOrEmpty(c.ToString())).ToArray();
                CachedCommitTypes = commitTypes;
                return commitTypes;
            }
            catch (Exception ex)
            {
                popupManager.Show($"Failed to read configuration file, using default values.{Environment.NewLine}{Environment.NewLine}{ex}", "Error");
                throw;
            }
        }

        private async Task<(bool isValid, string content)> TryGetValidConfigFileAsync(string path)
        {
            var content = await fileAccessor.ReadFileAsync(path);

            try
            {
                string types = JObject.Parse(content).SelectToken("types").ToString();
                return (true, types);
            }
            catch (Exception)
            {
                return (false, null);
            }
        }

        internal protected async Task<(bool isLoaded, string path)> GetCurrentSolutionAsync()
        {
            var asyncServiceProvider = serviceProvider.GetService(typeof(SAsyncServiceProvider)) as IAsyncServiceProvider;
            var dte = await asyncServiceProvider.GetServiceAsync(typeof(SDTE)) as DTE2;

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            return (dte?.Solution != null, dte?.Solution.FullName);
        }

        async Task<string> GetConfigAsync()
        {
            var (isSolutionLoaded, solutionPath) = await GetCurrentSolutionAsync();

            if (isSolutionLoaded)
            {
                var configFileInSolution = Path.Combine(Path.GetDirectoryName(solutionPath), CONFIGFILE_NAME);
                if (fileAccessor.Exists(configFileInSolution))
                {
                    var (isValid, content) = await TryGetValidConfigFileAsync(configFileInSolution);
                    if (isValid)
                        return content;
                }
            }

            var configFileInUserSettings = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), EXTENSION_FOLDER_NAME, CONFIGFILE_NAME);
            var pathToFolder = Path.GetDirectoryName(configFileInUserSettings);
            Directory.CreateDirectory(pathToFolder);

            if (fileAccessor.Exists(configFileInUserSettings))
            {
                var (isValid, content) = await TryGetValidConfigFileAsync(configFileInUserSettings);
                if (isValid)
                    return content;
            }

            return await GenerateDefaultConfigFileAsync(configFileInUserSettings);
        }

        private async Task<string> GenerateDefaultConfigFileAsync(string configFileInUserSettings)
        {
            using (var fileStream = File.CreateText(configFileInUserSettings))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = assembly.GetManifestResourceNames().Single(str => str.EndsWith("defaultConfigFile.json"));

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    var defaultConfigFileContent = await reader.ReadToEndAsync();
                    await fileStream.WriteAsync(defaultConfigFileContent);

                    return defaultConfigFileContent;
                }
            }
        }
    }
}
