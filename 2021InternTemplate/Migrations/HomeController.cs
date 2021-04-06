using _2021InternTemplate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace _2021InternTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<MoneroUser> _userManager;
        private readonly SignInManager<MoneroUser> _loginManager;

        public HomeController(ILogger<HomeController> logger, 
            UserManager<MoneroUser> userManager, 
            SignInManager<MoneroUser> loginManager, 
            ApplicationDBContext dbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _loginManager = loginManager;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated) {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.Balance = user.Wallet.Balance;
            }

            else
            {
                ViewBag.Balance = "No balance found";
            }
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
