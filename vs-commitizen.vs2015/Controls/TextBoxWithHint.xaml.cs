using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace vs_commitizen.vs2015.Controls
{
    /// <summary>
    /// Interaction logic for TextBoxWithHint.xaml
    /// </summary>
    public partial class TextBoxWithHint : UserControl, INotifyPropertyChanged
    {
        public TextBoxWithHint()
        {
            InitializeComponent();

            this.DataContext = this;
            this.HintText = "Type something...";
        }

        public static readonly DependencyProperty HintTextProperty = DependencyProperty.Register("HintText", typeof(string), typeof(TextBoxWithHint));

        public string HintText
        {
            get { return (string)GetValue(HintTextProperty); }
            set { SetValue(HintTextProperty, value); }
        }

        public static readonly DependencyProperty AcceptReturnProperty = DependencyProperty.Register("AcceptReturn", typeof(bool), typeof(TextBoxWithHint));

        public bool AcceptReturn
        {
            get { return (bool)GetValue(AcceptReturnProperty); }
            set { SetValue(AcceptReturnProperty, value); }
        }

        public string Text
        {
            get => this.txtInputBox.Text;
            set
            {
                this.txtInputBox.Text = value;
                SetValue(TextProperty, value);
                OnPropertyChanged();
            }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBoxWithHint));

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
