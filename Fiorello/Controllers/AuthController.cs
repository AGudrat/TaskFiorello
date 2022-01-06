using Fiorello.Models;
using Fiorello.ViewModel.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Threading.Tasks;

namespace Fiorello.Controllers
{
    public class AuthController : Controller
    {
        private UserManager<ApplicationUser> _userMagager;
        private SignInManager<ApplicationUser> _signInManager;
        private IEmailService _emailService;

        public AuthController(UserManager<ApplicationUser> userManager,
                              SignInManager<ApplicationUser> signInManager,
                              IEmailService emailService)
        {
            _userMagager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Register()
        {
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

            IdentityResult identityReult =await _userMagager.CreateAsync(newUser,register.Password);
            if (!identityReult.Succeeded)
            {
                foreach (var error in identityReult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(register);
            }
            else
            {
                //generation of the email token
                var code = await _userMagager.GenerateEmailConfirmationTokenAsync(newUser);
                var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = newUser.Id, code },Request.Scheme,Request.Host.ToString());

                await _emailService.SendAsync(newUser.Email,"email verify", $"<a href=\"{link}\">Verify Email<\a>",true);

                ViewBag.ErrorTitle("Registration succesful");
                ViewBag.ErrorMessage("Before you can Login, please confirm your email, by clicking on the confirmation link we have emailed you");
                return View("Error");
            }
        }

        public async Task<IActionResult> VerifyEmail(string userId,string code)
        {
            var user = await _userMagager.FindByIdAsync(userId);
            if (user == null) return BadRequest();
            var result = await _userMagager.ConfirmEmailAsync(user,code);
            if (result.Succeeded) return View();
            ViewBag.ErrorTitle("Your accaunt verified!");
            ViewBag.ErrorMessage("Thank you for verifying your email.");
            return View("Error");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
