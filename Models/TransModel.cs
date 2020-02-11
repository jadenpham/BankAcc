using System.ComponentModel.DataAnnotations;
using System;

namespace BankAcc.Models
{
    public class Transactions
    {
        [Key]
        public int TransId {get; set;}

        public decimal Amount {get; set;}

        public DateTime CreatedAt {get ;set;}

        public int UserId {get; set;}
        
        public Register Creator {get; set;}
    }
}