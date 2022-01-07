using Fiorello.Models;
using Fiorello.Utilities.File;
using Fiorello.ViewModel.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System;
using System.Threading.Tasks;

namespace Fiorello.Controllers
{
    public class AccauntController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailService _emailService;
        private RoleManager<IdentityRole> _roleManager;
        public AccauntController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IEmailService emailService,
                              RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _roleManager = roleManager;
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

            IdentityResult identityReult =await _userManager.CreateAsync(newUser,register.Password);
            if (!identityReult.Succeeded)
            {
                foreach (var error in identityReult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }
            //else
            //{
            //    //generation of the email token
            //    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            //    var link = Url.Action(nameof(VerifyEmail), "Auth", new { userId = newUser.Id, code },Request.Scheme,Request.Host.ToString());

            //    await _emailService.SendAsync("abidzadeq2002@gmail.com", "Confirm", $"<a href=\"{link}\">Verify Email<\a>",true);

            //    ViewBag.ErrorTitle("Registration succesful");
            //    ViewBag.ErrorMessage("Before you can Login, please confirm your email, by clicking on the confirmation link we have emailed you");
            //    return View("Error");
            //}
            await _userManager.AddToRoleAsync(newUser,UserRoles.Admin.ToString());
            await _signInManager.SignInAsync(newUser, false);
            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> VerifyEmail(string userId,string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest();
            var result = await _userManager.ConfirmEmailAsync(user,code);
            if (result.Succeeded) return View();
            ViewBag.ErrorTitle("Your accaunt verified!");
            ViewBag.ErrorMessage("Thank you for verifying your email.");
            return View("Error");
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
                ModelState.AddModelError(String.Empty, "Please,activate your account. Check your email.");
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
