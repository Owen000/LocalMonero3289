using _2021InternTemplate.Models;
using _2021InternTemplate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2021InternTemplate.Controllers
{
    [Authorize(Roles = Constants.AdminRole)]
    public class AdminController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<MoneroUser> _userManager;
        private readonly SignInManager<MoneroUser> _loginManager;
        private readonly IMoneroEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(ILogger<AccountController> logger,
            UserManager<MoneroUser> userManager,
            SignInManager<MoneroUser> loginManager,
            ApplicationDBContext dbContext,
            IMoneroEmailService emailService,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _userManager = userManager;
            _loginManager = loginManager;
            _dbContext = dbContext;
            _emailService = emailService;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> DebugCreateRole(string RoleName)
        {
            //DO NOT USE THIS CODE, DEMONSTRATION ONLY
            IdentityRole role = new IdentityRole()
            {
                Name = RoleName
            };
            var result = await _roleManager.CreateAsync(role);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public async Task<IActionResult> AddAdminToFirstUser()
        {
            //DO NOT USE THIS CODE, DEMONSTRATION ONLY            
            MoneroUser user = await _userManager.FindByNameAsync("Admin");
            var result = await _userManager.AddToRoleAsync(user, Constants.AdminRole);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles =Constants.AdminRole)]
        public async Task<IActionResult> CheckAdmin()
        {

            return RedirectToAction("Index", "Home");
        }

    }
}