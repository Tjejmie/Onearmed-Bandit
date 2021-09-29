using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Commands
{
    class SpinCommand : ICommand
    {
        public event EventHandler CanExecuteChanged { add { } remove { } }

        private readonly GameViewModel gameViewModel;
        private readonly BettingGameViewModel bettingGameViewModel;

        public SpinCommand(BettingGameViewModel bettingGameViewModel)
        {
            this.bettingGameViewModel = bettingGameViewModel;
        }

        public SpinCommand(GameViewModel gameViewModel)
        {
            this.gameViewModel = gameViewModel;
        }
        

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {

            if (gameViewModel != null)
                gameViewModel.SpinSlots();
            else
                bettingGameViewModel.SpinSlots();
                
        }
    }
}
