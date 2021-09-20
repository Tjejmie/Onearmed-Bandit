using System;
using System.Collections.Generic;
using System.Text;
using UnusArmatusLattro.Models;
using UnusArmatusLattro.Repositories;

namespace UnusArmatusLattro.ViewModels
{
    public class HighscoreViewModel : BaseViewModel
    {
      
        UserRepository db = new UserRepository();

        public HighscoreViewModel()
        {
          
        }

        
    }
}
