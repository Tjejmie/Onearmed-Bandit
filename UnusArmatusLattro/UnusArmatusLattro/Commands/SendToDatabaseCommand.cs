using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Commands
{
    public class SendToDatabaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged { add { } remove { } }
        private readonly GameOverViewModel gameOverViewModel;
        
        public SendToDatabaseCommand(GameOverViewModel gameOverViewModel)
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
