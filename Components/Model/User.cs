using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Components.Model
{
    public class User
    {
        internal List<Debt> Debts;
        internal IEnumerable<object> Transactions;
        private int CurrentUserID { get; set; }
        private int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } 
        public string Email { get; set; }
        public decimal Balance { get; set; }
        public string PreferredCurrency { get; set; }
        public string SelectedCurrency { get; set; } = "USD"; // Default currency

    }


}
