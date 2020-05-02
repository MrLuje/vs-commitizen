using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute;
using NSubstitute.Extensions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using vs_commitizen.Infrastructure;
using vs_commitizen.Settings;
using vs_commitizen.Tests.TestAttributes;
using vs_commitizen.vs.ViewModels;
using Xunit;

namespace vs_commitizen.Tests
{
    public class CommitizenViewModelTests_CustomCommitType
    {
        CommitizenViewModel getSut(Fixture fixture, ConfigFileProvider configFileProvider, (bool, string) currentSolution)
        {
            configFileProvider.Configure().SubscribeToSolutionEventsAsync().Returns(Task.CompletedTask);
            configFileProvider.Configure().GetLocalPathAsync().Returns(currentSolution);
            fixture.Register<IConfigFileProvider>(() => configFileProvider);
            return fixture.Create<CommitizenViewModel>();
        }

        private static void SetupConfigFileInUserProfile(IFileAccessor fileAccessor, string userProfileContent, string inRepoConfigContent = null)
        {
            fileAccessor.Exists(Arg.Any<string>()).ReturnsForAnyArgs(inRepoConfigContent != null, true);

            if (string.IsNullOrWhiteSpace(inRepoConfigContent))
                fileAccessor.ReadFileAsync(Arg.Any<string>()).ReturnsForAnyArgs(userProfileContent);
            else
                fileAccessor.ReadFileAsync(Arg.Any<string>()).ReturnsForAnyArgs(inRepoConfigContent, userProfileContent);
            fileAccessor.Configure().CreateText(Arg.Any<string>()).ReturnsForAnyArgs(new StreamWriter(new MemoryStream()));
        }

        private static void SetupNoConfigFiles(IFileAccessor fileAccessor)
        {
            fileAccessor.Exists(Arg.Any<string>()).ReturnsForAnyArgs(false, false);
        }

        private static void SetupConfigFileInRepo(IFileAccessor fileAccessor, string fileContent)
        {
            fileAccessor.Exists(Arg.Any<string>()).ReturnsForAnyArgs(true);
            fileAccessor.ReadFileAsync(Arg.Any<string>()).ReturnsForAnyArgs(fileContent);
            fileAccessor.Configure().CreateText(Arg.Any<string>()).ReturnsForAnyArgs(new StreamWriter(new MemoryStream()));
        }

        [Theory, TestConventions]
        public void InRepo_Config_Should_Be_Used_First(
            [Frozen]IFileAccessor fileAccessor,
            [Frozen][Substitute] IPopupManager popupManager,
            [Frozen][Substitute] ConfigFileProvider configFileProvider,
            Fixture fixture
            )
        {
            SetupConfigFileInUserProfile(fileAccessor,
                userProfileContent: "{\"types\": [{\"type\": \"feat\"}, {\"type\": \"nope\"}]}",
                inRepoConfigContent: "{\"types\": [{\"type\": \"feat\"}, {\"type\": \"nope\"}, {\"type\": \"nope2\"}]}");

            var sut = getSut(fixture, configFileProvider, (true, "here"));

            sut.CommitTypes.Count.ShouldBe(3);
            popupManager.DidNotReceiveWithAnyArgs().Show(Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory, TestConventions]
        public void UserProfileConfig_Should_Be_Used_If_InRepo_Is_Not_Existing(
            [Frozen]IFileAccessor fileAccessor,
            [Frozen][Substitute] IPopupManager popupManager,
            [Frozen][Substitute] ConfigFileProvider configFileProvider,
            Fixture fixture
            )
        {
            SetupConfigFileInUserProfile(fileAccessor,
                userProfileContent: "{\"types\": [{\"type\": \"feat\"}, {\"type\": \"nope\"}, {\"type\": \"nope2\"}]}");

            var sut = getSut(fixture, configFileProvider, (true, "here"));

            sut.CommitTypes.Count.ShouldBe(3);
            popupManager.DidNotReceiveWithAnyArgs().Show(Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory, TestConventions]
        public void Non_Parsable_Config_Should_Give_Default_CommitTypes_And_Warn_User(
            [Frozen]IFileAccessor fileAccessor,
            [Frozen][Substitute] IPopupManager popupManager,
            [Frozen][Substitute] ConfigFileProvider configFileProvider,
            Fixture fixture
            )
        {
            SetupConfigFileInRepo(fileAccessor, ">");

            var sut = getSut(fixture, configFileProvider, (true, "here"));

            sut.CommitTypes.Count.ShouldBe(11);
            popupManager.ReceivedWithAnyArgs().Show(Arg.Any<string>(), Arg.Any<string>());
        }

        [Theory, TestConventions]
        public void UserProfileConfig_Should_Be_Generated_If_None_Are_Found(
            [Frozen]IFileAccessor fileAccessor,
            [Frozen][Substitute] IPopupManager popupManager,
            [Frozen][Substitute] ConfigFileProvider configFileProvider,
            Fixture fixture
            )
        {
            SetupNoConfigFiles(fileAccessor);

            var sut = getSut(fixture, configFileProvider, (false, null));

            sut.CommitTypes.Count.ShouldBe(11);
            popupManager.DidNotReceiveWithAnyArgs().Show(Arg.Any<string>(), Arg.Any<string>());
            fileAccessor.ReceivedWithAnyArgs().CreateText(Arg.Any<string>());
        }
    }
}
