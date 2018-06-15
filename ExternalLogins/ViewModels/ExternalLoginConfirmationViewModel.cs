using System.ComponentModel.DataAnnotations;

namespace ExternalLogins.ViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }


    }
}
