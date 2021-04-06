using _2021InternTemplate.Models;
using _2021InternTemplate.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace _2021InternTemplate.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<MoneroUser> _userManager;
        private readonly SignInManager<MoneroUser> _loginManager;
        private readonly IMoneroEmailService _emailService;

        public AccountController(ILogger<AccountController> logger,
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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            Wallet w = new Wallet() { Balance = 100 };
            WalletTransactions t = new WalletTransactions() { Name = "Initial", Amount = 100 };
            w.Transactions = new List<WalletTransactions>();
            w.Transactions.Add(t);
            MoneroUser user = new MoneroUser()
            {
                Email = viewModel.Email,
                UserName = viewModel.Username,
                Wallet = w
            };
            IdentityResult result = await _userManager.CreateAsync(user, viewModel.Password);
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(viewModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            MoneroUser user = await _userManager.FindByNameAsync(viewModel.Username);
            if (user != null)
            {
                var result = await _loginManager.CheckPasswordSignInAsync(user, viewModel.Password, false);

                if (result.Succeeded)
                {
                    string code = new Random().Next(0, 999999).ToString("000000");
                    _emailService.SendLoginCode(user.Email, code);
                    user.Code = code;
                    await _userManager.UpdateAsync(user);
                    HttpContext.Session.SetString("UserName", user.UserName);

                    
                    HttpContext.Session.SetString("UserId", user.Id);
                    await _loginManager.SignInAsync(user, false);
                    return RedirectToAction("ConfirmCode", "Account");
                }
            }
            return View(viewModel);
        }


        public IActionResult ConfirmCode()
        {
            return View();
            
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null )
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);

                 
                    _logger.Log(LogLevel.Warning, passwordResetLink);
                    _emailService.SendResetLink(user.Email, passwordResetLink);

                    return View("ForgotPasswordConfirmation");
                }

                
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {

            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {

                        return View("ResetPasswordConfirmation");
                    }
                 
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCode(ConfirmCode viewModel)
        {
            string UserName = HttpContext.Session.GetString("UserName");
            MoneroUser user = await _userManager.FindByNameAsync(UserName);

            if (user.Code == viewModel.Confirm) 
            {

               await _loginManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
                
            } 

            else
            {

                return RedirectToAction("Login", "Account");

            }

        }

    }
}
