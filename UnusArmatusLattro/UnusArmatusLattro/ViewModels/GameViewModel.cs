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
        public ICommand HomeCommand { get; set; }
        public string Score { get; set; }
        public string User { get; set; }
        public Dictionary<Symbol, string> symbols { get; set; }
        public UserRepository Repo { get; set; } = new UserRepository();
        public string NewHighScore { get; set; } = "Hidden";
        public LeverButton LeverObj { get; set; } = new LeverButton();
        public int RemainingSpins { get; set; } = 10;
        public string GameOverState { get; set; } = "Visible";
        public string ShowScoreToAdd { get; set; } = "Hidden";
        private int CurrentSlot { get; set; } = 0;
        public DispatcherTimer Timer { get; set; }
        public bool IsGameOver { get; set; }
        public Difficulties Difficulty { get; set; }
        public string ScoreToAdd { get; set; }
        public int Cols { get; set; }
        public GameViewModel(MainViewModel parent, Difficulties diff)
        {
            this.parent = parent;
            HomeCommand = new GameToHomeCommand(this);

            GenerateDictionary();
            SlotMachine = new ObservableCollection<Slots>();
            Difficulty = diff;
            FillSlots();
            
            GetHighscores();
            Spin = new SpinCommand(this);
            Score = "0"; //metod
            User = ""; //metod
            sendToDatabase = new sendToDatabase(this);
            
            
            //var timer = new System.Timers.Timer(1000);
            //timer.Elapsed += OnTimedEvent;
            //timer.AutoReset = true;
            //timer.Enabled = true;

            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(OnTimedEvent);
            Timer.Interval = TimeSpan.FromMilliseconds((int)diff);
            //Timer.Start();
        }

        public void GoToMenu()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }

        private void OnTimedEvent(Object source, EventArgs e)
        {

            SlotMachine[CurrentSlot].BorderColor = Brushes.Yellow;
            
            
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
                        SlotMachine[CurrentSlot].BorderColor = Brushes.Yellow;
                    }
                }
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
            if (HighScores.Count < 10)
            {
                NewHighScore = "Visible";
                return true;
            }
            return false;
        }

        private void GameOver()
        {
            GameOverState = "Hidden";
            IsGameOver = true;
            IsHighScore(int.Parse(Score));
            Timer.Stop();
        }

        private int CalculateScore()
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

            //foreach (var slot in SlotMachine)
            //{
            //    List<string> scoreList = new List<string>();
            //    foreach (var slots in SlotMachine)
            //    {
            //        if (slot.number.Equals(slots.number))
            //        {
            //            scoreList.Add(slot.number);
            //        }
            //    }
            //    if (scoreList.Count > bestScore.Count)
            //    {
            //        bestScore = scoreList;
            //    }
            //    else if (scoreList.Count == 2 && bestScore.Count == 2 && !scoreList.Contains(bestScore[0]))
            //    {
            //        total += int.Parse(scoreList[0]) * 100;
            //        total += int.Parse(bestScore[0]) * 100;
            //        RemainingSpins++;
            //        return total;
            //    }

            //    //else if (scoreList.Count == 3 && bestScore.Count == 2 && !scoreList.Contains(bestScore[0]))
            //    //{
            //    //    total += int.Parse(scoreList[0]) * 100;
            //    //    total += int.Parse(bestScore[0]) * 100;
            //    //    RemainingSpins++;
            //    //    return total;
            //    //}
            //    else if (scoreList.Count == 2 && bestScore.Count == 3 && !bestScore.Contains(scoreList[0]))
            //    {
            //        total += int.Parse(scoreList[0]) * 100;
            //        total += int.Parse(bestScore[0]) * 100;
            //        RemainingSpins++;
            //        return total;
            //    }
            //}
            
            //if (bestScore.Count == 2)
            //{
            //    total += int.Parse(bestScore[0]) * 100;
            //}

            //if (bestScore.Count == Cols)
            //{
            //    return total += 1000000;
            //}

            //if (bestScore.Count == 3)
            //{
            //    total += int.Parse(bestScore[0]) * 1000;
            //}

            //if (bestScore.Count == 4)
            //{
            //    total += int.Parse(bestScore[0]) * 100000;
            //}

            //return total;
        }

        public void SendUser()
        {
            User user = new User(User, int.Parse(Score));

            Repo.sendUser(user, Difficulty);
        
        }

        public void StartTimer()
        {
            Timer.Start();
        }

    }
}

