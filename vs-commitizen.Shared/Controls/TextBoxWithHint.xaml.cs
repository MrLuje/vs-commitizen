using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

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
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(TextBoxWithHint));

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }
        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register("MaxLength", typeof(int), typeof(TextBoxWithHint));

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void TxtInputBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
    }
}
