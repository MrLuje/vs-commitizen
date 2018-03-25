using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs_commitizen.vs.Settings
{
    public interface IUserSettings
    {
        int MaxLineLength { get; set; }
        //void Save();
    }

    [Export(typeof(IUserSettings))]
    public class UserSettings : SettingsManagerBase, IUserSettings
    {
        private const string SettingsRoot = "vs-commitizen";

        public UserSettings() : this(ServiceProvider.GlobalProvider)
        {
        }

        public UserSettings(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [global::System.Configuration.DefaultSettingValueAttribute("80")]
        public int MaxLineLength
        {
            get
            {
                return ReadInt32(SettingsRoot, nameof(this.MaxLineLength), 80);
            }
            set
            {
                WriteInt32(SettingsRoot, nameof(this.MaxLineLength), value);
            }
        }
    }

    public abstract class SettingsManagerBase
    {
        private readonly Lazy<SettingsManager> _settingsManager;

        protected SettingsManagerBase(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _settingsManager = new Lazy<SettingsManager>(() => new ShellSettingsManager(serviceProvider));
        }

        protected bool ReadBool(string settingsRoot, string property, bool defaultValue = false)
        {
            var userSettingsStore = _settingsManager.Value.GetReadOnlySettingsStore(SettingsScope.UserSettings);
            if (userSettingsStore.CollectionExists(settingsRoot))
            {
                return userSettingsStore.GetBoolean(settingsRoot, property, defaultValue);
            }

            return defaultValue;
        }

        protected void WriteBool(string settingsRoot, string property, bool value)
        {
            var userSettingsStore = GetWritableSettingsStore(settingsRoot);
            userSettingsStore.SetBoolean(settingsRoot, property, value);
        }

        protected int ReadInt32(string settingsRoot, string property, int defaultValue = 0)
        {
            var userSettingsStore = _settingsManager.Value.GetReadOnlySettingsStore(SettingsScope.UserSettings);
            if (userSettingsStore.CollectionExists(settingsRoot))
            {
                return userSettingsStore.GetInt32(settingsRoot, property, defaultValue);
            }

            return defaultValue;
        }

        protected void WriteInt32(string settingsRoot, string property, int value)
        {
            var userSettingsStore = GetWritableSettingsStore(settingsRoot);
            userSettingsStore.SetInt32(settingsRoot, property, value);
        }

        protected string ReadString(string settingsRoot, string property, string defaultValue = "")
        {
            var userSettingsStore = _settingsManager.Value.GetReadOnlySettingsStore(SettingsScope.UserSettings);
            if (userSettingsStore.CollectionExists(settingsRoot))
            {
                return userSettingsStore.GetString(settingsRoot, property, defaultValue);
            }

            return defaultValue;
        }

        protected string[] ReadStrings(string settingsRoot, string[] properties, string defaultValue = "")
        {
            var userSettingsStore = _settingsManager.Value.GetReadOnlySettingsStore(SettingsScope.UserSettings);
            if (userSettingsStore.CollectionExists(settingsRoot))
            {
                string[] values = new string[properties.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = userSettingsStore.GetString(settingsRoot, properties[i], defaultValue);
                }
                return values;
            }

            return null;
        }

        protected bool DeleteProperty(string settingsRoot, string property)
        {
            var userSettingsStore = GetWritableSettingsStore(settingsRoot);
            return userSettingsStore.DeleteProperty(settingsRoot, property);
        }

        protected void WriteStrings(string settingsRoot, string[] properties, string[] values)
        {
            Debug.Assert(properties.Length == values.Length);

            var userSettingsStore = GetWritableSettingsStore(settingsRoot);
            for (int i = 0; i < properties.Length; i++)
            {
                userSettingsStore.SetString(settingsRoot, properties[i], values[i]);
            }
        }

        protected void WriteString(string settingsRoot, string property, string value)
        {
            var userSettingsStore = GetWritableSettingsStore(settingsRoot);

            userSettingsStore.SetString(settingsRoot, property, value);
        }

        protected void ClearAllSettings(string settingsRoot)
        {
            var userSettingsStore = _settingsManager.Value.GetWritableSettingsStore(SettingsScope.UserSettings);
            userSettingsStore.DeleteCollection(settingsRoot);
        }

        private WritableSettingsStore GetWritableSettingsStore(string settingsRoot)
        {
            var userSettingsStore = _settingsManager.Value.GetWritableSettingsStore(SettingsScope.UserSettings);
            if (!userSettingsStore.CollectionExists(settingsRoot))
            {
                userSettingsStore.CreateCollection(settingsRoot);
            }
            return userSettingsStore;
        }
    }
}

public static class SettingsExtension
{
    public static WritableSettingsStore GetWritableSettingsStore(this IServiceProvider vsServiceProvider)
    {
        var shellSettingsManager = new ShellSettingsManager(vsServiceProvider);
        return shellSettingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
    }
}
