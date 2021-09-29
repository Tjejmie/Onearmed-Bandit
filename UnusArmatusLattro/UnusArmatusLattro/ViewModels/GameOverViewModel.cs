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
        public string User { get; set; }
        public ICommand SendToDatabase { get; }
        public bool InputAccepted { get; set; } = true;
        public ICommand Home { get; }

        public GameOverViewModel(MainViewModel parent, string score, Difficulties diff)
        {
            Difficulty = diff;
            this.parent = parent;
            GameOverCommand = new GameOverCommand(this);
            Points = score;
            GetHighscores();
            User = "";
            SendToDatabase = new SendToDatabaseCommand(this);
            GameOver();
            Home = new GoToHomeCommand(this);
        }

        public void SpinGame()
        {
            if (Difficulty != Difficulties.Betting)
            {
                parent.CurrentViewModel = new SpinGameViewModel(parent);
            }
            else
            {
                parent.CurrentViewModel = new BettingGameViewModel(parent, Difficulty);
            }
            
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
        public void SendUser()
        {
            if (User != "")
            {
                User user = new User(User, int.Parse(Points));
                Repo.SendUser(user, Difficulty);
                InputAccepted = false;
            }
        }
        private bool IsHighScore(int score)
        {
            foreach (var highScore in HighScores)
            {
                if (score > highScore.Score)
                {
                    return true;
                }
            }
            if (HighScores.Count < 10)
            {
                return true;
            }
            return false;
        }

        private void GameOver()
        {
            IsHighScore(int.Parse(Points));
        }

        public void GoHome()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }
    }



}
