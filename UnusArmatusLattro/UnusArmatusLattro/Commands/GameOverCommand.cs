using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Commands
{
    public class GameOverCommand 
    {
        public event EventHandler CanExecuteChanged;
        public GameOverViewModel gameOverModel;

        public GameOverCommand(GameOverViewModel gameOverViewModel)
        {
            //this.gameOverModel = gameOverViewModel;
        }
    }
}
