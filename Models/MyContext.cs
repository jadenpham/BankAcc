using Microsoft.EntityFrameworkCore;

namespace BankAcc.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) {}

        public DbSet<Register> BankUsers {get; set;}
        public DbSet<Transactions> BankTransactions {get; set;}
    }
}