using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Commands
{
    public class StartOverCommand : ICommand
    {
        public event EventHandler CanExecuteChanged { add { } remove { } }
        public GameOverViewModel gameOverViewModel;

        public StartOverCommand(GameOverViewModel gameOverViewModel)
        {
            this.gameOverViewModel = gameOverViewModel;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            gameOverViewModel.PlayAgain();
        }
    }
}
