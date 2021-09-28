using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;


namespace UnusArmatusLattro.Commands
{
    public class FinishGame : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public BettingGameViewModel bettingGameView;

        public FinishGame(BettingGameViewModel bettingGameView)
        {
            this.bettingGameView = bettingGameView;
        }

        public bool CanExecute(object parameter) => true;
        
        public void Execute(object parameter)
        {
            bettingGameView.GameOver();
        }
    }
}
