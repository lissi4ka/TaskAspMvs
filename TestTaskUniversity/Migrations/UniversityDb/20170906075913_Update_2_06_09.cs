using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TestTaskUniversity.Migrations.UniversityDb
{
    public partial class Update_2_06_09 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherCourse_Students_StudentAppUserId",
                table: "TeacherCourse");

            migrationBuilder.DropIndex(
                name: "IX_TeacherCourse_StudentAppUserId",
                table: "TeacherCourse");

            migrationBuilder.DropColumn(
                name: "StudentAppUserId",
                table: "TeacherCourse");

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "StudentCourse",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentCourse_TeacherId",
                table: "StudentCourse",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentCourse_Teachers_TeacherId",
                table: "StudentCourse",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentCourse_Teachers_TeacherId",
                table: "StudentCourse");

            migrationBuilder.DropIndex(
                name: "IX_StudentCourse_TeacherId",
                table: "StudentCourse");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "StudentCourse");

            migrationBuilder.AddColumn<string>(
                name: "StudentAppUserId",
                table: "TeacherCourse",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherCourse_StudentAppUserId",
                table: "TeacherCourse",
                column: "StudentAppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherCourse_Students_StudentAppUserId",
                table: "TeacherCourse",
                column: "StudentAppUserId",
                principalTable: "Students",
                principalColumn: "AppUserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
