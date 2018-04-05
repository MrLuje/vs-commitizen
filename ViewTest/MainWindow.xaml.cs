using System.Windows;
using vs_commitizen.Settings;
using vs_commitizen.vs.Settings;

namespace ViewTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            IoC.Container.Inject<IUserSettings>(new DummyUserSettings());

            InitializeComponent();
        }

        public class DummyUserSettings : IUserSettings
        {
            public int MaxLineLength { get; set; } = 80;
        }
    }
}
