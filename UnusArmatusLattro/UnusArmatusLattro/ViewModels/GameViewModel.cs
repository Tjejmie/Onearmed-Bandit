using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using UnusArmatusLattro.Commands;
using UnusArmatusLattro.Data;
using UnusArmatusLattro.Models;
using UnusArmatusLattro.Repositories;
using UnusArmatusLattro.Views;
using System.Windows.Threading;
using System.Windows.Media;
using System.Linq;
using System.Threading.Tasks;

namespace UnusArmatusLattro.ViewModels
{

    public class GameViewModel : BaseViewModel
    {
        private readonly MainViewModel parent;
        public ObservableCollection<Slots> SlotMachine { get; }
        public ObservableCollection<HighscoreView> HighScores { get; set; }
        private static readonly Random random = new Random();
        public ICommand Spin { get; }
        public ICommand sendToDatabase { get; }
        public ICommand Home { get; }
        
        
        public Dictionary<Symbol, string> symbols { get; set; }
        public UserRepository Repo { get; set; } = new UserRepository();
       
        public string Score { get; set; }

        public LeverButton LeverObj { get; set; } = new LeverButton();
        public int RemainingSpins { get; set; } = 10;
        public string GameOverState { get; set; } = "Visible";
        public string ShowScoreToAdd { get; set; } = "Hidden";
        private int CurrentSlot { get; set; } = 0;
        public DispatcherTimer Timer { get; set; }
        public bool IsGameOver { get; set; }
        public Difficulties Difficulty { get; set; }
        
        public bool StopBtnEnabled { get; set; } = false;

        public string ScoreToAdd { get; set; }
        public int Cols { get; set; }
        public GameViewModel(MainViewModel parent, Difficulties diff)
        {
            this.parent = parent;
            Home = new GoToHomeCommand(this);
            GenerateDictionary();
            SlotMachine = new ObservableCollection<Slots>();
            Difficulty = diff;
            FillSlots();
            GetHighscores();
            Spin = new SpinCommand(this);
            Score = "0";
            sendToDatabase = new sendToDatabase(this);
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(OnTimedEvent);
            Timer.Interval = TimeSpan.FromMilliseconds((int)diff);
            
        }


        private void OnTimedEvent(Object source, EventArgs e)
        {

            SlotMachine[CurrentSlot].BorderColor = Brushes.Blue;
            
            
                int value = random.Next(1, 7);
                int num = int.Parse(SlotMachine[CurrentSlot].number);
                
                while (num == value)
                {
                    value = random.Next(1, 7);
                }
                var enumValue = (Symbol)value;
                SlotMachine[CurrentSlot].number = value.ToString();
                SlotMachine[CurrentSlot].ImageSource = symbols[enumValue];
            
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
            FillSlotsByDifficulty();
            for (int i = 0; i < Cols; i++)
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

        public void FillSlotsByDifficulty()
        {
            switch (Difficulty)
            {
                case Difficulties.Easy:
                    Cols = 3;
                    break;
                case Difficulties.Normal:
                    Cols = 4;
                    break;
                case Difficulties.Hard:
                    Cols = 5;
                    break;
                default:
                    break;
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

        public void SpinSlots()
        {

            if (!IsGameOver)
            {
                SlotMachine[CurrentSlot].BorderColor = Brushes.Gray;
                CurrentSlot++;
                if (CurrentSlot == SlotMachine.Count)
                {
                    Timer.Stop();
                    RemainingSpins -= 1;
                    ScoreToAdd = $"+{CalculateScore()}";
                    Score = $"{int.Parse(Score) + CalculateScore()}";
                    CurrentSlot = 0;

                    if (RemainingSpins == 0)
                        GameOver();
                    else
                    {
                        SlotMachine[CurrentSlot].BorderColor = Brushes.Blue;
                    }
                }
            }
        }
        public void GoToGameOver()
        {
            parent.CurrentViewModel = new GameOverViewModel(parent, Score, Difficulty);
        }


    

        private void GameOver()
        {
            GameOverState = "Hidden";
            IsGameOver = true;

            Timer.Stop();
            GoToGameOver();
        }

        public int CalculateScore()
        {
            List<string> bestScore = new List<string>();
            int total = 0;

            Dictionary<string, int> scoreDictionary = new Dictionary<string, int>();
            scoreDictionary.Add("1", 0);
            scoreDictionary.Add("2", 0);
            scoreDictionary.Add("3", 0);
            scoreDictionary.Add("4", 0);
            scoreDictionary.Add("5", 0);
            scoreDictionary.Add("6", 0);

            for (int i = 1; i < scoreDictionary.Count+1; i++)
            {
                var currentCount = SlotMachine.Where(t => t.number == $"{i}");
                scoreDictionary[$"{i}"] = currentCount.Count();
            }

            bool hasPair = false;
            bool hasThreeOfAKind = false;
            int tempScore = 0;
            foreach (var item in scoreDictionary)
            {
                if(item.Value == 2)
                {
                    if (hasPair)
                    {
                        RemainingSpins++;
                    }
                    else
                    {
                        hasPair = true;
                    }
                    tempScore += int.Parse(item.Key) * 100;
                }
                else if (item.Value == Cols)
                {
                    return total += 1000000;
                }
                else if (item.Value == 3)
                {
                    hasThreeOfAKind = true;
                    tempScore += int.Parse(item.Key) * 1000;
                }
                else if (item.Value == 4)
                {
                    tempScore += int.Parse(item.Key) * 10000;
                }
            }

            if(hasPair && hasThreeOfAKind)
            {
                RemainingSpins++;
                return total += tempScore * 2;
            }

            return total += tempScore;

        }

        public void StartTimer()
        {
            Timer.Start();
            StopBtnEnabled = true;
        }
        public void GoHome()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }

    }
}

