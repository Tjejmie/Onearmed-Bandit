using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Commands
{
    public class HighScoreCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private HighScoreViewModel highScoreViewModel;

        public HighScoreCommand(HighScoreViewModel startViewModel)
        {
            this.highScoreViewModel = startViewModel;
        }


        public bool CanExecute(object parameter) => true;


        public void Execute(object parameter)
        {
            highScoreViewModel.GoToMenu();
        }
    }
}
