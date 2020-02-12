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
using Microsoft.EntityFrameworkCore;

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
                return RedirectToAction("Account", new {id = user.UserId});
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(Login userLogin)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.BankUsers.FirstOrDefault(u => u.Email == userLogin.LogInEmail);
                if(userInDb == null)
                {
                    //unhash pw n match
                    ModelState.AddModelError("LogInEmail", "Email or password doesn't match");
                    return View("Index");
                }
                var hasher = new PasswordHasher<Login>();
                var result = hasher.VerifyHashedPassword(userLogin, userInDb.Pw, userLogin.LogInPw);
                if(result == 0)
                {
                    ModelState.AddModelError("LogInPw", "Email or password doesn't match");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("userid", userInDb.UserId);
                ViewBag.userInfo = userInDb;
                return RedirectToAction("Account", new {id = userInDb.UserId});
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("account/{id}")]
        public IActionResult Account(int id)
        {
            var sessionId = HttpContext.Session.GetInt32("userid");
            if(sessionId == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var userInfo = dbContext.BankUsers.Include(u => u.Transactions).FirstOrDefault(u => u.UserId == id);
                ViewBag.userInfo = userInfo;
                foreach(var amount in userInfo.Transactions)
                {
                    System.Console.WriteLine(amount);
                }
                return View();
            }
        }
        [HttpPost("transactions")]
        public IActionResult Transactions(Transactions trans)
        {
            if(ModelState.IsValid)
            {
                //for deposits, amount > 0
                // System.Console.WriteLine(trans.UserId);
                var userInfo = dbContext.BankUsers.Include(u => u.Transactions).FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userid"));
                Transactions newTrans = new Transactions();
                if(trans.Amount < userInfo.Balance)
                {
                    TempData["Error"] = "Insufficient funds";
                }
                trans.UserId = userInfo.UserId;
                trans.CreatedAt = DateTime.Now;
                dbContext.Add(trans);
                dbContext.SaveChanges();
                userInfo.Transactions.Add(dbContext.BankTransactions.First());
                return RedirectToAction("Account", new {id = HttpContext.Session.GetInt32("userid")});
            }
            else
            {
                // ModelState.AddModelError("Amount", "Please insert amount");
                // ViewBag.userInfo = dbContext.BankUsers.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userid"));
                TempData["Error"] = "Please submit an amount";
                return RedirectToAction("Account", new {id = HttpContext.Session.GetInt32("userid")});
            }
        }
        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
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
