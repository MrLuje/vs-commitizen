using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using vs_commitizen.vs.Models;

namespace vs_commitizen.vs
{
    /// <summary>
    /// Interaction logic for VsCommitizenView.xaml
    /// </summary>
    public partial class VsCommitizenView : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty CommitTypesProperty = DependencyProperty.Register("CommitTypes", typeof(List<CommitType>), typeof(VsCommitizenView));

        public List<CommitType> CommitTypes
        {
            get { return (List<CommitType>)GetValue(CommitTypesProperty); }
            set { SetValue(CommitTypesProperty, value); }
        }

        public VsCommitizenView()
        {
            InitializeComponent();

            this.CommitTypes = new List<CommitType>
            {
                new CommitType("feat", "A new feature"),
                new CommitType("fix", "A bug fix"),
            };

            this.DataContext = this;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;


    }
}
