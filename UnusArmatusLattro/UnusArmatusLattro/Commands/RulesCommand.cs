using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Commands
{
    public class RulesCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private RulesViewModel ruleViewModel;

        public RulesCommand(RulesViewModel startViewModel)
        {
            this.ruleViewModel = startViewModel;
        }


        public bool CanExecute(object parameter) => true;
        

        public void Execute(object parameter)
        {
            ruleViewModel.GoToMenu();
        }
    }
}
