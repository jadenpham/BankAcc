using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
namespace BankAcc.Models
{
    public class Transactions
    {
        [Key]
        public int TransId {get; set;}

        [Required(ErrorMessage = "Please input an amount")]
        public decimal Amount {get; set;}

        public DateTime CreatedAt {get ;set;} = DateTime.Now;
        public DateTime UpdatedAt {get ;set;} = DateTime.Now;

        public int UserId {get; set;}

        public Register Creator {get; set;}
    }

    // public class TransactionsCalc
    // {
    //     public decimal BankTotal(List<Transactions> trans)
    //     {
    //         decimal total = 0;
    //         foreach(var amount in )
    //         {
    //             // total += amount;
    //             System.Console.WriteLine(amount);
    //         }
    //         return total;
    //     }
    // }
}