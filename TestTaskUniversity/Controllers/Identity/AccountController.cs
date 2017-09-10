using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTaskUniversity.Models.Users;
using TestTaskUniversity.ViewModels;
using TestTaskUniversity.Models.DbContext;

namespace TestTaskUniversity.Controllers.Identity
{
    public class AccountController : Controller
    {
        private string _student = "Student";
        private string _admin = "Admin";
        private string _teacher = "Teacher";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UniversityDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleManager, 
            UniversityDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result =
                    await _signInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {

                    // Verify that the URL belongs to the application.
                    if (!String.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("UserEditAccount", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            // delete authentication cookies
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Login,
                    Age = model.Age,
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                };
                // add user
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if(_roleManager.Roles.ToList().Count == 0)
                        await _createRolesAndAdminUser();

                    await _addRoleAndCreateAsPerson(model.TypePerson, user);

                    var thisUser = await _userManager.GetUserAsync(User);
                    if (await _userManager.IsInRoleAsync(thisUser, _admin))
                    {
                        return RedirectToAction("AllUsers", "Account");
                    }
                    // setting cookies
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var studentList = await _userManager.GetUsersInRoleAsync(_student);
            var adminList = await _userManager.GetUsersInRoleAsync(_admin);
            var teacherList = await _userManager.GetUsersInRoleAsync(_teacher);

            var user = await _userManager.GetUserAsync(User);
            adminList.Remove(user);

            var studentViewModel = new AllUsersViewModel();
            studentViewModel.Users = studentList;

            foreach (var student in studentList)
            {
                studentViewModel.Students.Add(await _context.Students
                    .Include(t=>t.StudentCourses)
                    .ThenInclude(t => t.Course)
                    .FirstOrDefaultAsync(t=>t.AppUserId == student.Id));
            }

            var adminViewModel = new AllUsersViewModel();
            adminViewModel.Users = adminList;

            var teacherViewModel = new AllUsersViewModel();
            teacherViewModel.Users = teacherList;

            foreach (var teacher in teacherList)
            {
                teacherViewModel.Teachers.Add(await _context.Teachers
                    .Include(t => t.TeacherCourses)
                    .ThenInclude(t=>t.Course)
                    .FirstOrDefaultAsync(t => t.AppUserId == teacher.Id));
            }

            ViewBag.StudentViewModel = studentViewModel;
            ViewBag.AdminViewModel = adminViewModel;
            ViewBag.TeacherViewModel = teacherViewModel;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = new
            {
                age = user.Age,
                name = user.FirstName + " " + user.LastName,
                email = user.Email,
                login = user.UserName,
            };

            return Json(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UserEditAccount(string id)
        {
            var allAdmins = await _userManager.GetUsersInRoleAsync(_admin);
            ViewBag.CountAdmin = allAdmins.Count;
            ViewBag.Student = _student;
            ViewBag.Admin = _admin;
            ViewBag.Student = _student;
            ViewBag.Teacher = _teacher;

            ApplicationUser user;
            if (String.IsNullOrEmpty(id))
                user = await _userManager.GetUserAsync(User);
            else
                user = await _userManager.FindByIdAsync(id);

            UserEditViewModel accountViewModel = new UserEditViewModel();
            if (await _userManager.IsInRoleAsync(user, _student))
            {
                var student = await _context.Students.Include(t => t.StudentCourses).ThenInclude(y => y.Course)
                    .FirstOrDefaultAsync(t => t.AppUserId == user.Id);

                accountViewModel.StudentCourses = student.StudentCourses;
                accountViewModel.Department = student.Department;
                accountViewModel.Direction = student.Direction;
            }
            else if (await _userManager.IsInRoleAsync(user, _teacher))
            {
                var teacher = await _context.Teachers.Include(t => t.TeacherCourses).ThenInclude(y => y.Course)
                    .FirstOrDefaultAsync(t => t.AppUserId == user.Id);

                accountViewModel.TeacherCourses = teacher.TeacherCourses;
                accountViewModel.Subject = teacher.Subject;
            }

            accountViewModel.Id = user.Id;
            accountViewModel.Age = user.Age;
            accountViewModel.FirstName = user.FirstName;
            accountViewModel.LastName = user.LastName;
            accountViewModel.Login = user.UserName;
            accountViewModel.Email = user.Email;
            
            return View(accountViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEditAccount(UserEditViewModel model)
        {
            var allAdmins = await _userManager.GetUsersInRoleAsync(_admin);
            ViewBag.CountAdmin = allAdmins.Count;
            ViewBag.Student = _student;
            ViewBag.Admin = _admin;
            ViewBag.Teacher = _teacher;
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                user.Age = model.Age;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.Login;
                user.Email = model.Email;
                if (!String.IsNullOrEmpty(model.OldPassword) && !String.IsNullOrEmpty(model.NewPassword))
                {
                    var resUpdPaswr = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (!resUpdPaswr.Succeeded)
                    {
                        foreach (var error in resUpdPaswr.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

                await _userManager.UpdateAsync(user);

                if (await _userManager.IsInRoleAsync(user, _student))
                {
                    var student = await _context.Students.Include(t => t.StudentCourses).ThenInclude(y => y.Course)
                        .FirstOrDefaultAsync(t => t.AppUserId == user.Id);

                    student.Department = model.Department;
                    student.Direction = model.Direction;
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    model.StudentCourses = student.StudentCourses;
                }
                else if (await _userManager.IsInRoleAsync(user, _teacher))
                {
                    var teacher = await _context.Teachers.Include(t => t.TeacherCourses).ThenInclude(y => y.Course)
                        .FirstOrDefaultAsync(t => t.AppUserId == user.Id);

                    teacher.Subject = model.Subject;
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                    model.TeacherCourses = teacher.TeacherCourses;
                }
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RemoveUserAccount(string id)
        {
            ApplicationUser user;
            if (String.IsNullOrEmpty(id))
                user = await _userManager.GetUserAsync(User);
            else
                user = await _userManager.FindByIdAsync(id);

            if (await _userManager.IsInRoleAsync(user, _student))
            {
                var student = await _context.Students.Include(t => t.StudentCourses).ThenInclude(y => y.Course)
                    .FirstOrDefaultAsync(t => t.AppUserId == user.Id);
                if (student != null)
                {
                    _context.Remove(student);
                    await _context.SaveChangesAsync();
                }

            }

            if (await _userManager.IsInRoleAsync(user, _teacher))
            {
                var teacher = await _context.Teachers.Include(t => t.TeacherCourses).ThenInclude(y => y.Course)
                    .FirstOrDefaultAsync(t => t.AppUserId == user.Id);

                if (teacher != null)
                {
                    _context.Remove(teacher);
                    await _context.SaveChangesAsync();
                }
            }

            await _userManager.DeleteAsync(user);
            await _signInManager.SignOutAsync();
            
            return Json("sucsess");
        }
        // GET /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task _createRolesAndAdminUser()
        {
            var roleTeacher = new IdentityRole("Teacher");
            var roleStudent = new IdentityRole("Student");
            var roleAdmin = new IdentityRole("Admin");

            await _roleManager.CreateAsync(roleTeacher);
            await _roleManager.CreateAsync(roleStudent);
            var resultRoleAdmin = await _roleManager.CreateAsync(roleAdmin);
            if (resultRoleAdmin.Succeeded)
            {
                ApplicationUser admin = new ApplicationUser
                {
                    Email = "admin@gmail.com",
                    UserName = "Admin",
                    Age = 35,
                    LastName = "Admin",
                    FirstName = "Admin",
                };
                var resultAddAdmin = await _userManager.CreateAsync(admin, "Qwerty123!");
                if (resultAddAdmin.Succeeded)
                {
                    List<string> adminRole = new List<string>();
                    adminRole.Add("Admin");
                    await _userManager.AddToRolesAsync(admin, adminRole);
                }
            }
        }

        private async Task _addRoleAndCreateAsPerson(TypePerson modelTypePerson, ApplicationUser user)
        {
            switch (modelTypePerson)
            {
                case TypePerson.Student:
                    List<string> studentRole = new List<string>();
                    studentRole.Add("Student");
                    await _userManager.AddToRolesAsync(user, studentRole);

                    var student = new Student();
                    student.AppUserId = user.Id;

                    _context.Add(student);
                    await _context.SaveChangesAsync();

                    break;
                case TypePerson.Teacher:
                    List<string> teacherRole = new List<string>();
                    teacherRole.Add("Teacher");
                    await _userManager.AddToRolesAsync(user, teacherRole);

                    var teacher = new Teacher();
                    teacher.AppUserId = user.Id;

                    _context.Add(teacher);
                    await _context.SaveChangesAsync();

                    break;
                case TypePerson.Admin:
                    List<string> adminRole = new List<string>();
                    adminRole.Add("Admin");
                    await _userManager.AddToRolesAsync(user, adminRole);
                    
                    break;
            }
        }
    }
}