using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.Data;
using UnusArmatusLattro.ViewModels;


namespace UnusArmatusLattro.Commands
{
    public class ChangeViewCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private StartViewModel baseViewModel;
        


        public ChangeViewCommand(StartViewModel startViewModel)
        {
            this.baseViewModel = startViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(Object parameter)
        {
            if (parameter.GetType() == typeof(Difficulties))
            {
                baseViewModel.StartGame((Difficulties)parameter);
            }
                try
            {
                    switch (parameter)
                    {
                        case Data.GoToView.Menu:
                            break;
                        case Data.GoToView.Rules:
                            baseViewModel.Rules();
                            break;
                        case Data.GoToView.HighScore:
                            baseViewModel.Highscore();
                            break;
                        case Data.GoToView.BettingGame:
                            baseViewModel.BettingGame(Difficulties.Betting);
                            break;
                        case Data.GoToView.SpinGame:
                            baseViewModel.SpinGame();
                            break;
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
