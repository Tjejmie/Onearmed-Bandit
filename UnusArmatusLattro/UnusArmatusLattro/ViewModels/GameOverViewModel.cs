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
        public GameOverViewModel(MainViewModel parent)
        {
            this.parent = parent;
            GameOverCommand = new GameOverCommand(this);
            
        }


        public void GetPoints()
        {
            
        }
        public void SpinGame()
        {
            parent.CurrentViewModel = new SpinGameViewModel(parent);
        }

    }



}
