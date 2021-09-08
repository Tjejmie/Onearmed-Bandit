using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UnusArmatusLattro.Commands;
using UnusArmatusLattro.Data;
using UnusArmatusLattro.Views;

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


        public GameViewModel()
        {
            GenerateDictionary();
            SlotMachine = new ObservableCollection<Slots>();
            FillSlots();
            Spin = new SpinCommand(this);
            Score = ""; //metod
            User = "user"; //metod

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
                Slots temp = new Slots() {
                    number = GenerateRandomNumber(),
                    ImageSource = symbols[(Symbol)random.Next(6)]
                };
                SlotMachine.Add(temp);
            }
        }
        public void SpinSlots()
        {
            RemainingSpins -= 1;

            foreach (var slot in SlotMachine)
            {
                slot.number = GenerateRandomNumber();
                slot.ImageSource = symbols[(Symbol)random.Next(6)];
            }
            Score = CalculateScore().ToString();

            if (RemainingSpins == 0)
                GameOver = "Hidden";
        }

        private string GenerateRandomNumber()
        {
            return $"{random.Next(1, 10)}";
        }

        private int CalculateScore()
        {
            int total = 0;
            foreach (var slot in SlotMachine)
            {
                int score = int.Parse(slot.number);
                total += score;
            }

            return total;
        }



    }
}
