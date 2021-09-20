using System;
using System.Collections.Generic;
using System.Text;
using UnusArmatusLattro.Models;
using UnusArmatusLattro.Repositories;

namespace UnusArmatusLattro.ViewModels
{
    public class HighscoreViewModel : BaseViewModel
    {
        //  public List<Username> ScoreList { get; set; }
        UserRepository db = new UserRepository();

        public HighscoreViewModel()
        {
            //  ScoreList = db.GetUsers();
        }

        //internal void GoToMenu()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
