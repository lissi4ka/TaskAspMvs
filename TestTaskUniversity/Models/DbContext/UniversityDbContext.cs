using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestTaskUniversity.Models.Course;
using TestTaskUniversity.Models.Users;

namespace TestTaskUniversity.Models.DbContext
{
    public class UniversityDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public UniversityDbContext(DbContextOptions<UniversityDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(t => new { t.StudentId, t.CourseId });

            modelBuilder.Entity<StudentCourse>()
                .HasOne(pt => pt.Student)
                .WithMany(p => p.StudentCourses)
                .HasForeignKey(pt => pt.StudentId);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(pt => pt.Course)
                .WithMany(t => t.StudentCourses)
                .HasForeignKey(pt => pt.CourseId);

          
            modelBuilder.Entity<TeacherCourse>()
                .HasKey(t => new { t.TeacherId, t.CourseId });

            modelBuilder.Entity<TeacherCourse>()
                .HasOne(pt => pt.Teacher)
                .WithMany(p => p.TeacherCourses)
                .HasForeignKey(pt => pt.TeacherId);

            modelBuilder.Entity<TeacherCourse>()
                .HasOne(pt => pt.Course)
                .WithMany(t => t.TeacherCourses)
                .HasForeignKey(pt => pt.CourseId);
        }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course.Course> Courses { get; set; }
        public DbSet<TestTaskUniversity.Models.Course.StudentCourse> StudentCourse { get; set; }
        public DbSet<TestTaskUniversity.Models.Course.TeacherCourse> TeacherCourses { get; set; }
        public DbSet<TestTaskUniversity.Models.Users.ApplicationUser> ApplicationUser { get; set; }
    }
}
