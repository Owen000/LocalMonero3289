using _2021InternTemplate.Models;
using _2021InternTemplate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021InternTemplate.Controllers
{
    //[Authorize]
    public class WalletController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<MoneroUser> _userManager;
        private readonly SignInManager<MoneroUser> _loginManager;
        private readonly IMoneroEmailService _emailService;

        public WalletController(ILogger<AccountController> logger,
            UserManager<MoneroUser> userManager,
            SignInManager<MoneroUser> loginManager,
            ApplicationDBContext dbContext,
            IMoneroEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _loginManager = loginManager;
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {


            var user = _dbContext.Users.Include(u => u.Wallet).SingleOrDefault(u => u.UserName.ToLower() == HttpContext.Session.GetString("UserName").ToLower());
            return View(user);

            

        }
    }
}