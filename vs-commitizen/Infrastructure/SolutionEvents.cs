using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell.Interop;

namespace vs_commitizen.Infrastructure
{
    public class SolutionEvent
    {
        private readonly DTE2 _dte;
        private SolutionEvents _solutionEvents;
        private DTEEvents _dteEvents;

        public event _dispSolutionEvents_OpenedEventHandler OnSolutionOpened
        {
            add { _solutionEvents.Opened += value; }
            remove { _solutionEvents.Opened -= value; }
        }

        public event _dispSolutionEvents_BeforeClosingEventHandler OnBeforeClosing
        {
            add { _solutionEvents.BeforeClosing += value; }
            remove { _solutionEvents.BeforeClosing -= value; }
        }

        public SolutionEvent(SDTE dte)
        {
            _dte = (DTE2)dte;
            _solutionEvents = _dte.Events.SolutionEvents;
            _dteEvents = _dte.Events.DTEEvents;
        }
    }
}
