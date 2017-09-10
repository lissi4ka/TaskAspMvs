using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TestTaskUniversity.Models.Course;
using TestTaskUniversity.Models.Users;

namespace TestTaskUniversity.ViewModels
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Login")]
        public string Login { get; set; }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Age")]
        public int Age { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [MaxLength(300)]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [MaxLength(300)]
        [Display(Name = "Direction")]
        public string Direction { get; set; }

        [MaxLength(300)]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        public List<TeacherCourse> TeacherCourses { get; set; }

        public List<StudentCourse> StudentCourses { get; set; }
    }
}
