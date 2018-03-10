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
        public async Task Should_Display_CommitizenView()
        {
            var t = await Task.Factory.StartNew(async () =>
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();

                await Task.Delay(1);
            }, new CancellationToken(), TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

            await t;
            t.IsCompleted.ShouldBe(true);
            t.Exception.ShouldBeNull();
        }
    }
}
