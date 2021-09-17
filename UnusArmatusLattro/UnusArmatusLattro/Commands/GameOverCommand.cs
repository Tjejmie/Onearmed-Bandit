using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Commands
{
    public class GameOverCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public GameOverViewModel baseViewModel;

        public GameOverCommand(GameOverViewModel gameOverViewModel)
        {
            //this.gameOverModel = gameOverViewModel;
        }

        public bool CanExecute(object parameter) => true;
        

        public void Execute(object parameter)
        {
            try
            {
                switch (parameter)
                {
                    
                    case Data.GoToView.Exit:
                        Environment.Exit(0);
                        break;


                    default:
                        break;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
