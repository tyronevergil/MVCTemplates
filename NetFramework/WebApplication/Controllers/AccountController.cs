using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication.Infrastructure;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                if (IsRedirected())
                {
                    FormsAuthentication.SignOut();
                    HttpContext.User = null;

                    ModelState.AddModelError("", "You don't have access to the resource, Please try other account.");
                }
                else
                {
                    return Redirect(ResolveReturnUrl(returnUrl));
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginEntryModel entryModel, string returnUrl)
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
                    FormsAuthentication.SignOut();

                    HttpCookie authCookie = FormsAuthentication.GetAuthCookie(entryModel.Username, entryModel.RememberMe);
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

                    var authTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.IsPersistent ? ticket.Expiration : ticket.IssueDate.AddMinutes(Session.Timeout), ticket.IsPersistent, string.Join(",", user.Roles.ToArray()));
                    authCookie.Value = FormsAuthentication.Encrypt(authTicket);

                    Response.Cookies.Add(authCookie);
                    Session["AuthSync"] = authTicket.Expiration;

                    return Redirect(ResolveReturnUrl(returnUrl));
                }
            }

            return Login(returnUrl);
        }

        public ActionResult Logout(string returnUrl)
        {
            FormsAuthentication.SignOut();

            return Redirect(ResolveReturnUrl(returnUrl));
        }

        [Authorize]
        public ActionResult Password()
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
        public ActionResult Password(PasswordEntryModel entryModel)
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

                return RedirectToAction("Password");
            }

            return Password();

        }

        private bool IsRedirected()
        {
            var isRedirected = false;
            Boolean.TryParse(TempData.ContainsKey("Redirected") ? TempData["Redirected"].ToString() : "", out isRedirected);

            return isRedirected;
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
