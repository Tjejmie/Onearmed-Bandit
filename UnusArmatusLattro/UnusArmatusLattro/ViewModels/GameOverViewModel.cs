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
        public ICommand SendToDatabase { get; }
        public ICommand Home { get; }
        private readonly MainViewModel parent;
        public string Points { get; set; }
        public ObservableCollection<HighscoreView> HighScores { get; set; }
        public Difficulties Difficulty { get; set; }
        public UserRepository Repo { get; set; } = new UserRepository();
        public string UserName { get; set; }
        public bool InputAccepted { get; set; } = true;
        

        public GameOverViewModel(MainViewModel parent, string score, Difficulties diff)
        {
            GameOverCommand = new StartOverCommand(this);
            SendToDatabase = new SendToDatabaseCommand(this);
            Home = new GoToHomeCommand(this);
            Difficulty = diff;
            this.parent = parent;
            Points = score;
            GetHighscores();
            UserName = "";
            GameOver();
        }

        public void PlayAgain()
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
            List<User> highscoreList = Repo.GetUsers(Difficulty);

            foreach (var user in highscoreList)
            {
                HighscoreView player = new HighscoreView
                {
                    Name = user.UserName,
                    Score = user.Points
                };
                HighScores.Add(player);
            }
        }
        public void SendUser()
        {
            if (UserName != "")
            {
                User user = new User(UserName, int.Parse(Points));
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
