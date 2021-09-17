using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.Commands;
using UnusArmatusLattro.Views;

namespace UnusArmatusLattro.ViewModels
{
    public class GameOverViewModel : BaseViewModel
    {
        public ICommand GameOverCommand { get; set; }
        private readonly MainViewModel parent;
        public string Points { get; set; }
        public ObservableCollection<HighscoreView> HighScores { get; set; }

        GameViewModel gameViewModel;
        public GameOverViewModel(MainViewModel parent, string score, ObservableCollection<HighscoreView> highscores)
        {
            HighScores = highscores;
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
