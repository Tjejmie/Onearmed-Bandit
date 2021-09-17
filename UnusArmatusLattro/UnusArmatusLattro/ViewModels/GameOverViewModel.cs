using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.Commands;
using UnusArmatusLattro.Data;
using UnusArmatusLattro.Models;
using UnusArmatusLattro.Repositories;
using UnusArmatusLattro.Views;

namespace UnusArmatusLattro.ViewModels
{
    public class GameOverViewModel : BaseViewModel
    {
        public ICommand GameOverCommand { get; set; }
        private readonly MainViewModel parent;
        public string Points { get; set; }
        public ObservableCollection<HighscoreView> HighScores { get; set; }
        public Difficulties Difficulty { get; set; }
        public UserRepository Repo { get; set; } = new UserRepository();

        GameViewModel gameViewModel;
        public GameOverViewModel(MainViewModel parent, string score, Difficulties diff)
        {
            Difficulty = diff;
            this.parent = parent;
            GameOverCommand = new GameOverCommand(this);
            Points = score;
            GetHighscores();
        }

        public void SpinGame()
        {
            parent.CurrentViewModel = new SpinGameViewModel(parent);
        }

        public void GetHighscores()
        {
            HighScores = new ObservableCollection<HighscoreView>();
            List<Username> templist = Repo.GetUsers(Difficulty);

            foreach (var user in templist)
            {
                HighscoreView temp = new HighscoreView
                {
                    Name = user.Name,
                    Score = user.Points
                };
                HighScores.Add(temp);
            }

        }

    }



}
