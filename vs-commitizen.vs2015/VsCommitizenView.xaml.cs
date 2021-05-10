using System;
using System.Windows.Controls;
using vs_commitizen.Settings;
using vs_commitizen.vs.ViewModels;

namespace vs_commitizen.vs
{
    /// <summary>
    /// Interaction logic for VsCommitizenView.xaml
    /// </summary>
    public partial class VsCommitizenView : UserControl, IVsCommitizenView
    {
        CommitizenViewModel _viewModel = IoC.GetInstance<CommitizenViewModel>();

        public VsCommitizenView()
        {
            InitializeComponent();

            this.DataContext = _viewModel;
        }

        public CommitizenViewModel ViewModel => _viewModel;

        public void SetTeamExplorerMode(bool teamExplorerMode)
        {
            ViewModel.TeamExplorerMode = teamExplorerMode;
        }
    }
}