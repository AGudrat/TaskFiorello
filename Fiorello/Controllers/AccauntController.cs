using Fiorello.Models;
using Fiorello.Utilities.File;
using Fiorello.ViewModel.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Fiorello.Services;
using NETCore.MailKit.Core;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNet.Identity;

namespace Fiorello.Controllers
{

    public class AccauntController : Controller
    {

        private IConfiguration _configuration;
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;
        private IEmailSender _emailSender;

        public AccauntController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager,
                              IEmailSender emailSender,
                              IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public IActionResult Register()
        {
            IsAuthendicated();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }
            ApplicationUser newUser = new ApplicationUser
            {
                Fullname = register.FullName,
                Email = register.Email,
                UserName = register.UserName
            };

            Microsoft.AspNetCore.Identity.IdentityResult identityReult =await _userManager.CreateAsync(newUser,register.Password);
            if (!identityReult.Succeeded)
            {
                foreach (var error in identityReult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var callbackUrl = Url.Action(nameof(Submit), "Accaunt", new
                {
                    userId = newUser.Id,
                    code
                },protocol: Request.Scheme);
            var msg = new MailMessage(_configuration["Email:SenderEmail"], register.Email);
            msg.Subject = "Confirm your accaunt";
            msg.Body = $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>";
            msg.IsBodyHtml = true;
            await _emailSender.SendEmailAsync(msg);
            ViewBag.ErrorTitle = "Register Succesfuly";
            ViewBag.ErrorMessage = "Please confirm your email.";
            return View("Sumbit");
        }

        public async Task<IActionResult> Submit(string userId,string code)
        {
            var newUser = await _userManager.FindByIdAsync(userId);
            if (newUser == null) return BadRequest();
            var result = await _userManager.ConfirmEmailAsync(newUser, code);
            if (result.Succeeded)
            {
                ViewBag.ErrorTitle = "Confirmation complated!";
                ViewBag.ErrorMessage = "You can login in now!";
                return View();
            }
            return BadRequest();
        }

        public IActionResult Login()
        {
            IsAuthendicated();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login,string ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            ApplicationUser user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "Email or password is wrong");
                return View(login);
            }
            if (!user.IsActivated)
            {
                ModelState.AddModelError(String.Empty, "Your accaunt is not active. Please check your email.");
                return View(login);
            }
            var signInResult =
                await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError(String.Empty, "You tried to login many timed. Please wait a few minute.");
                return View(login);
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Email or password is wrong");
                return View(login);
            }
            if (ReturnUrl != null)
            {
                return LocalRedirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult ChangePassword()
        {
            return View();
        }
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user==null)
                {
                    return RedirectToAction("Login");
                }

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await _signInManager.RefreshSignInAsync(user);
                ViewBag.ErrorTitle = "Your password reseted!";
                return View("Submit");
            }
            return View(model);

        }
        


        private void IsAuthendicated()
        {
            if (User.Identity.IsAuthenticated)
            {
                throw new Exception("You already authendicated");
            }
        }
        #region CreateRole
        //public async Task CreateRole()
        //{
        //    foreach (var role in Enum.GetValues(typeof(UserRoles)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(role.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole() { Name = role.ToString() });
        //        }
        //    }

        //}

        #endregion
    }
}
