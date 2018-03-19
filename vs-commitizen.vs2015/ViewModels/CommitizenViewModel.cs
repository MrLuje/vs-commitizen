using Microsoft.TeamFoundation.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Media;
using vs_commitizen.vs.Extensions;
using vs_commitizen.vs.Interfaces;
using vs_commitizen.vs.Models;

namespace vs_commitizen.vs.ViewModels
{
    public class CommitizenViewModel : INotifyPropertyChanged, ICommentBuilder
    {
        #region Bound properties

        private List<CommitType> _commitTypes;
        public List<CommitType> CommitTypes
        {
            get => _commitTypes;
            set
            {
                _commitTypes = value;
                OnPropertyChanged();
            }
        }

        private CommitType _selectedCommitType;
        public CommitType SelectedCommitType
        {
            get => _selectedCommitType;
            set
            {
                _selectedCommitType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(OnProceed));
                OnPropertyChanged(nameof(SubjectLength));
                OnPropertyChanged(nameof(SubjectColor));
            }
        }

        private string _scope;
        public string Scope
        {
            get => _scope;
            set
            {
                _scope = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SubjectLength));
                OnPropertyChanged(nameof(SubjectColor));
            }
        }

        private string _body;
        public string Body
        {
            get => _body;
            set
            {
                _body = value;
                OnPropertyChanged();
            }
        }

        private string _breakingChanges;
        public string BreakingChanges
        {
            get => _breakingChanges;
            set
            {
                _breakingChanges = value;
                OnPropertyChanged();
            }
        }

        private string _issuesAffected;
        public string IssuesAffected
        {
            get => _issuesAffected;
            set
            {
                _issuesAffected = value;
                OnPropertyChanged();
            }
        }

        private string _subject;
        public string Subject
        {
            get => _subject;
            set
            {
                _subject = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(OnProceed));
                OnPropertyChanged(nameof(SubjectLength));
                OnPropertyChanged(nameof(SubjectColor));
            }
        }

        public int SubjectLength
        {
            get
            {
                int type = this.SelectedCommitType?.Type.Length ?? 0;
                int scope = this.Scope?.Length ?? 0;
                int subject = this.Subject?.Length ?? 0;
                var sum = type + scope + subject;
                sum = sum == 0 ? 0 : sum + 1;

                return sum;
            }
        }

        public Brush SubjectColor => this.SubjectLength > 50 ? Brushes.Red : Brushes.Black;

        private bool _hasGitPendingChanges;
        public bool HasGitPendingChanges
        {
            get => _hasGitPendingChanges;
            set
            {
                _hasGitPendingChanges = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(OnProceed));
            }
        }
        #endregion

        public CommitizenViewModel()
        {
            this.CommitTypes = new List<CommitType>
            {
                new CommitType("feat", "A new feature"),
                new CommitType("fix", "A bug fix"),
                new CommitType("docs", "Documentation only changes"),
                new CommitType("style", "Changes that do not affect the meaning of the code (formatting, etc)"),
                new CommitType("refactor", "A code change that neither fixes a bug nor adds a feature"),
                new CommitType("perf", "A code change that improves performance"),
                new CommitType("test", "Adding missing tests or correcting existing tests"),
                new CommitType("build", "Changes that affect the build system or external dependencies (example scopes: gulp, etc)"),
                new CommitType("ci", "Changes to our CI configuration files and scripts (example scopes: Travis, etc)"),
                new CommitType("chore", "Other changes that don't modify src or test files"),
                new CommitType("revert", "Reverts a previous commit")
            };
            this.OnProceed = new RelayCommand(Proceed, CanProceed);
            this.HasGitPendingChanges = true;   //TODO: Correct way to bind this
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanProceed(object param)
        {
            if (this.SelectedCommitType == null) return false;
            if (string.IsNullOrWhiteSpace(this.Subject)) return false;
            if (!this.HasGitPendingChanges) return false;

            return true;
        }

        public void Proceed(object param)
        {
            bool.TryParse(param.ToString(), out var doCommit);
            ProceedExecuted?.Invoke(this, doCommit);
        }

        public string GetComment()
        {
            if (this.SelectedCommitType == null) return string.Empty;

            var hasScope = !string.IsNullOrWhiteSpace(this.Scope);
            var scope = hasScope ? $"({this.Scope.SafeTrim()})" : string.Empty;
            var commitType = this.SelectedCommitType.Type;

            var head = $"{commitType}{scope}: {this.Subject.SafeTrim()}";
            var body = string.Join("\n", this.Body.SafeTrim().ChunkBySizePreverveWords(100));

            var hasBreakingChanges = !string.IsNullOrEmpty(this.BreakingChanges);
            var breakingChanges = this.BreakingChanges.SafeTrim();
            if (hasBreakingChanges)
            {
                breakingChanges = "BREAKING CHANGES: " + Regex.Replace(this.BreakingChanges, "^BREAKING CHANGES: ", string.Empty, RegexOptions.IgnoreCase);
                breakingChanges = string.Join("\n", breakingChanges.ChunkBySizePreverveWords(100));
            }

            var hasIssuesAffected = !string.IsNullOrEmpty(this.IssuesAffected);
            var issues = this.IssuesAffected.SafeTrim();
            if (hasIssuesAffected)
            {
                issues = int.TryParse(issues, out var _) ? $"#{issues}" : issues;
                issues = string.Join("\n", issues.ChunkBySizePreverveWords(100));
            }

            var comment = head;
            if (!string.IsNullOrEmpty(body)) comment += $"\n\n{body}";
            if (!string.IsNullOrEmpty(breakingChanges)) comment += $"\n\n{breakingChanges}";
            if (!string.IsNullOrEmpty(issues)) comment += $"\n\n{issues}";
            return comment;
        }

        public event EventHandler<bool> ProceedExecuted;

        private ICommand _onProceed;
        public ICommand OnProceed
        {
            get => _onProceed;
            set
            {
                _onProceed = value;
                OnPropertyChanged();
            }
        }
    }
}
