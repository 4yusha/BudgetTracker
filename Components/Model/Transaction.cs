using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Components.Model

{
    //public class Data
    //{
    //    public static IEnumerable<object> Debts { get; internal set; }
    //    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    //}

    public class Transaction{
        public int Id { get; set; }
        public int UserID { get; set; }
        public int DebtId { get; set; }
        public string Type { get; set; } // "Credit", "Debit", or "Debt"
        public decimal Amount { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public DateTime Date { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string Notes { get; set; }
        public string Title { get; set; } 


    }

}

