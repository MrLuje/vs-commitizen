using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute;
using NSubstitute.Extensions;
using Shouldly;
using System.Threading.Tasks;
using vs_commitizen.Infrastructure;
using vs_commitizen.Settings;
using vs_commitizen.Tests.TestAttributes;
using vs_commitizen.ViewModels;
using Xunit;

namespace vs_commitizen.Tests
{
    public class OpenGenerateLocalConfigViewModelTests
    {
        OpenGenerateLocalConfigViewModel getSut(Fixture fixture, ConfigFileProvider configFileProvider, IFileAccessor fileAccessor, string configPath, bool fileExists)
        {
            configFileProvider.Configure().TryGetLocalConfigAsync().Returns(configPath);
            fileAccessor.Exists(Arg.Any<string>()).ReturnsForAnyArgs(fileExists);

            IoC.Container.EjectAllInstancesOf<IConfigFileProvider>();
            IoC.Container.Inject<IConfigFileProvider>(configFileProvider);

            IoC.Container.EjectAllInstancesOf<IFileAccessor>();
            IoC.Container.Inject<IFileAccessor>(fileAccessor);

            return fixture.Create<OpenGenerateLocalConfigViewModel>();
        }

        [Theory]
        [InlineTestConventions(true, "path to config")]
        [InlineTestConventions(false, null)]
        public async Task Command_Is_Enabled_Based_On_Solution_Loaded(
            bool solutionLoaded,
            string configPath,
            [Frozen]IFileAccessor fileAccessor,
            [Frozen][Substitute] ConfigFileProvider configFileProvider,
            Fixture fixture
            )
        {
            var sut = getSut(fixture, configFileProvider, fileAccessor, configPath, solutionLoaded);

            var expectedEnabledState = solutionLoaded;
            (await sut.IsCommandEnabledAsync()).ShouldBe(expectedEnabledState);
        }

        [Theory]
        [InlineTestConventions(true, "path to config")]
        public async Task Execute_With_Existing_Config_Opens_The_File(
            bool solutionLoaded,
            string configPath,
            [Frozen]IFileAccessor fileAccessor,
            [Frozen][Substitute] IPopupManager popupManager,
            [Frozen][Substitute] ConfigFileProvider configFileProvider,
            Fixture fixture
            )
        {
            // Arrange
            var sut = getSut(fixture, configFileProvider, fileAccessor, configPath, solutionLoaded);
            var called = false;

            // Act
            await sut.ExecuteAsync(s =>
            {
                called = true;
                return Task.CompletedTask;
            });

            // Assert
            called.ShouldBeTrue();
            popupManager.DidNotReceiveWithAnyArgs().Confirm(Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineTestConventions(false, "path to config")]
        public async Task Execute_With_NonExisting_Config_Asks_To_Create_It(
            bool solutionLoaded,
            string configPath,
            [Frozen]IFileAccessor fileAccessor,
            [Frozen][Substitute] IPopupManager popupManager,
            [Frozen][Substitute] ConfigFileProvider configFileProvider,
            Fixture fixture
            )
        {
            // Arrange
            IoC.Container.Inject(popupManager);
            var sut = getSut(fixture, configFileProvider, fileAccessor, configPath, solutionLoaded);
            var called = false;

            // Act
            await sut.ExecuteAsync(s =>
            {
                called = true;
                return Task.CompletedTask;
            });

            // Assert
            called.ShouldBeFalse();
            popupManager.Received().Confirm(Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory]
        [InlineTestConventions(false, "path to config", true)]
        [InlineTestConventions(false, "path to config", false)]
        public async Task Response_To_Popup_Should_Open_File(
            bool solutionLoaded,
            string configPath,
            bool userWantsToCreateFile,
            [Frozen]IFileAccessor fileAccessor,
            [Frozen][Substitute] IPopupManager popupManager,
            [Frozen][Substitute] ConfigFileProvider configFileProvider,
            Fixture fixture
            )
        {
            // Arrange
            popupManager.Configure().Confirm(Arg.Any<string>(), Arg.Any<string>()).Returns(userWantsToCreateFile);
            IoC.Container.Inject(popupManager);
            var sut = getSut(fixture, configFileProvider, fileAccessor, configPath, solutionLoaded);
            var called = false;

            // Act
            await sut.ExecuteAsync(s =>
            {
                called = true;
                return Task.CompletedTask;
            });

            // Assert
            var expectedResult = userWantsToCreateFile;
            called.ShouldBe(expectedResult);
            popupManager.Received().Confirm(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}