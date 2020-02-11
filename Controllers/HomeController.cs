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
                    return RedirectToAction("Index");
                }
                var hasher = new PasswordHasher<Login>();
                var result = hasher.VerifyHashedPassword(userLogin, userInDb.Pw, userLogin.LogInPw);
                if(result == 0)
                {
                    ModelState.AddModelError("LogInPw", "Email or password doesn't match");
                    return RedirectToAction("Index");
                }
                HttpContext.Session.SetInt32("userId", userInDb.UserId);
                ViewBag.userInfo = userInDb;
                return View("Account", new {id = userInDb.UserId});
            }
            else
            {
                return RedirectToAction("Index");
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
                ViewBag.userInfo = dbContext.BankUsers.Include(u => u.UserId == id);
                return View();
            }
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
