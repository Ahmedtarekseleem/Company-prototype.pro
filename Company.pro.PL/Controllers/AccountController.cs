using Company.pro.DAL.Models;
using Company.pro.PL.Dtos;
using Company.pro.PL.Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.pro.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IMailService _mailService;
		private readonly ITwilioService _twiliService;

        public AccountController(UserManager<AppUser> userManager,
               SignInManager<AppUser> signInManager,
               IMailService mailService,
               ITwilioService twiliService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _twiliService = twiliService;
        }
        #region SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

		// P@ssW0rd
		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpDto model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(model.UserName);
				if (user is null)
				{
					user = await _userManager.FindByNameAsync(model.Email);
					if (user is null)
					{
						user = new AppUser()
						{
							UserName = model.UserName,
							FirstName = model.FirstName,
							LastName = model.LastName,
							Email = model.Email,
							IsAgree = model.IsAgree,
						};
						var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					return RedirectToAction("SignIn");
				}
				foreach (var item in result.Errors )
				{
					ModelState.AddModelError("", item.Description);
				}
					}
				}
				ModelState.AddModelError("", "Invalid SignUp !!");
			}

			 return View(); 
		}
		#endregion

		#region SignIn
		[HttpGet]
		public IActionResult SignIn()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignIn(SignInDto model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var flag = await _userManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						// Sign In
						var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,false);
						if (result.Succeeded)
						{
							return RedirectToAction(nameof(HomeController.Index), "Home");
						}
					}
				}
				ModelState.AddModelError("", "Invalid Login !");
			}
			return View();
		}
        //[HttpPost] // Change from implicit GET to explicit POST
        //[AllowAnonymous] // Add this to ensure unauthenticated users can access it
        public IActionResult GoogleLogin()
        {
            var prop = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(prop, GoogleDefaults.AuthenticationScheme);
        }
        
        public async Task<IActionResult> GoogleResponse()
		{
			var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
			var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(
				claim => new
				{
					claim.Type,
					claim.Value,
					claim.Issuer,
					claim.OriginalIssuer
				}

				);
			return RedirectToAction("Index", "Home");
		}
		#endregion

		#region SignOut
		public new async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}
		#endregion

		#region Forget Password
		[HttpGet]
		public  IActionResult ForgetPassword()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordDto model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					var url =Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

				var email = new Email()
				{
					To = model.Email,
					Subject = "Reset Password",
					Body = url
				};
					var Flag = EmailSettings.SendEmail(email);
					if (Flag)
					{
						return RedirectToAction("CheckYourInbox");
					}
					_mailService.SendEmail(email);
					return RedirectToAction("CheckYourInbox");
				}
			}
			ModelState.AddModelError("", "Invalid Reset Password Operation !!");
			return View("ForgetPassword", model);
		}
		public async Task<IActionResult> SendResetPasswordSms(ForgetPasswordDto model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					var url =Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

				var sms = new Sms()
				{
					To = user.PhoneNumber,
					Body = url
				};
					_twiliService.SendSms(sms);
					return RedirectToAction("CheckYourPhone");
				}
			}
			ModelState.AddModelError("", "Invalid Reset Password Operation !!");
			return View("ForgetPassword", model);
		}
		[HttpGet]
		public IActionResult CheckYourInbox()
		{
			return View();
		}
		[HttpGet]
		public IActionResult CheckYourPhone()
		{
			return View();
		}
		#endregion

		#region MyRegion
		[HttpGet]
		public IActionResult ResetPassword(string email,string token)
		{
			TempData["Email"]=email;
			TempData["Token"]=token;

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
		{
			if (ModelState.IsValid)
			{
				var email = TempData["Email"] as string;
				var token = TempData["token"] as string;
				
				if (email is null || token is null)
				{
					return BadRequest("Invalid Operation");
				}
				var user = await _userManager.FindByEmailAsync(email);
				if (user != null)
				{
					var result = await _userManager.ResetPasswordAsync(user, token,model.NewPassword);
					if (result.Succeeded)
					{
						return RedirectToAction("SignIn");
					}
				}
				ModelState.AddModelError("", "Invalid Reset Password Operation");
			}
			return View();
		}

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion
    }
}
