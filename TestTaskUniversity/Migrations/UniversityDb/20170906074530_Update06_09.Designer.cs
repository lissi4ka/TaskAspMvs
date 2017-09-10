﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using TestTaskUniversity.Models.DbContext;

namespace TestTaskUniversity.Migrations.UniversityDb
{
    [DbContext(typeof(UniversityDbContext))]
    [Migration("20170906074530_Update06_09")]
    partial class Update06_09
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TestTaskUniversity.Models.Course.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Hours");

                    b.Property<string>("Name")
                        .HasMaxLength(300);

                    b.Property<string>("Subject")
                        .HasMaxLength(300);

                    b.HasKey("Id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("TestTaskUniversity.Models.Course.StudentCourse", b =>
                {
                    b.Property<string>("StudentId");

                    b.Property<int>("CourseId");

                    b.HasKey("StudentId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("StudentCourse");
                });

            modelBuilder.Entity("TestTaskUniversity.Models.Course.TeacherCourse", b =>
                {
                    b.Property<string>("TeacherId");

                    b.Property<int>("CourseId");

                    b.Property<string>("StudentAppUserId");

                    b.HasKey("TeacherId", "CourseId");

                    b.HasIndex("CourseId");

                    b.HasIndex("StudentAppUserId");

                    b.ToTable("TeacherCourse");
                });

            modelBuilder.Entity("TestTaskUniversity.Models.Users.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("Age")
                        .HasMaxLength(2);

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Email");

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .HasMaxLength(300);

                    b.Property<string>("LastName")
                        .HasMaxLength(300);

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail");

                    b.Property<string>("NormalizedUserName");

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUser");
                });

            modelBuilder.Entity("TestTaskUniversity.Models.Users.Student", b =>
                {
                    b.Property<string>("AppUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Department")
                        .HasMaxLength(300);

                    b.Property<string>("Direction")
                        .HasMaxLength(300);

                    b.HasKey("AppUserId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("TestTaskUniversity.Models.Users.Teacher", b =>
                {
                    b.Property<string>("AppUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Subject")
                        .HasMaxLength(300);

                    b.HasKey("AppUserId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("TestTaskUniversity.Models.Course.StudentCourse", b =>
                {
                    b.HasOne("TestTaskUniversity.Models.Course.Course", "Course")
                        .WithMany("StudentCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TestTaskUniversity.Models.Users.Student", "Student")
                        .WithMany("StudentCourses")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TestTaskUniversity.Models.Course.TeacherCourse", b =>
                {
                    b.HasOne("TestTaskUniversity.Models.Course.Course", "Course")
                        .WithMany("TeacherCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TestTaskUniversity.Models.Users.Student")
                        .WithMany("TeacherCourse")
                        .HasForeignKey("StudentAppUserId");

                    b.HasOne("TestTaskUniversity.Models.Users.Teacher", "Teacher")
                        .WithMany("TeacherCourses")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
