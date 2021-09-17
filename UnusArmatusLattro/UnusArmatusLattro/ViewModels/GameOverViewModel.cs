using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.Commands;

namespace UnusArmatusLattro.ViewModels
{
    public class GameOverViewModel : BaseViewModel
    {
        public ICommand GameOverCommand { get; set; }
        private readonly MainViewModel parent;
        public string Points { get; set; }

        GameViewModel gameViewModel;
        public GameOverViewModel(MainViewModel parent, string score)
        {
            this.parent = parent;
            GameOverCommand = new GameOverCommand(this);
            Points = score; 
        }

        public void SpinGame()
        {
            parent.CurrentViewModel = new SpinGameViewModel(parent);
        }

    }



}
