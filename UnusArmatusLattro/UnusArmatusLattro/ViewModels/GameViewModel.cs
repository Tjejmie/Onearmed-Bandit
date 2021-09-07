using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UnusArmatusLattro.Commands;
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

        bool GameOver = false;

        public GameViewModel()
        {
            SlotMachine = new ObservableCollection<Slots>();
            FillSlots();
            Spin = new SpinCommand(this);
            Score = ""; //metod
            User = "user"; //metod
            

        }

        private void FillSlots()
        {   
            for (int i = 0; i < 4; i++)
            {
                Slots temp = new Slots() { number = GenerateRandomNumber() };
                SlotMachine.Add(temp);
            }
        }
        public void SpinSlots()
        {
            RemainingSpins -= 1;

            foreach (var slot in SlotMachine)
            {
                slot.number = GenerateRandomNumber();
                
            }
            Score = CalculateScore().ToString();
        }

        private string GenerateRandomNumber()
        {
            return $"{random.Next(1, 11)}";
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
