using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.Commands;

namespace UnusArmatusLattro.ViewModels
{
    public class MainMenuHighScoreViewModel : BaseViewModel
    {
        public ICommand HomeCommand { get; set; }
        private readonly MainViewModel parent;
        
        public MainMenuHighScoreViewModel(MainViewModel parent)
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
