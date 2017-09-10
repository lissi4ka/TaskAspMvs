using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TestTaskUniversity.Models.Users
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(300)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [MaxLength(300)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [MaxLength(2)]
        [Display(Name = "Age")]
        public int Age { get; set; }
    }
}
