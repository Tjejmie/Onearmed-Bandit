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
        private StartViewModel startViewModel;
        

        public ChangeViewCommand(StartViewModel startViewModel)
        {
            this.startViewModel = startViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter.ToString() == "game")
            {
                startViewModel.StartGame();
            }
        }
    }
}
