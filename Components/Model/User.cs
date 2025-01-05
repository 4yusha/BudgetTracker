using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Components.Model
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } // This will store the hashed password
        public string Email { get; set; }

        public decimal Balance { get; set; } 


    }
}
