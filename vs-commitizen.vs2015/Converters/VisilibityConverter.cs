using System.Windows;

namespace vs_commitizen.vs.Converters
{
    public class VisilibityConverter : BooleanConverter<Visibility>
    {
        public VisilibityConverter() : base(Visibility.Visible, Visibility.Hidden) { }
    }
}
