using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;
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
    }
}
