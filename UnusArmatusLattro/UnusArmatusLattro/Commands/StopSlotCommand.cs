using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Commands
{
    class StopSlotCommand : ICommand
    {
        public event EventHandler CanExecuteChanged { add { } remove { } }
        private readonly GameViewModel gameViewModel;
        private readonly BettingGameViewModel bettingGameViewModel;

        public StopSlotCommand(BettingGameViewModel bettingGameViewModel)
        {
            this.bettingGameViewModel = bettingGameViewModel;
        }

        public StopSlotCommand(GameViewModel gameViewModel)
        {
            this.gameViewModel = gameViewModel;
        }
        
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            if (gameViewModel != null)
                gameViewModel.StopSlot();
            else
                bettingGameViewModel.StopSlot();
        }
    }
}
