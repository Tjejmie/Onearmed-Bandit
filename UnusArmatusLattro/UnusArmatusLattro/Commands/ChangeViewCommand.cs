using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;


namespace UnusArmatusLattro.Commands
{
    public class ChangeViewCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private BaseViewModel baseViewModel;
        


        public ChangeViewCommand(BaseViewModel startViewModel)
        {
            this.baseViewModel = startViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(Object parameter)
        {
            try
            {
                switch (parameter)
                {
                    case Data.GoToView.Menu:
                        break;
                    case Data.GoToView.Rules:
                        (baseViewModel as StartViewModel).Rules();
                        break;
                    case Data.GoToView.HighScore:
                        (baseViewModel as StartViewModel).Highscore();
                        break;
                    case Data.GoToView.Game:
                        (baseViewModel as StartViewModel).StartGame();
                        //baseViewModel.StartGame();
                        break;
                    case Data.GoToView.Exit:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            if (parameter.ToString() == "game")
            {
                
            }
            if (true)
            {

            }
        }
    }
}
