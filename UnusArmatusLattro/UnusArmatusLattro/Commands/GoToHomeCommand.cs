using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Commands
{
    public class GoToHomeCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private BettingGameViewModel bettingGameViewModel;
        private GameViewModel gameViewModel;
        private SpinGameViewModel spinGameViewModel;

        public GoToHomeCommand(SpinGameViewModel spinGameViewModel)
        {
            this.spinGameViewModel = spinGameViewModel;
        }

        public GoToHomeCommand(BettingGameViewModel bettingGameViewModel)
        {
            this.bettingGameViewModel = bettingGameViewModel;
        }

        public GoToHomeCommand(GameViewModel gameViewModel)
        {
            this.gameViewModel = gameViewModel;
        }

        public bool CanExecute(object parameter) => true;
       

        public void Execute(object parameter)
        {
            if (bettingGameViewModel != null)
            {
                bettingGameViewModel.GoHome();
            }
            else if (gameViewModel != null)
            {
                gameViewModel.GoHome();
            }
            else if (spinGameViewModel != null)
            {
                spinGameViewModel.GoHome();
            }

        }
    }
}
