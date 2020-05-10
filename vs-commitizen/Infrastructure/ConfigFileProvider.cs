using EnvDTE;
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
using System.Runtime.Caching;
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
        private readonly IServiceProvider serviceProvider;
        private readonly IPopupManager popupManager;

        private SolutionEvents solutionEvents;
        private bool init;
        private static readonly Object @lock = new Object();
        private MemoryCache cache = new MemoryCache("commitTypes");

        public static string ConfigPathUserProfile => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), EXTENSION_FOLDER_NAME, CONFIGFILE_NAME);

        public ConfigFileProvider(IServiceProvider serviceProvider, IFileAccessor fileAccessor, IPopupManager popupManager)
        {
            this.fileAccessor = fileAccessor;
            this.serviceProvider = serviceProvider;
            this.popupManager = popupManager;
        }

        public async Task<IList<T>> GetCommitTypesAsync<T>() where T : class
        {
            string cacheKey = await GetCacheKeyAsync();
            lock (@lock)
            {
                var commitTypes = cache.GetCacheItem(cacheKey);
                if (commitTypes != null) return (IList<T>)commitTypes.Value;
            }

            try
            {
                var configStr = await GetConfigAsync();
                T[] commitTypes = JsonConvert.DeserializeObject<T[]>(configStr).Where(c => !string.IsNullOrEmpty(c.ToString())).ToArray();
                lock (@lock)
                {
                    cache.Set(cacheKey, commitTypes, new CacheItemPolicy());
                }
                return commitTypes;
            }
            catch (InvalidConfigurationFileException ex)
            {
                popupManager.Show($"Failed to read configuration file, using default values.{Environment.NewLine}{Environment.NewLine}{ex}", "Error");
                throw;
            }
        }

        private async Task<string> GetCacheKeyAsync()
        {
            var (ok, path) = await this.GetLocalPathAsync();
            if (ok)
                return string.Concat("commitTypes-", path.Normalize());
            return "commitTypes";
        }

        private async Task<(bool isValid, string content)> TryGetValidConfigFileAsync(string path)
        {
            var content = await fileAccessor.ReadFileAsync(path);

            try
            {
                string types = JObject.Parse(content).SelectToken("types").ToString();
                return (true, types);
            }
            catch (JsonReaderException)
            {
                throw new InvalidConfigurationFileException(path);
            }
            catch (Exception)
            {
                return (false, null);
            }
        }

        internal protected virtual async Task<(bool isLoaded, string path)> GetCurrentSolutionAsync()
        {
            var asyncServiceProvider = serviceProvider.GetService(typeof(SAsyncServiceProvider)) as IAsyncServiceProvider;
            var dte = await asyncServiceProvider?.GetServiceAsync(typeof(SDTE)) as DTE2;

            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            return ((dte?.Solution?.IsOpen).GetValueOrDefault(false), dte?.Solution.FullName);
        }

        internal protected virtual async Task<(bool isLoaded, string path)> GetLocalPathAsync()
        {
            var repository = IoC.GetInstance<IRepository>();
            return (repository != null, repository?.RepositoryPath);
        }

        public async Task<string> TryGetLocalConfigAsync()
        {
            var (isRepositoryLoaded, repositoryPath) = await GetLocalPathAsync(); // await GetCurrentSolutionAsync();
            if (isRepositoryLoaded)
            {
                return Path.Combine(repositoryPath, CONFIGFILE_NAME);
            }

            return string.Empty;
        }

        async Task<string> GetConfigAsync()
        {
            await SubscribeToSolutionEventsAsync();

            var localConfig = await TryGetLocalConfigAsync();

            if (!string.IsNullOrWhiteSpace(localConfig))
            {
                if (fileAccessor.Exists(localConfig))
                {
                    var (isValid, content) = await TryGetValidConfigFileAsync(localConfig);
                    if (isValid)
                        return content;
                }
            }

            var pathToFolder = Path.GetDirectoryName(ConfigPathUserProfile);
            Directory.CreateDirectory(pathToFolder);

            if (fileAccessor.Exists(ConfigPathUserProfile))
            {
                var (isValid, content) = await TryGetValidConfigFileAsync(ConfigPathUserProfile);
                if (isValid)
                    return content;
            }

            return await GenerateDefaultConfigFileAsync(ConfigPathUserProfile);
        }

        internal protected virtual async System.Threading.Tasks.Task SubscribeToSolutionEventsAsync()
        {
            if (!this.init)
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                var asyncServiceProvider = serviceProvider.GetService(typeof(SAsyncServiceProvider)) as IAsyncServiceProvider;
                var dte = await asyncServiceProvider?.GetServiceAsync(typeof(SDTE)) as DTE2;

                this.solutionEvents = dte.Events.SolutionEvents;
                this.solutionEvents.BeforeClosing += () =>
                {
                    lock (@lock)
                    {
                        this.cache = new MemoryCache("commitTypes");
                    }
                };

                this.init = true;
            }
        }

        private async Task<string> GenerateDefaultConfigFileAsync(string configFileInUserSettings)
        {
            using (var fileStream = fileAccessor.CreateText(configFileInUserSettings))
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
