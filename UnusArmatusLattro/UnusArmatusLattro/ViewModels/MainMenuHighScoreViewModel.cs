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
        public UserRepository Repo { get; set; } = new UserRepository();
        public ICommand HomeCommand { get; set; }
        private readonly MainViewModel parent;

        public ObservableCollection<HighscoreView> GetHighscores(Data.Difficulties difficulty)
        {
            ObservableCollection<HighscoreView> highscoreViews = new ObservableCollection<HighscoreView>();
            List<Username> templist = Repo.GetUsers(difficulty);

            foreach (var user in templist)
            {
                HighscoreView temp = new HighscoreView
                {
                    Name = user.Name,
                    Score = user.Points
                };
                highscoreViews.Add(temp);
            }

            return highscoreViews;



        }
        public MainMenuHighScoreViewModel(MainViewModel parent)
        {
            Easy = GetHighscores(Data.Difficulties.Easy);
            Normal = GetHighscores(Data.Difficulties.Normal);
            Hard = GetHighscores(Data.Difficulties.Hard);
            this.parent = parent;
            HomeCommand = new HighScoreCommand(this);
            
        }

        public void GoToMenu()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }
    }
}
