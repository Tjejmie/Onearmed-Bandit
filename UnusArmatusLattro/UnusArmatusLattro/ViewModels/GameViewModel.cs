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
        public GameViewModel()
        {
            SlotMachine = new ObservableCollection<Slots>();
            FillSlots();
            Spin = new SpinCommand(this);
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
            foreach (var slot in SlotMachine)
            {
                slot.number = GenerateRandomNumber();
            }
        }

        private string GenerateRandomNumber()
        {
            return $"{random.Next(1, 10)}";
        }

    }
}
