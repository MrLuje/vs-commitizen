using Microsoft.TeamFoundation.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using vs_commitizen.vs.Models;
using vs_commitizen.vs.Extensions;
using System.Text.RegularExpressions;
using vs_commitizen.vs.Interfaces;

namespace vs_commitizen.vs
{
    /// <summary>
    /// Interaction logic for VsCommitizenView.xaml
    /// </summary>
    public partial class VsCommitizenView : UserControl, INotifyPropertyChanged, ICommentBuilder
    {
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

        private string _scope;
        public string Scope
        {
            get => _scope;
            set
            {
                _scope = value;
                OnPropertyChanged();
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
                OnPropertyChanged(nameof(OnCommit));
            }
        }

        private bool _hasGitPendingChanges;
        public bool HasGitPendingChanges
        {
            get => _hasGitPendingChanges;
            set
            {
                _hasGitPendingChanges = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(OnCommit));
            }
        }

        public VsCommitizenView()
        {
            InitializeComponent();

            this.CommitTypes = new List<CommitType>
            {
                new CommitType("feat", "A new feature"),
                new CommitType("fix", "A bug fix"),
            };
            this.OnCommit = new RelayCommand(Commit, CanCommit);
            this.HasGitPendingChanges = true;   //TODO: Correct way to bind this

            this.DataContext = this;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public bool CanCommit(object param)
        {
            if (this.cbType.SelectedIndex < 0) return false;
            if (string.IsNullOrWhiteSpace(this.tbxSubject.Text)) return false;
            if (!this.HasGitPendingChanges) return false;

            return true;
        }

        public void Commit(object param)
        {
            CommitExecuted?.Invoke(this, new EventArgs());
        }

        public string GetComment()
        {
            var hasScope = !string.IsNullOrWhiteSpace(this.Scope);
            var scope = hasScope ? $"({this.Scope.SafeTrim()})" : string.Empty;
            var commitType = (this.cbType.SelectedItem as CommitType).Type;

            var head = $"{commitType}{scope}: {this.Subject.SafeTrim()}"; //TODO: add a max size check
            var body = string.Join("\n", this.Body.SafeTrim().ChunkBySize(100));

            var hasBreakingChanges = !string.IsNullOrEmpty(this.BreakingChanges);
            var breakingChanges = this.BreakingChanges.SafeTrim();
            if (hasBreakingChanges)
            {
                breakingChanges = "BREAKING CHANGES: " + Regex.Replace(this.BreakingChanges, "^BREAKING CHANGES: ", string.Empty, RegexOptions.IgnoreCase);
                breakingChanges = string.Join("\n", breakingChanges.ChunkBySize(100));
            }

            var issues = !string.IsNullOrWhiteSpace(this.IssuesAffected) ? string.Join("\n", this.IssuesAffected.SafeTrim().ChunkBySize(100)) : string.Empty;

            var comment = string.Empty;
            comment += $"{head}\n\n{body}\n\n";
            if (!string.IsNullOrEmpty(breakingChanges)) comment += $"{breakingChanges}\n\n";
            if (!string.IsNullOrEmpty(issues)) comment += $"{issues}\n\n";
            return comment;
        }

        public event EventHandler CommitExecuted;

        private ICommand _onCommit;
        public ICommand OnCommit
        {
            get => _onCommit;
            set
            {
                _onCommit = value;
                OnPropertyChanged();
            }
        }
    }
}