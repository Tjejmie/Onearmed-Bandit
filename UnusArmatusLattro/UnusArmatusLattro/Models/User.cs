using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnusArmatusLattro.Models
{
    public class User
    {
        public string UserName { get; set; }
        public int Points { get; set; }

        public User(string userName, int points)
        {
            UserName = userName;
            Points = points;
        }
    }
}
