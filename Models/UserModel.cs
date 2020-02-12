using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAcc.Models
{
    public class Register
    {
        [Key]
        public int UserId {get; set;}
        
        [Required(ErrorMessage = "First Name Required")]
        [Display(Name = "First Name")]
        public string FName{get; set;}

        [Required(ErrorMessage = "Last Name Required")]
        [Display(Name = "Last Name")]
        public string LName {get; set;}

        [Required(ErrorMessage = "Email Required")]
        [DataType(DataType.EmailAddress)]
        public string Email {get; set;}

        [Required(ErrorMessage = "Password Required")]
        [Display(Name = "Password")]
        [MinLength(8, ErrorMessage = "Mininum Length is 8 characters")]
        [DataType(DataType.Password)]
        public string Pw {get; set;}

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Pw")]
        [Display(Name = "Confirm Password")]
        public string CPw {get; set;}

        public List<Transactions> Transactions {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;

        public DateTime UpdatedAt {get; set;} = DateTime.Now;

        public decimal Balance {get; set;} = 0;
    }   

    public class Login
    {
        [Required(ErrorMessage = "Login Email Required")]
        [Display(Name = "Email Address")]
        public string LogInEmail {get; set;}

        [Required(ErrorMessage = "Login Password Required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string LogInPw {get; set;}

    }
}