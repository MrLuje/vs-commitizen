using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using System.ComponentModel.Composition;
using vs_commitizen.Settings;

namespace vs_commitizen.vs.Settings
{
    public partial class SettingsGeneral : DialogPage, IUserSettings
    {
        IUserSettings UserSettings;

        public SettingsGeneral()
        {
            this.UserSettings = IoC.GetInstance<IUserSettings>();
        }

        [DisplayName("Max line length")]
        [Description("Maximum length of a commit description, if line is longer it will be split every x chars.")]
        [Category("General")]
        public int MaxLineLength
        {
            get { return UserSettings.MaxLineLength; }
            set { UserSettings.MaxLineLength = value; }
        }
    }
}
