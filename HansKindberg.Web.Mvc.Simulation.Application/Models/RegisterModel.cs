using System.ComponentModel.DataAnnotations;
using HansKindberg.Web.Mvc.Simulation.Application.Business.Web.Security;

namespace HansKindberg.Web.Mvc.Simulation.Application.Models
{
    public class RegisterModel
    {
        #region Properties

        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public virtual string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        [Required]
        public virtual string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        [ValidatePasswordLength]
        public virtual string Password { get; set; }

        [Display(Name = "User name")]
        [Required]
        public virtual string UserName { get; set; }

        #endregion
    }
}