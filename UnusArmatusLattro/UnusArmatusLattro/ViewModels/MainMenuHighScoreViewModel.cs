using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.Commands;
using UnusArmatusLattro.Models;
using UnusArmatusLattro.Repositories;
using UnusArmatusLattro.Views;

namespace UnusArmatusLattro.ViewModels
{
    public class MainMenuHighScoreViewModel : BaseViewModel
    {
        public ObservableCollection<HighscoreView> Easy { get; set; }
        public ObservableCollection<HighscoreView> Normal  { get; set; }
        public ObservableCollection<HighscoreView> Hard { get; set; }
        public ObservableCollection<HighscoreView> Betting { get; set; }
        public UserRepository Repo { get; set; } = new UserRepository();
        
        private readonly MainViewModel parent;
        public ICommand Home { get; }

        /// <summary>
        /// Visar highscorelista utifrån svårighetsgrad och spel
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public ObservableCollection<HighscoreView> GetHighscores(Data.Difficulties difficulty)
        {
            ObservableCollection<HighscoreView> highscoreViews = new ObservableCollection<HighscoreView>();
            List<User> highscoreList = Repo.GetUsers(difficulty);

            foreach (var user in highscoreList)
            {
                HighscoreView player = new HighscoreView
                {
                    Name = user.UserName,
                    Score = user.Points
                };
                highscoreViews.Add(player);
            }
            return highscoreViews;
        }
        public MainMenuHighScoreViewModel(MainViewModel parent)
        {
            Easy = GetHighscores(Data.Difficulties.Easy);
            Normal = GetHighscores(Data.Difficulties.Normal);
            Hard = GetHighscores(Data.Difficulties.Hard);
            Betting = GetHighscores(Data.Difficulties.Betting);
            this.parent = parent;
            Home = new GoToHomeCommand(this);
        }

        public void GoHome()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }
    }
}
