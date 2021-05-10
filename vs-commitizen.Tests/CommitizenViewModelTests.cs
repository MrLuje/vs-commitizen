using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using vs_commitizen.Settings;
using vs_commitizen.Tests.TestAttributes;
using vs_commitizen.vs.Settings;
using vs_commitizen.vs.ViewModels;
using Xunit;

namespace vs_commitizen.Tests
{
    public class CommitizenViewModelTests
    {
        [Theory, TestConventions]
        public void CantProcess_If_Subject_Empty(CommitizenViewModel sut)
        {
            sut.Subject = string.Empty;
            sut.CanProceed(null).ShouldBe(false);
        }

        [Theory, TestConventions]
        public void CantProcess_If_CommitType_IsNot_Selected(CommitizenViewModel sut)
        {
            sut.SelectedCommitType = null;
            sut.CanProceed(null).ShouldBe(false);
        }

        [Theory, TestConventions]
        public void CanProcess_If_CommitType_And_Subject_Are_Filled(CommitizenViewModel sut)
        {
            sut.SelectedCommitType.ShouldNotBeNull();
            sut.Subject.ShouldNotBeNullOrEmpty();
            sut.CanProceed(null).ShouldBe(true);
        }

        [Theory, TestConventions]
        public void Should_Contains_CommitTypes_List(CommitizenViewModel sut)
        {
            sut.CommitTypes.Count.ShouldBeGreaterThan(0);
        }

        void TestPropertyChangedFor(CommitizenViewModel sut, Action<CommitizenViewModel> action, string[] changed)
        {
            var calledSoFar = new List<string>();
            sut.PropertyChanged += (s, e) =>
            {
                // Assert
                e.PropertyName.ShouldBeOneOf(changed);
                calledSoFar.Add(e.PropertyName);
            };

            // Act
            action(sut);

            // Assert
            calledSoFar.ShouldBe(changed, ignoreOrder: true);
        }

        [Theory, TestConventions]
        public void Scope_Changes_Should_Trigger_SubjectLength_Color_Change(CommitizenViewModel sut)
        {
            TestPropertyChangedFor(sut, (s) => s.Scope = "abc", new[] { "Scope", "SubjectLength", "SubjectColor" });
        }

        [Theory, TestConventions]
        public void SelectedCommitType_Changes_Should_Trigger_SubjectLength_Color_Change_And_OnProceed(CommitizenViewModel sut)
        {
            TestPropertyChangedFor(sut, s => s.SelectedCommitType = s.CommitTypes.Last(), new[] { "SelectedCommitType", "SubjectLength", "SubjectColor", "OnProceed", "OnCopy", "OnReset" });
        }

        [Theory, TestConventions]
        public void Subject_Changes_Should_Trigger_SubjectLength_Color_Change_And_OnProceed(CommitizenViewModel sut)
        {
            TestPropertyChangedFor(sut, s => s.Subject = "sub", new[] { "Subject", "SubjectLength", "SubjectColor", "OnProceed" });
        }

        [Theory, TestConventions]
        public void SubjectLength_Should_DependsOn_Scope(CommitizenViewModel sut)
        {
            var actualLength = sut.SubjectLength;

            // Act 
            sut.Scope = sut.Scope.Substring(0, sut.Scope.Length - 1);

            // Assert
            sut.SubjectLength.ShouldBe(actualLength - 1);
        }

        [Theory, TestConventions]
        public void SubjectLength_Should_DependsOn_Subject(CommitizenViewModel sut)
        {
            var actualLength = sut.SubjectLength;

            // Act 
            sut.Subject = sut.Subject.Substring(0, sut.Subject.Length - 1);

            // Assert
            sut.SubjectLength.ShouldBe(actualLength - 1);
        }

        [Theory, TestConventions]
        public void SubjectLength_Should_DependsOn_CommitType(CommitizenViewModel sut)
        {
            sut.SelectedCommitType = sut.CommitTypes.First();
            var actualLength = sut.SubjectLength;

            // Act 
            sut.SelectedCommitType = sut.CommitTypes.Skip(1).First();

            // Assert
            int differenceBetweenSubjectLength = Math.Abs(sut.SubjectLength - actualLength);
            int differenceBetweenTypeLength = Math.Abs(sut.CommitTypes.First().Type.Length - sut.CommitTypes.Skip(1).First().Type.Length);
            differenceBetweenSubjectLength.ShouldBe(differenceBetweenTypeLength);
        }

        [Theory, TestConventions]
        public void SubjectLength_Is_sum(CommitizenViewModel sut)
        {
            var actualLength = sut.SubjectLength;

            // Assert
            actualLength.ShouldBe(sut.Subject.Length + sut.Scope.Length + sut.SelectedCommitType.Type.Length + 1);
        }

        [Theory, TestConventions]
        public void SubjectLength_Is_Zero_If_No_Selected(CommitizenViewModel sut)
        {
            // Act
            sut.Subject = string.Empty;
            sut.Scope = string.Empty;
            sut.SelectedCommitType = null;

            // Assert
            sut.SubjectLength.ShouldBe(0);
        }

