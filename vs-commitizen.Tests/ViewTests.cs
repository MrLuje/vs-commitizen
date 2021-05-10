using Shouldly;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using vs_commitizen.Settings;
using vs_commitizen.vs;
using vs_commitizen.vs.Settings;
using Xunit;

namespace vs_commitizen.Tests
{
    public class ViewTests
    {
        [WpfFact]
        public async Task Should_Display_CommitizenViewAsync()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        [WpfTheory]
        [InlineData("btnCommit")]
        [InlineData("btnProceed")]
        public async Task Buttons_Are_Visible_By_Default_In_TeamExplorer(string expectedBtnId)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();

            var btn = (Button)LogicalTreeHelper.FindLogicalNode(mainWindow, expectedBtnId);
            btn.Visibility.ShouldBe(Visibility.Visible);
        }

        [WpfTheory]
        [InlineData("btnReset")]
        [InlineData("btnCopy")]
        public async Task Buttons_Are_Hidden_By_Default_In_TeamExplorer(string expectedBtnId)
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();

            var btn = (Button)LogicalTreeHelper.FindLogicalNode(mainWindow, expectedBtnId);
            btn.IsVisible.ShouldBeFalse();
        }
        
        [WpfTheory]
        [InlineData("btnCommit")]
        [InlineData("btnProceed")]
        public async Task Buttons_Are_Hidden_In_Window_Mode(string expectedBtnId)
        {
            IoC.Container.Inject<IUserSettings>(new MainWindow.DummyUserSettings());
            IoC.Container.Inject<IConfigFileProvider>(new MainWindow.DummyConfigFileProvider());
            IoC.Container.Inject<IVsCommitizenView>(new VsCommitizenView());

            var mainWindow = new VsCommitizenWindow();
            var content = (DependencyObject)mainWindow.Content;

            var btn = (Button)LogicalTreeHelper.FindLogicalNode(content, expectedBtnId);
            btn.IsVisible.ShouldBeFalse();
        }
        
        [WpfTheory]
        [InlineData("panelToolWindow")]
        public async Task Buttons_Are_Visible_In_Window_Mode(string expectedBtnId)
        {
            IoC.Container.Inject<IUserSettings>(new MainWindow.DummyUserSettings());
            IoC.Container.Inject<IConfigFileProvider>(new MainWindow.DummyConfigFileProvider());
            IoC.Container.Inject<IVsCommitizenView>(new VsCommitizenView());

            var mainWindow = new VsCommitizenWindow();
            var content = (DependencyObject)mainWindow.Content;

            var stackPanel = (StackPanel)LogicalTreeHelper.FindLogicalNode(content, expectedBtnId);
            stackPanel.Visibility.ShouldBe(Visibility.Visible);
        }
    }
}
