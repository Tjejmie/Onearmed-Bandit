using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.Commands;

namespace UnusArmatusLattro.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            
        }

        public BaseViewModel CurrentViewModel { get; set; }

    }
}
