using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTracker.Components.Model

{
    
    public class Debt{
        public int Id { get; set; }
        public int UserID { get; set; }
        public string Source { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string Title { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string Notes { get; set; }

    }

}
