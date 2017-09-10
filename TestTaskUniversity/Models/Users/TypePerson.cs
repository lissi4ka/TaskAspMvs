using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskUniversity.Models.Users
{
    public enum TypePerson : byte
    {
        [Display(Name = "Student")]
        Student = 0,
        [Display(Name = "Teacher")]
        Teacher = 1,
        [Display(Name = "Admin")]
        Admin= 2
    }
}
