using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using vs_commitizen.Settings;
using vs_commitizen.vs.Settings;

namespace vs_commitizen.Tests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            IoC.Container.Inject<IUserSettings>(new DummyUserSettings());
            IoC.Container.Inject<IConfigFileProvider>(new DummyConfigFileProvider());

            InitializeComponent();
        }

        internal class DummyUserSettings : IUserSettings
        {
            public int MaxLineLength { get; set; } = 80;
        }

        internal class DummyConfigFileProvider : IConfigFileProvider
        {
            public Task<IList<T>> GetCommitTypesAsync<T>() where T : class
            {
                throw new NotImplementedException();
            }
        }
    }
}
