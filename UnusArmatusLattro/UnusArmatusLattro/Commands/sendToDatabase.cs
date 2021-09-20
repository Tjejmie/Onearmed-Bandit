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
        public event EventHandler CanExecuteChanged;
        private GameOverViewModel gameOverViewModel;

        public sendToDatabase(GameOverViewModel gameOverViewModel)
        {
            this.gameOverViewModel = gameOverViewModel;
        }

        public bool CanExecute(object parameter) => true;
        

        public void Execute(object parameter)
        {
            gameOverViewModel.SendUser();
            gameOverViewModel.GetHighscores();
        }
    }
}
