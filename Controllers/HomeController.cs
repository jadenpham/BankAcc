using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BankAcc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace BankAcc.Controllers
{
    public class HomeController : Controller
    {
        public MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("register")]
        public IActionResult Register(Register user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.BankUsers.Any(u => u.Email == user.Email))
                {
                
                    ModelState.AddModelError("Email", "Email already in use");
                    return View("Index");
                }
                PasswordHasher<Register> Hasher = new PasswordHasher<Register>();
                user.Pw = Hasher.HashPassword(user, user.Pw);
                dbContext.Add(user);
                dbContext.SaveChanges();
                HttpContext.Session.SetInt32("userid", user.UserId);
                return RedirectToAction("Account");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("account")]
        public IActionResult Account()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
