using System;
using System.Collections.Generic;
using System.Text;

namespace UnusArmatusLattro.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        //public BaseViewModel CurrentViewModel { get; set; } = new GameViewModel();
        public BaseViewModel CurrentViewModel { get; set; } = new StartViewModel();
    }
}
