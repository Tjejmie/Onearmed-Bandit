using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.Commands;

namespace UnusArmatusLattro.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel
    {
        public ICommand Spin { get; set; }
    }
}
