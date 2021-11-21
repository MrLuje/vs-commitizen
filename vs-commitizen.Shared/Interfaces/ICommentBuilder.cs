using Microsoft.TeamFoundation.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using vs_commitizen.vs.Models;
using vs_commitizen.vs.Extensions;
using System.Text.RegularExpressions;

namespace vs_commitizen.vs.Interfaces
{

    internal interface ICommentBuilder
    {
        string GetComment();
    }
}