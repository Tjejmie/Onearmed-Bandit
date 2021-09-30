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
        private int CurrentSlot { get; set; } = 0;
        public DispatcherTimer Timer { get; set; }
        public Difficulties Difficulty { get; set; }
        public bool StopBtnEnabled { get; set; } = false;
        public string ScoreToAdd { get; set; }
        public string SpinnToAdd { get; set; }
        public int NumberOfSlots { get; set; }

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
            Spin = new StopSlotCommand(this);
            GenerateDictionary();
            Difficulty = diff;
            FillSlots();
            StopBtnEnabled = false;
            GetHighscores();           
            Score = "0";
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(OnTimedEvent);
            Timer.Interval = TimeSpan.FromMilliseconds((int)diff);         
        }


        private void OnTimedEvent(Object source, EventArgs e)
        {

            int value = random.Next(1, 8);

            while (SlotMachine[CurrentSlot].Number == value)
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

        /// <summary>
        /// Fyller slots i slotmaskinen
        /// </summary>
        public async void FillSlots()
        {
            StopBtnEnabled = false;
            await Task.Delay(1000);
            SlotMachine = new ObservableCollection<Slots>();
            FillSlotsByDifficulty();
            for (int i = 0; i < NumberOfSlots; i++)
            {
                var enumValue = (Symbol)random.Next(1, 8);
                int value = (int)enumValue;
                Slots temp = new Slots() {
                    Number = value,
                    ImageSource = Symbols[enumValue]
                };
                SlotMachine.Add(temp);
            }
            SlotMachine[CurrentSlot].BorderColor = Brushes.Blue;
            SlotMachine[CurrentSlot].BorderSlot = 4;
        }

        /// <summary>
        /// Fyller slots beroende på vilken svårighetsgrad man valt
        /// </summary>
        public void FillSlotsByDifficulty()
        {
            switch (Difficulty)
            {
                case Difficulties.Easy:
                    NumberOfSlots = 3;
                    break;
                case Difficulties.Normal:
                    NumberOfSlots = 4;
                    break;
                case Difficulties.Hard:
                    NumberOfSlots = 5;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Metod som körs när man trycker på stopknappen
        /// </summary>
        public void StopSlot()
        {
            Playeffect(Sounds.Stop);

            SlotMachine[CurrentSlot].BorderColor = Brushes.Gray;
            SlotMachine[CurrentSlot].BorderSlot = 2;
            if (SlotMachine[CurrentSlot].Number == (int)Symbol.Bandit)
            {
                BanditHit();
            }
            else
            {
                CurrentSlot++;
                if (CurrentSlot == SlotMachine.Count)
                {
                    RoundEnd();
                }
                else
                {
                    SlotMachine[CurrentSlot].BorderColor = Brushes.Blue;
                    SlotMachine[CurrentSlot].BorderSlot = 4;
                }
            }
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

        /// <summary>
        /// Räknar ut par
        /// </summary>
        /// <returns></returns>
        private int GetPairScore()
        {
            int score = 0;
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
                    score += pair.Key * 100;
                    hasPairAlready = true;
                }
            }

            return score;
        }

        private int GetTripletScore() => ScoreDictionary.ContainsValue(3) ? (ScoreDictionary.FirstOrDefault(t => t.Value == 3).Key * 1000) : 0;
        private int GetQuadScore() => ScoreDictionary.ContainsValue(4) ? (ScoreDictionary.First(t => t.Value == 4).Key * 10000) : 0;
        private bool Jackpot() => ScoreDictionary.ContainsValue(NumberOfSlots);
        private bool HasFullHouse() => ScoreDictionary.ContainsValue(2) && ScoreDictionary.ContainsValue(3);

        /// <summary>
        /// Räknar ut den toala poängen
        /// </summary>
        /// <returns>poängen</returns>
        public int CalculateScore()
        {
            FillScoreDictionary();

            if (Jackpot())
                return 1000000;

            int score = GetPairScore();
            score += GetTripletScore();

            if (Difficulty == Difficulties.Hard)
                score += GetQuadScore();

            if (HasFullHouse())
            {
                RemainingSpins++;
                SpinnToAdd = $"+1";
                return score * 2;
            }

            return score;
        }

        #endregion

        /// <summary>
        /// Metod som körs när man träffar banditen
        /// </summary>
        private void BanditHit()
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
                FillSlots();
            }
        }
        /// <summary>
        /// Hämtar highscore från databasen
        /// </summary>
        public void GetHighscores()
        {
            HighScores = new ObservableCollection<HighscoreView>();
            List<User> templist = Repo.GetUsers(Difficulty);

            foreach (var user in templist)
            {
                HighscoreView temp = new HighscoreView
                {
                    Name = user.UserName,
                    Score = user.Points
                };
                HighScores.Add(temp);
            }
        }

        /// <summary>
        /// Metod som körs efter varje runda
        /// </summary>
        private void RoundEnd()
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
                FillSlots();
            }
        }

        public void GoToGameOver()
        {
            parent.CurrentViewModel = new GameOverViewModel(parent, Score, Difficulty);
        }

        private void GameOver()
        {
            Timer.Stop();
            GoToGameOver();
        }

        public void StartTimer()
        {
            Timer.Start();
            StopBtnEnabled = true;
            SlotMachine[CurrentSlot].BorderColor = Brushes.Blue;
            SlotMachine[CurrentSlot].BorderSlot = 4;
        }
        public void GoHome()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }

        /// <summary>
        /// Metod för ljudeffekter, spelar upp det ljudet man säger åt den att spela upp
        /// </summary>
        /// <param name="sounds"></param>
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

