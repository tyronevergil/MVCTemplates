using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class PasswordViewModel : PasswordEntryModel
    {
        public PasswordViewModel(string username, bool hasPasswordChanged)
        {
            Username = username;
            HasPasswordChanged = hasPasswordChanged;
        }

        [Display(Name = "Username:")]
        public string Username { get; private set; }

        public bool HasPasswordChanged { get; private set; }

        [Display(Name = "Current Password:", Prompt = "Current Password")]
        public override string CurrentPassword { get; set; }

        [Display(Name = "New Password:", Prompt = "New Password")]
        public override string NewPassword { get; set; }

        [Display(Name = "Confirm Password:", Prompt = "Confirm Password")]
        public override string ConfirmPassword { get; set; }
    }
}
