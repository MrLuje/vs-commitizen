using System.Windows;
using System.Windows.Data;

namespace vs_commitizen.vs.Converters
{
    public class HiddenConverter : BooleanConverter<Visibility>
    {
        public HiddenConverter() : base(Visibility.Hidden, Visibility.Visible) { }
    }
}
