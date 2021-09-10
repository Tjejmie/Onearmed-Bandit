﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using UnusArmatusLattro.Commands;
using UnusArmatusLattro.Data;
using UnusArmatusLattro.Views;
using System.Windows.Threading;
using System.Windows.Media;

namespace UnusArmatusLattro.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public ObservableCollection<Slots> SlotMachine { get; }
        private static readonly Random random = new Random();
        public ICommand Spin { get; }
        public string Score { get; set; }
        public string User { get; set; }
        public int RemainingSpins { get; set; } = 10;
        public string GameOver { get; set; } = "Visible";
        public Dictionary<Symbol, string> symbols { get; set; }
        private int CurrentSlot { get; set; } = 0;
        public DispatcherTimer Timer { get; set; }
        public GameViewModel()
        {
            GenerateDictionary();
            SlotMachine = new ObservableCollection<Slots>();
            FillSlots();
            Spin = new SpinCommand(this);
            Score = "0"; //metod
            User = "user"; //metod

            //var timer = new System.Timers.Timer(1000);
            //timer.Elapsed += OnTimedEvent;
            //timer.AutoReset = true;
            //timer.Enabled = true;

            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(OnTimedEvent);
            Timer.Interval = TimeSpan.FromMilliseconds(300);
            Timer.Start();
        }

    private void OnTimedEvent(Object source, EventArgs e)
        {

            SlotMachine[CurrentSlot].BorderColor = Brushes.Yellow;
            for (int i = CurrentSlot; i < SlotMachine.Count; i++)
            {
                var enumValue = (Symbol)random.Next(1, 7);
                int value = (int)enumValue;
                SlotMachine[i].number = value.ToString();
                SlotMachine[i].ImageSource = symbols[enumValue];
            }
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
        public void SpinSlots()
        {

            //foreach (var slot in SlotMachine)
            //{
            //    var enumValue = (Symbol)random.Next(1, 7);
            //    int value = (int)enumValue;
            //    slot.number = value.ToString();
            //    slot.ImageSource = symbols[enumValue];
            //}
            SlotMachine[CurrentSlot].BorderColor = Brushes.Gray;
            CurrentSlot++;
            if(CurrentSlot == SlotMachine.Count)
            {
                RemainingSpins -= 1;
                Timer.Stop();
                Score = CalculateScore().ToString();

                CurrentSlot = 0;

                if (RemainingSpins == 0)
                    GameOver = "Hidden";

                Timer.Start();
            }
            SlotMachine[CurrentSlot].BorderColor = Brushes.Yellow;

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



    }
}
