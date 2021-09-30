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
using System.Media;

namespace UnusArmatusLattro.ViewModels
{
    public class BettingGameViewModel : BaseViewModel
    {
        private readonly MainViewModel parent;
        public ObservableCollection<Slots> SlotMachine { get; }
        public ObservableCollection<HighscoreView> HighScores { get; set; }
        private static readonly Random random = new Random();
        public ICommand BetCommand { get; }
        public ICommand FinishGame { get; }
        public ICommand Home { get;}
        public string Score { get; set; }
        public Dictionary<Symbol, string> Symbols { get; set; }
        public UserRepository Repo { get; set; } = new UserRepository();
        public bool BettingEnabled { get; set; } = true;
        public string BetLabel { get; set; } = "Lägg ett bet";
        public int Wallet { get; set; } = 100;
        public string CurrentBet { get; set; } = "";
        private int CurrentSlot { get; set; } = 0;
        public DispatcherTimer Timer { get; set; }
        public string ScoreToAdd { get; set; }
        public Difficulties Difficulty { get; set; }
        public int NumberOfSlots { get; set; } = 4;
        public bool StopBtnEnabled { get; set; } = false;
        
        public BettingGameViewModel(MainViewModel parent, Difficulties diff)
        {
            GenerateSymbolsDictionary();
            BetCommand = new BetCommand(this);
            FinishGame = new FinishGameCommand(this);
            Spin = new StopSlotCommand(this);
            Home = new GoToHomeCommand(this);
            SlotMachine = new ObservableCollection<Slots>();
            FillSlots();
            Difficulty = diff;
            this.parent = parent;
            GetHighscores();
            Score = "0";
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(OnTimedEvent);
            Timer.Interval = TimeSpan.FromMilliseconds((int)diff);
        }

        private void GenerateSymbolsDictionary()
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
        private void FillSlots()
        {
            for (int i = 0; i < NumberOfSlots; i++)
            {
                Symbol enumValue = (Symbol)random.Next(1, 8);
                int value = (int)enumValue;
                Slots slot = new Slots()
                {
                    Number = value,
                    ImageSource = Symbols[enumValue]
                };
                SlotMachine.Add(slot);
            }
        }

        /// <summary>
        /// Genererar en slumpad bild på den aktiva slotten och ser till att det ej blir två irad
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(Object source, EventArgs e) 
        {
            int value = random.Next(1, 8);
            while (SlotMachine[CurrentSlot].Number == value)
            {
                value = random.Next(1, 8);
            }
            Symbol enumValue = (Symbol)value;
            SlotMachine[CurrentSlot].Number = value;
            SlotMachine[CurrentSlot].ImageSource = Symbols[enumValue];
        }
        public void StartTimer()
        {
            Timer.Start();
        }
        /// <summary>
        /// Startar ny runda om man har pengar kvar, annars gameover
        /// </summary>

        private void NewRound()
        {
            CurrentSlot = 0;

            if (Wallet <= 0)
            {
                GameOver();
            }
            else
            {
                CurrentBet = "";
                BettingEnabled = true;
                BetLabel = "Lägg ett bet";
                StopBtnEnabled = false;
                Timer.Stop();
                SlotMachine[CurrentSlot].BorderColor = Brushes.Gray;
                SlotMachine[CurrentSlot].BorderSlot = 2;
            }
        }

        /// <summary>
        /// Metod för ljudeffekter, spelar upp det ljudet man säger åt den att spela upp
        /// </summary>
        /// <param name="sounds"></param>
        public void PlayEffect(Sounds sounds)
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

        /// <summary>
        /// Metod som kontrollerar om man lagt ett godkänt bet
        /// </summary>
        /// <param name="bet"></param>
        /// <param name="wallet"></param>
        /// <returns>returnerar godkänt bet eller inte</returns>
        public bool ConfirmBet(String bet, string wallet)
        {
            if (bet == "")
            {
                return false;
            }
            int currentBet = int.Parse(bet);
            if (currentBet != 0 && currentBet <= int.Parse(wallet))
            {
                SlotMachine[CurrentSlot].BorderColor = Brushes.Blue;
                SlotMachine[CurrentSlot].BorderSlot = 4;
                Wallet -= currentBet;
                BettingEnabled = false;
                BetLabel = "Lagt bet";
                StopBtnEnabled = true;
                Timer.Start();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Metod som körs när man trycker på stopknappen (StopSlotCommand)
        /// </summary>
        public void StopSlot()
        {
            PlayEffect(Sounds.Stop);

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
        /// <summary>
        /// Metod som räknar ut poängen
        /// </summary>
        /// <returns></returns>
        private int CalculateScore()
        {
            Dictionary<int, int> scoreDictionary = new Dictionary<int, int>();
            int tempBet = int.Parse(CurrentBet);
            scoreDictionary.Add(1, 0);
            scoreDictionary.Add(2, 0);
            scoreDictionary.Add(3, 0);
            scoreDictionary.Add(4, 0);
            scoreDictionary.Add(5, 0);
            scoreDictionary.Add(6, 0);

            for (int i = 1; i < scoreDictionary.Count + 1; i++)
            {
                var currentCount = SlotMachine.Where(t => t.Number == i);
                scoreDictionary[i] = currentCount.Count();
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
                    tempScore += item.Key * tempBet;
                }
                else if (item.Value == NumberOfSlots)
                {
                    return tempBet + 1000000;
                }
                else if (item.Value == 3)
                {
                    hasThreeOfAKind = true;
                    tempScore += item.Key * tempBet + 100;
                }
                else if (item.Value == 4)
                {
                    tempScore += item.Key * tempBet + 1000;
                }
            }

            if (hasPair && hasThreeOfAKind)
            {
                return tempScore * 2;
            }

            return tempScore;
        }

        /// <summary>
        /// Metod som körs när man träffar banditen
        /// </summary>
        private void BanditHit()
        {
            Timer.Stop();
            ScoreToAdd = $"El bandito";
            CurrentSlot = 0;
            NewRound();
            PlayEffect(Sounds.Bandit);
            if (Wallet == 0)
                GameOver();
            return;
        }

        /// <summary>
        /// Metod som körs efter varje runda
        /// </summary>
        private void RoundEnd()
        {
            int winnings = CalculateScore();

            if (winnings >= 1000000)
            {
                PlayEffect(Sounds.Jackpot);
            }
            else if (winnings > 0)
            {
                PlayEffect(Sounds.Cash);
            }

            ScoreToAdd = $"+{winnings}";
            Wallet += winnings;
            NewRound();

            if (Wallet == 0)
                GameOver();
        }

        /// <summary>
        /// Hämtar highscorelista från databasen
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
        /// Byter från spelvyn till gameovervyn
        /// </summary>
        public void GoToGameOver()
        {
            parent.CurrentViewModel = new GameOverViewModel(parent, $"{Wallet}", Difficulty);
        }

        /// <summary>
        /// Metod som körs när man får gameover
        /// </summary>
        public void GameOver()
        {
            Timer.Stop();
            GoToGameOver();
        }

        /// <summary>
        /// Metod för att gå till startmenyn
        /// </summary>
        public void GoHome()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }

    }
}

