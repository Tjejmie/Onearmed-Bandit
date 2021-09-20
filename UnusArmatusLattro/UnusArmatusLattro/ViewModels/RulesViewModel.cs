using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UnusArmatusLattro.Commands;

namespace UnusArmatusLattro.ViewModels
{
    public class RulesViewModel : BaseViewModel
    {
        public ICommand HomeCommand { get; }
        private readonly MainViewModel parent;

        public RulesViewModel(MainViewModel parent)
        {
            this.parent = parent;
            HomeCommand = new RulesCommand(this);
        }
        
        public void GoToMenu()
        {
            parent.CurrentViewModel = new StartViewModel(parent);
        }
    }
}
