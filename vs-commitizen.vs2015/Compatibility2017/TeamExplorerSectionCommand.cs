using System.Windows.Input;
using System.Windows.Media;

namespace vs_commitizen.vs
{
    internal class TeamExplorerSectionCommand : ITeamExplorerSectionCommand
    {
        private ICommand command;
        private string v;
        private DrawingBrush drawingBrush;

        public TeamExplorerSectionCommand(ICommand command, string v, DrawingBrush drawingBrush)
        {
            this.command = command;
            this.v = v;
            this.drawingBrush = drawingBrush;
        }
    }
}