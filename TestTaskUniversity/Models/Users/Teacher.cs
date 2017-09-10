using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TestTaskUniversity.Models.Course;

namespace TestTaskUniversity.Models.Users
{
    public class Teacher 
    {
        [Display(Name = "Id")]
        [Key]
        public string AppUserId { get; set; }

        [MaxLength(300)]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        public List<TeacherCourse> TeacherCourses { get; set; }
    }
}
