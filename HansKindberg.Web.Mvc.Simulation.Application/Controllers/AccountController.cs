using System;
using System.Web.Mvc;
using System.Web.Security;
using HansKindberg.Web.Mvc.Simulation.Application.Models;
using HansKindberg.Web.Simulation.Applications.Logic.Web.Security;

namespace HansKindberg.Web.Mvc.Simulation.Application.Controllers
{
    public class AccountController : Controller
    {
        #region Fields

        private readonly IAccountValidation _accountValidation;
        private const string _changePasswordConfirmationSessionName = "ChangePasswordConfirmation";
        private readonly IFormsAuthentication _formsAuthentication;
        private readonly IMembershipProvider _membershipProvider;
        private const StringComparison _passwordComparison = StringComparison.Ordinal;
        private const string _registerConfirmationSessionName = "RegisterConfirmation";

        #endregion

        #region Constructors

        public AccountController(IMembershipProvider membershipProvider, IFormsAuthentication formsAuthentication, IAccountValidation accountValidation)
        {
            if(membershipProvider == null)
                throw new ArgumentNullException("membershipProvider");

            if(formsAuthentication == null)
                throw new ArgumentNullException("formsAuthentication");

            if(accountValidation == null)
                throw new ArgumentNullException("accountValidation");

            this._accountValidation = accountValidation;
            this._formsAuthentication = formsAuthentication;
            this._membershipProvider = membershipProvider;
        }

        #endregion

        #region Properties

        protected internal virtual StringComparison PasswordComparison
        {
            get { return _passwordComparison; }
        }

        #endregion

        #region Methods

        public virtual ActionResult ChangePassword()
        {
            bool? isChangePasswordConfirmation = this.Session[_changePasswordConfirmationSessionName] as bool?;

            if(!isChangePasswordConfirmation.HasValue || !isChangePasswordConfirmation.Value)
            {
                this.ViewBag.PasswordLength = this._membershipProvider.MinimumRequiredPasswordLength;
            }
            else
            {
                this.ViewBag.Confirm = true;
                this.Session[_changePasswordConfirmationSessionName] = null;
            }

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ChangePassword(ChangePasswordModel model)
        {
            if(model == null)
                throw new ArgumentNullException("model");

            this.ViewBag.PasswordLength = this._membershipProvider.MinimumRequiredPasswordLength;

            if(this.ModelState.IsValid)
            {
                try
                {
                    if(!model.NewPassword.Equals(model.ConfirmPassword, this.PasswordComparison))
                        throw new InvalidOperationException("The new password and the confirm password are not equal.");

                    this._membershipProvider.ChangePassword(this.User.Identity.Name, model.OldPassword, model.NewPassword);

                    this.Session[_changePasswordConfirmationSessionName] = true;

                    return this.RedirectToAction("ChangePassword", "Account");
                }
                catch(InvalidOperationException invalidOperationException)
                {
                    this.ModelState.AddModelError(string.Empty, invalidOperationException.Message);
                }
            }

            return this.View(model);
        }

        public virtual ActionResult LogOff()
        {
            this._formsAuthentication.SignOut();

            if(this.Request.UrlReferrer != null)
                return this.Redirect(this.Request.UrlReferrer.ToString());

            return this.RedirectToAction("Index", "Home");
        }

        public virtual ActionResult LogOn()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult LogOn(LogOnModel model)
        {
            if(model == null)
                throw new ArgumentNullException("model");

            if(this.ModelState.IsValid)
            {
                if(this._membershipProvider.ValidateUser(model.UserName, model.Password))
                    this._formsAuthentication.RedirectFromLogOnPage(model.UserName, model.RememberMe);

                this.ModelState.AddModelError(string.Empty, "Invalid username or password.");
            }

            return this.View(model);
        }

        public virtual ActionResult Register()
        {
            bool? isRegisterConfirmation = this.Session[_registerConfirmationSessionName] as bool?;

            if(!isRegisterConfirmation.HasValue || !isRegisterConfirmation.Value)
            {
                this.ViewBag.PasswordLength = this._membershipProvider.MinimumRequiredPasswordLength;
            }
            else
            {
                this.ViewBag.Confirm = true;
                this.Session[_registerConfirmationSessionName] = null;
            }

            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Register(RegisterModel model)
        {
            if(model == null)
                throw new ArgumentNullException("model");

            this.ViewBag.PasswordLength = this._membershipProvider.MinimumRequiredPasswordLength;

            if(this.ModelState.IsValid)
            {
                try
                {
                    if(!model.Password.Equals(model.ConfirmPassword, this.PasswordComparison))
                        throw new InvalidOperationException("The password and the confirm password are not equal.");

                    MembershipCreateStatus membershipCreateStatus;
                    MembershipUser membershipUser = this._membershipProvider.CreateUser(model.UserName, model.Password, model.Email, out membershipCreateStatus);

                    if(membershipCreateStatus != MembershipCreateStatus.Success)
                        throw new InvalidOperationException(this._accountValidation.GetMessage(membershipCreateStatus));

                    this._formsAuthentication.SetAuthenticationCookie(membershipUser.UserName, false);

                    this.Session[_registerConfirmationSessionName] = true;

                    return this.RedirectToAction("Register", "Account");
                }
                catch(InvalidOperationException invalidOperationException)
                {
                    this.ModelState.AddModelError(string.Empty, invalidOperationException.Message);
                }
            }

            return this.View(model);
        }

        #endregion
    }
}