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
            this.baseViewModel = gameOverViewModel;
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
                    case Data.GoToView.SpinGame:
                        baseViewModel.SpinGame();
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
