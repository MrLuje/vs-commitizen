using System;
using System.Windows.Controls;
using vs_commitizen.vs.ViewModels;

namespace vs_commitizen.vs
{
    /// <summary>
    /// Interaction logic for VsCommitizenView.xaml
    /// </summary>
    public partial class VsCommitizenView : UserControl
    {
        CommitizenViewModel _viewModel = new CommitizenViewModel();

        public VsCommitizenView()
        {
            InitializeComponent();

            this.DataContext = _viewModel;
        }

        public CommitizenViewModel ViewModel => _viewModel;
    }
}