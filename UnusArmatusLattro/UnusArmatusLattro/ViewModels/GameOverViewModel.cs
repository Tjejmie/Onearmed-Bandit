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
        public string TextboxLabel { get; set; }
        public string DisplayInputField { get; set; }


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
            IsHighScore(int.Parse(Points));
        }

        /// <summary>
        /// Metod för att kunna välja att spela igen
        /// </summary>
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

        /// <summary>
        /// Metod för att hämta highscore från databasen
        /// </summary>
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

        /// <summary>
        /// Kontrollerar om man kvalificerar sig på highscorelistan
        /// </summary>
        /// <param name="score"></param>
        /// <returns></returns>
        private void IsHighScore(int score)
        {
            foreach (var highScore in HighScores)
            {
                if (score > highScore.Score)
                {
                    TextboxLabel = "Namn:";
                    DisplayInputField = "Visible";
                    return;
                }
                else
                {
                    TextboxLabel = "Oh no, du fick tyvärr inte plats på topplistan";
                    DisplayInputField = "Hidden";
                }
            }
            if (HighScores.Count < 10)
            {
                TextboxLabel = "Namn:";
                DisplayInputField = "Visible";
                return;
            }
            return;
        }

        /// <summary>
        /// Metod som skickar spelaren till highscore om textrutan inte är tom 
        /// </summary>
        public void SendUser()
        {
            if (UserName != "")
            {
                User user = new User(UserName, int.Parse(Points));
                Repo.SendUser(user, Difficulty);
                InputAccepted = false;
            }
        }

  
        public void GoHome()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }
    }
}
