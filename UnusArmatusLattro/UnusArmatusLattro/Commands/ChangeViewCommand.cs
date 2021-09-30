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
        public event EventHandler CanExecuteChanged { add { } remove { } }
        private readonly StartViewModel startViewModel;

        public ChangeViewCommand(StartViewModel startViewModel)
        {
            this.startViewModel = startViewModel;
        }

        public bool CanExecute(object parameter) => true;

        public void Execute(Object parameter)
        {
            if (parameter.GetType() == typeof(Difficulties))
            {
                startViewModel.StartGame((Difficulties)parameter);
            }
            try
            {
                switch (parameter)
                {
                    case GoToView.Rules:
                        startViewModel.Rules();
                        break;
                    case GoToView.HighScore:
                        startViewModel.Highscore();
                        break;
                    case GoToView.BettingGame:
                        startViewModel.BettingGame(Difficulties.Betting);
                        break;
                    case GoToView.SpinGame:
                        startViewModel.SpinGame();
                        break;
                    case GoToView.Exit:
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
