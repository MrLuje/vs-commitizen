using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vs_commitizen.vs.Settings
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [Guid("1D9ECCF3-5D2F-4112-9B25-264596873DC9")]
    public class SettingsPage : DialogPage
    {
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //protected override IWin32Window Window
        //{
        //    get
        //    {
        //        var page = new SettingsUpdate_Form();
        //        page.optionsPage = this;
        //        page.Initialize();
        //        return page;
        //    }
        //}
    }
}
