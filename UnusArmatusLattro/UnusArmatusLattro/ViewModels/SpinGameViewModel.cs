using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.Commands;
using UnusArmatusLattro.Data;

namespace UnusArmatusLattro.ViewModels
{
    public class SpinGameViewModel : BaseViewModel
    {
        public ICommand Home { get; }
        public ICommand SpinView { get; }
        private readonly MainViewModel parent;

        public SpinGameViewModel(MainViewModel parent)
        {
            this.parent = parent;
            SpinView = new StartNewGameCommand(this);
            Home = new GoToHomeCommand(this);
        }

        public void GoHome()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }

        public void StartGame(Difficulties diff)
        {
            parent.CurrentViewModel = new GameViewModel(parent, diff);
        }
        

    }
}
