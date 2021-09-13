using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.Commands;

namespace UnusArmatusLattro.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        private readonly MainViewModel parent;

        public StartViewModel(MainViewModel parent)
        {
            this.parent = parent;
            ChangeView = new ChangeViewCommand(this);
        }
        public ICommand ChangeView { get; set; }

        public void StartGame()
        {
            parent.CurrentViewModel = new GameViewModel();
        }

        public void Rules()
        {
            parent.CurrentViewModel = new RulesViewModel(parent);
        }
    }
}
