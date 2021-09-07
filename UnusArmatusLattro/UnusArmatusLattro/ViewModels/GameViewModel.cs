using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnusArmatusLattro.Views;

namespace UnusArmatusLattro.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public ObservableCollection<Slots> SlotMachine { get; }

        public GameViewModel()
        {
            SlotMachine = new ObservableCollection<Slots>
            {
                new Slots(),
                new Slots(),
                new Slots(),
                new Slots()

            };
        }

    }
}
