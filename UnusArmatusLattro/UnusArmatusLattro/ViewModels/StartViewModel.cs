﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnusArmatusLattro.Commands;
using UnusArmatusLattro.Data;

namespace UnusArmatusLattro.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        private readonly MainViewModel parent;

        public StartViewModel(MainViewModel parent)
        {
            this.parent = parent;
            ChangeView = new ChangeViewCommand(this);
        }
        public ICommand ChangeView { get; set; }
        
        public void StartGame(Difficulties diff)
        {
            parent.CurrentViewModel = new GameViewModel(parent, diff);
        }

        public void Rules()
        {
            parent.CurrentViewModel = new RulesViewModel(parent);
        }
        public void Highscore()
        {
            parent.CurrentViewModel = new MainMenuHighScoreViewModel(parent);
        }

        public void SpinGame()
        {
            parent.CurrentViewModel = new SpinGameViewModel(parent);
        }

        public void BettingGame(Difficulties diff)
        {
            parent.CurrentViewModel = new BettingGameViewModel(parent, diff);
        }


    }
}
