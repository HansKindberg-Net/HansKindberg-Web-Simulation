using System.ComponentModel.DataAnnotations;

namespace HansKindberg.Web.Mvc.Simulation.Application.Models
{
    public class LogOnModel
    {
        #region Properties

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required]
        public virtual string Password { get; set; }

        [Display(Name = "Remember me?")]
        public virtual bool RememberMe { get; set; }

        [Display(Name = "User name")]
        [Required]
        public virtual string UserName { get; set; }

        #endregion
    }
}