﻿using System;
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
using System.Media;

namespace UnusArmatusLattro.ViewModels
{

    public class GameViewModel : BaseViewModel
    {
        private readonly MainViewModel parent;
        public ObservableCollection<Slots> SlotMachine { get; set; }
        public ObservableCollection<HighscoreView> HighScores { get; set; }
        private static readonly Random random = new Random();
        public ICommand Home { get; }
        public Dictionary<Symbol, string> Symbols { get; set; }
        public UserRepository Repo { get; set; } = new UserRepository();
        public string Score { get; set; }
        public int RemainingSpins { get; set; } = 10;
        public string GameOverState { get; set; } = "Visible";
        private int CurrentSlot { get; set; } = 0;
        public DispatcherTimer Timer { get; set; }
        public bool IsGameOver { get; set; }
        public Difficulties Difficulty { get; set; }
        public bool StopBtnEnabled { get; set; } = false;
        public string ScoreToAdd { get; set; }
        public string SpinnToAdd { get; set; }
        public int Cols { get; set; }
        private readonly Dictionary<int, int> ScoreDictionary = new Dictionary<int, int>() {
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 },
            { 6, 0 },
            };
        public GameViewModel(MainViewModel parent, Difficulties diff)
        {
            this.parent = parent;
            Home = new GoToHomeCommand(this);
            GenerateDictionary();
            Difficulty = diff;
            FillSlots();
            StopBtnEnabled = false;
            GetHighscores();
            Spin = new SpinCommand(this);
            Score = "0";
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(OnTimedEvent);
            Timer.Interval = TimeSpan.FromMilliseconds((int)diff);
            
        }


        private void OnTimedEvent(Object source, EventArgs e)
        {

            SlotMachine[CurrentSlot].BorderColor = Brushes.Blue;
            
            
                int value = random.Next(1, 8);
                int num = SlotMachine[CurrentSlot].Number;
                
                while (num == value)
                {
                    value = random.Next(1, 8);
                }
                var enumValue = (Symbol)value;
                SlotMachine[CurrentSlot].Number = value;
                SlotMachine[CurrentSlot].ImageSource = Symbols[enumValue];
            
        }
    private void GenerateDictionary()
        {
            Symbols = new Dictionary<Symbol, string>
            {
                { Symbol.Cherry, "/Resources/Images/cherries.png" },
                { Symbol.Lemon, "/Resources/Images/lemon.png" },
                { Symbol.Grapes, "/Resources/Images/grapes.png" },
                { Symbol.Banana, "/Resources/Images/banana.png" },
                { Symbol.Apple, "/Resources/Images/apple.png" },
                { Symbol.Strawberry, "/Resources/Images/strawberry.png" },
                { Symbol.Bandit, "/Resources/Images/bandit.png" }
            };
        }

        public async void FillSlots()
        {
            StopBtnEnabled = false;
            await Task.Delay(1000);
            SlotMachine = new ObservableCollection<Slots>();
            FillSlotsByDifficulty();
            for (int i = 0; i < Cols; i++)
            {
                var enumValue = (Symbol)random.Next(1, 8);
                int value = (int)enumValue;
                Slots temp = new Slots() {
                    Number = value,
                    ImageSource = Symbols[enumValue]
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
            Playeffect(Sounds.Stop);
            if (!IsGameOver)
            {
                SlotMachine[CurrentSlot].BorderColor = Brushes.Gray;
                if(SlotMachine[CurrentSlot].Number == (int)Symbol.Bandit)
                {
                    Timer.Stop();
                    RemainingSpins--;
                    ScoreToAdd = $"El bandito";
                    Playeffect(Sounds.Bandit);
                    CurrentSlot = 0;

                    if (RemainingSpins == 0)
                        GameOver();
                    else
                    {
                        SlotMachine[CurrentSlot].BorderColor = Brushes.Blue;
                    }
                    
                    FillSlots();
                    
                    return;
                }
                CurrentSlot++;
                if (CurrentSlot == SlotMachine.Count)
                {
                    Timer.Stop();
                    RemainingSpins -= 1;
                    int Winnings = CalculateScore();
                    
                    if (Winnings >= 1000000)
                    {
                        Playeffect(Sounds.Jackpot);
                    }
                    else if (Winnings > 0)
                    {
                        Playeffect(Sounds.Cash);
                    }

                    ScoreToAdd = $"+{Winnings}";
                    Score = $"{int.Parse(Score) + Winnings}";
                    CurrentSlot = 0;

                    if (RemainingSpins == 0)
                        GameOver();
                    else
                    {
                        SlotMachine[CurrentSlot].BorderColor = Brushes.Blue;
                    }
                    
                    FillSlots();
                    
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

    #region Score calculation
        private void FillScoreDictionary()
        {
            for (int i = 1; i < ScoreDictionary.Count + 1; i++)
            {
                var currentCount = SlotMachine.Where(t => t.Number == i);
                ScoreDictionary[i] = currentCount.Count();
            }
        }

        private int GetPairScore()
        {
            int temp = 0;
            bool hasPairAlready = false;
            foreach (var pair in ScoreDictionary)
            {
                if (pair.Value == 2)
                {
                    if (hasPairAlready)
                    {
                        RemainingSpins++;
                        SpinnToAdd = $"+1";
                    }
                    temp += pair.Key * 100;
                    hasPairAlready = true;
                }
            }

            return temp;
        }

        private int GetTripletScore() => ScoreDictionary.ContainsValue(3) ? (ScoreDictionary.FirstOrDefault(t => t.Value == 3).Key * 1000) : 0;
        private int GetQuadScore() => ScoreDictionary.ContainsValue(4) ? (ScoreDictionary.First(t => t.Value == 4).Key * 10000) : 0;
        private bool Jackpot() =>  ScoreDictionary.ContainsValue(Cols);
        private bool HasFullHouse() => ScoreDictionary.ContainsValue(2) && ScoreDictionary.ContainsValue(3);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int CalculateScore()
        {
            FillScoreDictionary();

            if (Jackpot())
                return 1000000;

            int tempScore = GetPairScore();
            tempScore += GetTripletScore();

            if (Difficulty == Difficulties.Hard)
                tempScore += GetQuadScore();

            if(HasFullHouse())
            {
                RemainingSpins++;
                SpinnToAdd = $"+1";
                return tempScore * 2;
            }

            return tempScore;
        }

    #endregion

        public void StartTimer()
        {
            Timer.Start();
            StopBtnEnabled = true;
        }
        public void GoHome()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }

        public void Playeffect(Sounds sounds)
        {
            System.IO.Stream sound = sounds switch
            {
                Sounds.Bandit => Resources.Resource1.sm64_whomp,
                Sounds.Lever => Resources.Resource1.LeverPush,
                Sounds.Cash => Resources.Resource1.win,
                Sounds.Jackpot => Resources.Resource1.jackpot,
                Sounds.Stop => Resources.Resource1.LeverPull,
                _ => null,
            };
            SoundPlayer player = new SoundPlayer(sound);
            player.Play();
        }
    }
}

