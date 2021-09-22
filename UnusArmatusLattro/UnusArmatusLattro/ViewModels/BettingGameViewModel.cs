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
    public class BettingGameViewModel : BaseViewModel
    {
        private readonly MainViewModel parent;
        public ObservableCollection<Slots> SlotMachine { get; }
        public ObservableCollection<HighscoreView> HighScores { get; set; }
        private static readonly Random random = new Random();
        public ICommand Spin { get; }
        public ICommand sendToDatabase { get; }
        public ICommand BetCommand { get; }
        public ICommand FinishGame { get; }
        public string Score { get; set; }
        public string User { get; set; }
        public Dictionary<Symbol, string> symbols { get; set; }
        public UserRepository Repo { get; set; } = new UserRepository();
        public string NewHighScore { get; set; } = "Hidden";
        public bool BettingEnabled { get; set; } = true;
        public string BetLabel { get; set; } = "Lägg ett bet";
        public int Wallet { get; set; } = 100;
        public string CurrentBet { get; set; } = "0";
        public string GameOverState { get; set; } = "Hidden";
        public string BetBtn { get; set; } = "Visible";
        private int CurrentSlot { get; set; } = 0;
        public DispatcherTimer Timer { get; set; }
        public bool IsGameOver { get; set; }
        public string ScoreToAdd { get; set; }
        public Difficulties Difficulty { get; set; }
        public int Cols { get; set; } = 4;
        public bool StopBtnEnabled { get; set; } = false;
        public ICommand Home { get;}

        public BettingGameViewModel(MainViewModel parent, Difficulties diff)
        {
            GenerateDictionary();
            BetCommand = new BetCommand(this);
            SlotMachine = new ObservableCollection<Slots>();
            FillSlots();
            Difficulty = diff;
            this.parent = parent;
            GetHighscores();
            Spin = new SpinCommand(this);
            Score = "0";
            sendToDatabase = new sendToDatabase(this);
            FinishGame = new FinishGame(this);
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(OnTimedEvent);
            Timer.Interval = TimeSpan.FromMilliseconds((int)diff);
            Home = new GoToHomeCommand(this);
           

        }

        private void OnTimedEvent(Object source, EventArgs e)
        {

            SlotMachine[CurrentSlot].BorderColor = Brushes.Red;


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

            for (int i = 0; i < Cols; i++)
            {
                var enumValue = (Symbol)random.Next(1, 7);
                int value = (int)enumValue;
                Slots temp = new Slots()
                {
                    number = value.ToString(),
                    ImageSource = symbols[enumValue]
                };
                SlotMachine.Add(temp);
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
                    Wallet -= int.Parse(CurrentBet);
                    ScoreToAdd = $"+{CalculateScore()}";
                    Wallet = Wallet + CalculateScore();
                    NewRound();

                    if (Wallet == 0)
                        GameOver();
                    else
                    {
                        SlotMachine[CurrentSlot].BorderColor = Brushes.Red;
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

        public void GoToGameOver()
        {
            parent.CurrentViewModel = new GameOverViewModel(parent, $"{Wallet}", Difficulty);
        }

        public void GameOver()
        {
            GameOverState = "Hidden";
            IsGameOver = true;
            IsHighScore(Wallet);
            Timer.Stop();
            GoToGameOver();
        }

        private int CalculateScore()
        {
            //List<string> bestScore = new List<string>();
            int total = Wallet;
            Dictionary<string, int> scoreDictionary = new Dictionary<string, int>();
            int tempBet = int.Parse(CurrentBet);
            scoreDictionary.Add("1", 0);
            scoreDictionary.Add("2", 0);
            scoreDictionary.Add("3", 0);
            scoreDictionary.Add("4", 0);
            scoreDictionary.Add("5", 0);
            scoreDictionary.Add("6", 0);

            for (int i = 1; i < scoreDictionary.Count + 1; i++)
            {
                var currentCount = SlotMachine.Where(t => t.number == $"{i}");
                scoreDictionary[$"{i}"] = currentCount.Count();
            }

            bool hasPair = false;
            bool hasThreeOfAKind = false;
            int tempScore = 0;
            foreach (var item in scoreDictionary)
            {
                if (item.Value == 2)
                {
                    if (hasPair)
                    {
                        tempScore += tempBet * 10;
                    }
                    else
                    {
                        hasPair = true;
                    }
                    tempScore += int.Parse(item.Key) * tempBet;
                }
                else if (item.Value == Cols)
                {
                    return total = tempBet + 1000000;
                }
                else if (item.Value == 3)
                {
                    hasThreeOfAKind = true;
                    tempScore += int.Parse(item.Key) * tempBet + 100;
                }
                else if (item.Value == 4)
                {
                    tempScore += int.Parse(item.Key) * tempBet + 1000;
                }
            }

            if (hasPair && hasThreeOfAKind)
            {
                return total += tempScore * 2;
            }

            return total += tempScore;
        }

        public void SendUser()
        {
            User user = new User(User, int.Parse(Score));

            Repo.sendUser(user, Difficulty);

        }

        public bool ConfirmBet(String bet, string wallet)
        {
            int tempBet = int.Parse(bet);
            if (tempBet != 0 && tempBet <= int.Parse(wallet))
            {
                BettingEnabled = false;
                BetLabel = "Lagt bet";
                GameOverState = "Visible";
                BetBtn = "Hidden";
                StopBtnEnabled = true;
                Timer.Start();
                return true;
            }
            return false;

        }
        private void NewRound()
        {
            CurrentSlot = 0;

            if (Wallet <= 0)
            {
                GameOver();
            }
            else
            {
                CurrentBet = "0";
                BettingEnabled = true;
                BetLabel = "Lägg ett bet";
                GameOverState = "Hidden";
                BetBtn = "Visible";
                StopBtnEnabled = false;
                Timer.Stop();
            }

        }

        public void StartTimer()
        {
            Timer.Start();
        }

        public void GoHome()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }

    }
}

