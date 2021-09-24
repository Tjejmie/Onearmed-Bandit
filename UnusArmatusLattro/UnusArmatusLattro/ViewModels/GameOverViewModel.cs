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

        public string txtboxLabel { get; set; } = "";
        public string DisplayInputField { get; set; } = "Hidden";
        public string Points { get; set; }
        public ObservableCollection<HighscoreView> HighScores { get; set; }
        public Difficulties Difficulty { get; set; }
        public UserRepository Repo { get; set; } = new UserRepository();
        
        public string User { get; set; }
        public ICommand sendToDatabase { get; }

        public GameOverViewModel(MainViewModel parent, string score, Difficulties diff)
        {
            Difficulty = diff;
            this.parent = parent;
            GameOverCommand = new GameOverCommand(this);
            Points = score;
            GetHighscores();
            User = "";
            sendToDatabase = new sendToDatabase(this);
            GameOver();
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
            User user = new User(User, int.Parse(Points));

            Repo.sendUser(user, Difficulty);

        }
        private bool IsHighScore(int score)
        {
            foreach (var highScore in HighScores)
            {
                if (score > highScore.Score)
                {
                    txtboxLabel = "Namn:";
                    DisplayInputField = "Visible";
                    return true;
                }
                else
                {
                    txtboxLabel = "Oh noo...Du fick inte plats på topplistan dessvärre.";
                    DisplayInputField = "Hidden";
                    
                }
            }
            if (HighScores.Count < 10)
            {
                txtboxLabel = "Namn:";
                DisplayInputField = "Visible";
                return true;
            }
            return false;
        }

        private void GameOver()
        {
            IsHighScore(int.Parse(Points));
        }
    }



}
