using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TestTaskUniversity.Models.Course;

namespace TestTaskUniversity.Models.Users
{
    public class Student
    {
        [Display(Name = "Id")]
        [Key]
        public string AppUserId { get; set; }

        [MaxLength(300)]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [MaxLength(300)]
        [Display(Name = "Direction")]
        public string Direction { get; set; }

        public List<StudentCourse> StudentCourses { get; set; }
    }
}
