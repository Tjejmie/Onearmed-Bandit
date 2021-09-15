using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Commands
{
    public class BetCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private GameViewModel gameViewModel;

        public BetCommand(GameViewModel gameViewModel)
        {
            this.gameViewModel = gameViewModel;
        }

        public bool CanExecute(object parameter) => true;
        

        public void Execute(object parameter)
        {
            gameViewModel.ConfirmBet();
        }
    }
}
