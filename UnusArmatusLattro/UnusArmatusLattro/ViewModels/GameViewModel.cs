using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UnusArmatusLattro.Commands;
using UnusArmatusLattro.Data;
using UnusArmatusLattro.Models;
using UnusArmatusLattro.Repositories;
using UnusArmatusLattro.Views;

namespace UnusArmatusLattro.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public ObservableCollection<Slots> SlotMachine { get; }
        public ObservableCollection<HighscoreView> HighScores { get; set; }
        private static readonly Random random = new Random();
        public ICommand Spin { get; }
        public ICommand sendToDatabase { get; }
        public string Score { get; set; }
        public string User { get; set; }
        public int RemainingSpins { get; set; } = 20;
        public string GameOver { get; set; } = "Visible";
        public Dictionary<Symbol, string> symbols { get; set; }
        public UserRepository Repo { get; set; } = new UserRepository();
        public string NewHighScore { get; set; } = "Hidden";

        public GameViewModel()
        {
            GenerateDictionary();
            SlotMachine = new ObservableCollection<Slots>();
           
            FillSlots();
            GetHighscores();
            Spin = new SpinCommand(this);
            Score = "0"; //metod
            User = ""; //metod
            sendToDatabase = new sendToDatabase(this);
        }
        private void GenerateDictionary()
        {
            symbols = new Dictionary<Symbol, string>();
            symbols.Add(Symbol.Cherry, "/Resources/Images/cherries.png");
            symbols.Add(Symbol.Lemon, "/Resources/Images/lemon.png");
            symbols.Add(Symbol.Grapes, "/Resources/Images/grapes.png");
            symbols.Add(Symbol.Banana, "/Resources/Images/banana.png");
            symbols.Add(Symbol.Apple, "/Resources/Images/apple.png");
            symbols.Add(Symbol.Strawberry, "/Resources/Images/strawberry.png");
        }

        private void FillSlots()
        {   

            for (int i = 0; i < 4; i++)
            {
                var enumValue = (Symbol)random.Next(1, 7);
                int value = (int)enumValue;
                Slots temp = new Slots() {
                    number = value.ToString(),
                    ImageSource = symbols[enumValue]
                };
                SlotMachine.Add(temp);
            }

        }

        public void GetHighscores()
        {
            HighScores = new ObservableCollection<HighscoreView>();
            List<Username> templist = Repo.GetUsers();

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

        public void SpinSlots()
        {
            RemainingSpins -= 1;

            foreach (var slot in SlotMachine)
            {
                var enumValue = (Symbol)random.Next(1, 7);
                int value = (int)enumValue;
                slot.number = value.ToString();
                slot.ImageSource = symbols[enumValue];
            }
            Score = CalculateScore().ToString();

            if (RemainingSpins == 0)
            {
                GameOver = "Hidden";
                IsHighScore(int.Parse(Score));
            }
        }

        private bool IsHighScore(int score)
        {
            foreach (var highScore in HighScores)
            {
                if (score > highScore.Score)
                {
                    NewHighScore = "Visible";
                    return true;
                }

            }
            if (HighScores.Count == 0)
            {
                NewHighScore = "Visible";
                return true;
            }
            return false;

           
        }

        //private string GenerateRandomNumber()
        //{
        //    return $"{random.Next(1, 4)}";
        //}

        private int CalculateScore()
        {
            List<string> bestScore = new List<string>();
            int total = int.Parse(Score);
            foreach (var slot in SlotMachine)
            {
                List<string> scoreList = new List<string>();
                foreach (var slots in SlotMachine)
                {
                    if (slot.number.Equals(slots.number))
                    {
                        scoreList.Add(slot.number);
                    }
                }
                if (scoreList.Count > bestScore.Count)
                {
                    bestScore = scoreList;

                }
                else if (scoreList.Count == 2 && bestScore.Count == 2 && !scoreList.Contains(bestScore[0]))
                {
                    total += int.Parse(scoreList[0]) * 100;
                    total += int.Parse(bestScore[0]) * 100;
                    RemainingSpins++;
                    return total;
                }
            }
            
            if (bestScore.Count == 2)
            {

               total += int.Parse(bestScore[0]) *100; 

            }

            if (bestScore.Count == 3)
            {

                total += int.Parse(bestScore[0]) * 1000;

            }

            if (bestScore.Count == 4)
            {

                total += 1000000;

            }

            return total;
        }

        public void SendUser()
        {
            User user = new User(User, int.Parse(Score));

            Repo.sendUser(user);
        
        }

    }
}
