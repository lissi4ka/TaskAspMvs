using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TestTaskUniversity.Models.Users;

namespace TestTaskUniversity.Models.Course
{
    public class Course
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [MaxLength(300)]
        [Display(Name = "Название курса")]
        public string Name { get; set; }

        [MaxLength(300)]
        [Display(Name = "Предмет")]
        public string Subject { get; set; }

        [Display(Name = "Количество часов")]
        public int Hours { get; set; }

        public List<StudentCourse> StudentCourses { get; set; }
        public List<TeacherCourse> TeacherCourses { get; set; }

        [NotMapped]
        public bool HasInUserList { get; set; } = false;
    }

    public class StudentCourse
    {
        public string StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }

    public class TeacherCourse
    {
        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