        [Theory]
        [InlineTestConventions("true", true)]
        [InlineTestConventions("false", false)]
        public void Proceed_Sets_Autocommit(string autoCommit, bool expected, CommitizenViewModel sut)
        {
            // Assert
            sut.ProceedExecuted += (s, b) => b.ShouldBe(expected);

            // Act
            sut.Proceed(autoCommit);
        }

        [Theory, TestConventions]
        public void GetComment_With_No_SelectedCommitType_ShouldBe_Empty(
            IUserSettings userSettings,
            IConfigFileProvider configFileProvider
            )
        {
            var sut = new CommitizenViewModel(userSettings, configFileProvider);
            sut.SelectedCommitType = null;
            sut.GetComment().ShouldBeEmpty();
        }

        [Theory, TestConventions]
        public void GetComment_With_No_Scope(CommitizenViewModel sut)
        {
            sut.Scope = null;
            sut.GetComment().ShouldStartWith($"{sut.SelectedCommitType.Type}: ");
        }

        [Theory, TestConventions]
        public void GetComment_With_Scope(CommitizenViewModel sut)
        {
            sut.GetComment().ShouldStartWith($"{sut.SelectedCommitType.Type}({sut.Scope}): ");
        }

        [Theory, TestConventions]
        public void GetComment_With_No_Body(CommitizenViewModel sut)
        {
            sut.IssuesAffected = sut.Body = sut.BreakingChanges = null;
            sut.GetComment().ShouldBe($"{sut.SelectedCommitType.Type}({sut.Scope}): {sut.Subject}");
        }

        [Theory, TestConventions]
        public void GetComment_Should_Prefix_BreakingChanges(CommitizenViewModel sut)
        {
            sut.BreakingChanges = "no more login !";
            sut.GetComment().ShouldContain($"BREAKING CHANGE: {sut.BreakingChanges}");
        }

        [Theory, TestConventions]
        public void GetComment_Should_NotSuffix_Type_If_No_Highlight_BreakingChanges(CommitizenViewModel sut)
        {
            sut.BreakingChanges = "no more login !";
            sut.GetComment().ShouldStartWith($"{sut.SelectedCommitType.Type}({sut.Scope}):");
        }

        [Theory, TestConventions]
        public void GetComment_Should_Suffix_Type_If_Highlight_BreakingChanges(CommitizenViewModel sut)
        {
            sut.BreakingChanges = "no more login !";
            sut.HighlighBreakingChanges = true;
            sut.GetComment().ShouldStartWith($"{sut.SelectedCommitType.Type}({sut.Scope})!:");
        }

        [Theory, TestConventions]
        public void GetComment_Should_Prefix_Issues_If_Number(CommitizenViewModel sut)
        {
            sut.IssuesAffected = "666";
            sut.GetComment().ShouldEndWith("\n\ncloses #666");
        }

        [Theory, TestConventions]
        public void GetComment_ShouldNot_Prefix_Issues_If_NotNumber(CommitizenViewModel sut)
        {
            sut.IssuesAffected = "666 & 999";
            sut.GetComment().ShouldEndWith($"\n\ncloses {sut.IssuesAffected}");
        }

        [Theory, TestConventions]
        public void GetComment_ShouldNot_Take_Last_Space_If_Over_ChunkSize(
            IUserSettings userSettings,
            IConfigFileProvider configFileProvider)
        {
            userSettings.MaxLineLength = 10;

            var sut = new CommitizenViewModel(userSettings, configFileProvider);
            sut.SelectedCommitType = sut.CommitTypes.First(f => f.Type.Contains("feat"));
            sut.Scope = "test";
            sut.Body = "test";
            sut.Body += " tenwordsss tenwordsss";
            sut.GetComment().ShouldBe("feat(test): \n\ntest\ntenwordsss\ntenwordsss");
        }

        [WpfTheory, TestConventions]
        public void Copy_Should_Contains_Comment_Message(CommitizenViewModel sut)
        {
            Clipboard.Clear();
            var expectedComment = sut.GetComment();

            // Act
            sut.CopyMessage(null);

            // Assert
            var comment = Clipboard.GetText();
            comment.ShouldBe(expectedComment);
        }

        [WpfTheory, TestConventions]
        public void Reset_Should_Clear_Form(CommitizenViewModel sut)
        {
            sut.Body.ShouldNotBeNullOrWhiteSpace();
            sut.BreakingChanges.ShouldNotBeNullOrWhiteSpace();
            sut.IssuesAffected.ShouldNotBeNullOrWhiteSpace();
            sut.Scope.ShouldNotBeNullOrWhiteSpace();
            sut.Subject.ShouldNotBeNullOrWhiteSpace();
            var highlightChanges = sut.HighlighBreakingChanges;

            // Act
            sut.Reset(null);

            // Assert
            sut.Body.ShouldBeNullOrWhiteSpace();
            sut.BreakingChanges.ShouldBeNullOrWhiteSpace();
            sut.IssuesAffected.ShouldBeNullOrWhiteSpace();
            sut.Scope.ShouldBeNullOrWhiteSpace();
            sut.Subject.ShouldBeNullOrWhiteSpace();

            if(highlightChanges)
                sut.HighlighBreakingChanges.ShouldBeFalse();
        }
    }
}
