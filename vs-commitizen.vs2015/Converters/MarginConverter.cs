using System.Windows;

namespace vs_commitizen.vs.Converters
{
    public class MarginConverter : BooleanConverter<Thickness>
    {
        public MarginConverter() : base(new Thickness(0, 0, 0, 0), new Thickness(10, 5, 0, 0)) { }
    }
}
