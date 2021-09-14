using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.Commands;

namespace UnusArmatusLattro.ViewModels
{
    public class HighScoreViewModel : BaseViewModel
    {
        public ICommand HomeCommand { get; set; }
        private readonly MainViewModel parent;

        public HighScoreViewModel(MainViewModel parent)
        {
            this.parent = parent;
            HomeCommand = new HighScoreCommand(this);
        }

        public void GoToMenu()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }

    }
}
