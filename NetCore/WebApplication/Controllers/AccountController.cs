using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApplication.Infrastructure;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationUserManager _userManager;

        public AccountController(IConfiguration configuration, ApplicationUserManager userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (IsRedirected())
                {
                    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.User = null;

                    ModelState.AddModelError("", "You don't have access to the resource, Please try other account.");
                }
                else
                {
                    return Redirect(ResolveReturnUrl(returnUrl));
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginEntryModel entryModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Find(entryModel.Username, entryModel.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
                else
                {
                    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName)
                    };
                    claims.AddRange(user.Roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    var timeout = GetAuthTimeout(entryModel.RememberMe);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = entryModel.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(timeout)
                    };

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                    return Redirect(ResolveReturnUrl(returnUrl));
                }
            }

            return Login(returnUrl);
        }

        public IActionResult Logout(string returnUrl)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect(ResolveReturnUrl(returnUrl));
        }

        [Authorize]
        public IActionResult Password()
        {
            var hasPasswordChanged = false;
            if (IsRedirected())
            {
                Boolean.TryParse(TempData.ContainsKey("PasswordChanged") ? TempData["PasswordChanged"].ToString() : "", out hasPasswordChanged);
            }

            return View(new PasswordViewModel(User.Identity.Name, hasPasswordChanged));
        }

        [HttpPost]
        [Authorize]
        public IActionResult Password(PasswordEntryModel entryModel)
        {
            var currentUser = User.Identity.Name;

            if (!string.IsNullOrWhiteSpace(entryModel.CurrentPassword))
            {
                var user = _userManager.Find(currentUser, entryModel.CurrentPassword);
                if (user == null)
                {
                    ModelState.AddModelError("CurrentPassword", "Invalid current password.");
                }
            }

            if (ModelState.IsValid)
            {
                _userManager.UpdatePassword(currentUser, entryModel.NewPassword);
                TempData["PasswordChanged"] = true;

                return RedirectToAction();
            }

            return Password();

        }

        private bool IsRedirected()
        {
            var isRedirected = false;
            Boolean.TryParse(TempData.ContainsKey("Redirected") ? TempData["Redirected"].ToString() : "", out isRedirected);

            return isRedirected;
        }

        private int GetAuthTimeout(bool isPersistent)
        {
            var sessionTimeout = 60;
            var persistentTimeout = 43200;

            int.TryParse(_configuration["Timeout:Session"], out sessionTimeout);
            int.TryParse(_configuration["Timeout:Authentication:Persistent"], out persistentTimeout);

            return isPersistent ? persistentTimeout : sessionTimeout;
        }

        private string GetHomeUrl()
        {
            return Url.Action("Index", "Home");
        }

        private string ResolveReturnUrl(string returnUrl)
        {
            return string.IsNullOrWhiteSpace(returnUrl) ? GetHomeUrl() : returnUrl;
        }
    }
}
