using System.Windows.Input;
using System.Windows.Media;

namespace vs_commitizen.vs
{
    internal class TeamExplorerSectionCommand : ITeamExplorerSectionCommand
    {
        public ICommand Command { get; private set; }
        public string Text { get; private set; }
        public object Icon { get; private set; }
        public bool IsVisible { get; set; }

        public TeamExplorerSectionCommand(ICommand command, string text, object drawingBrush)
        {
            this.Command = command;
            this.Text = text;
            this.Icon = drawingBrush;
        }
    }
}