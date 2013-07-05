using System.ComponentModel.DataAnnotations;
using HansKindberg.Web.Mvc.Simulation.Application.Business.Web.Security;

namespace HansKindberg.Web.Mvc.Simulation.Application.Models
{
    public class ChangePasswordModel
    {
        #region Properties

        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public virtual string ConfirmPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [Required]
        [ValidatePasswordLength]
        public virtual string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        [Required]
        public virtual string OldPassword { get; set; }

        #endregion
    }
}