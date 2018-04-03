using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace vs_commitizen.vs
{
    public interface ITeamExplorerSectionCommand
    {
        ICommand Command
        {
            get;
        }

        object Icon
        {
            get;
        }

        bool IsVisible
        {
            get;
        }

        string Text
        {
            get;
        }
    }
}