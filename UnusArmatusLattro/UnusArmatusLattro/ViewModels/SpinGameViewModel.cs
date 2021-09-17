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
        public ICommand HomeCommand { get; set; }
        public ICommand SpinView { get; set; }
        private readonly MainViewModel parent;

        public SpinGameViewModel(MainViewModel parent)
        {
            this.parent = parent;
            SpinView = new StartNewGameCommand(this);
        }

        public void GoToMenu()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }

        public void StartGame(Difficulties diff)
        {
            parent.CurrentViewModel = new GameViewModel(parent, diff);
        }
        
    }
}
