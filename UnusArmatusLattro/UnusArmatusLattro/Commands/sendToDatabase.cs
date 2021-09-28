using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Commands
{
    public class sendToDatabase : ICommand
    {
        public event EventHandler CanExecuteChanged { add { } remove { } }
        private GameOverViewModel gameOverViewModel;
        private GameViewModel gameViewModel;
        private BettingGameViewModel bettingGameViewModel;

        public sendToDatabase(BettingGameViewModel bettingGameViewModel)
        {
            this.bettingGameViewModel = bettingGameViewModel;
        }

        public sendToDatabase(GameOverViewModel gameOverViewModel)
        {
            this.gameOverViewModel = gameOverViewModel;
        }

        public sendToDatabase(GameViewModel bettingGameViewModel)
        {
            this.gameViewModel = bettingGameViewModel;
        }
        public bool CanExecute(object parameter) => true;
        

        public void Execute(object parameter)
        {
            gameOverViewModel.SendUser();
            gameOverViewModel.GetHighscores();

            //if (gameViewModel != null)
            //{
            //    //gameViewModel.SendUser();
            //    gameViewModel.GetHighscores();
            //}
            //else
            //{
            //    bettingGameViewModel.SendUser();
            //    bettingGameViewModel.GetHighscores();
            //}
            
        }
    }
}
