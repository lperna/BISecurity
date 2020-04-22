using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BISecurityWithAuth.Models
{
    public class UserViewModel
    {
        public UserViewModel(User user, IdentityUser loginUser, Customer customer) {
            LoginUserId = loginUser.Id;
            Id = user.Id.ToString();
            UserName = loginUser.UserName;
            Email = loginUser.Email;
            IsAdmin = user.IsAdmin;
            Customer = customer?.Name;
        }
        [HiddenInput]
        public string LoginUserId { get; private set; }
        [HiddenInput]
        public string Id { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Customer")]
        public string Customer { get; set; }
        [Display(Name = "Administrator")]
        public bool IsAdmin { get; set; }
    }
}
