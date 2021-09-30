using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;


namespace UnusArmatusLattro.Commands
{
    public class FinishGameCommand : ICommand
    {
        public event EventHandler CanExecuteChanged { add { } remove { } }
        public BettingGameViewModel bettingGameViewModel;

        public FinishGameCommand(BettingGameViewModel bettingGameViewModel)
        {
            this.bettingGameViewModel = bettingGameViewModel;
        }

        public bool CanExecute(object parameter) => true;
        
        public void Execute(object parameter)
        {
            bettingGameViewModel.GameOver();
        }
    }
}
