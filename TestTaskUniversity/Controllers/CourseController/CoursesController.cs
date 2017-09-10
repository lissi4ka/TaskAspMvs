using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestTaskUniversity.Models.Course;
using TestTaskUniversity.Models.DbContext;
using TestTaskUniversity.Models.Users;
using TestTaskUniversity.ViewModels.TagHelper;

namespace TestTaskUniversity.Controllers.CourseController
{
    [Authorize]
    public class CoursesController : Controller
    {
        private string _student = "Student";
        private string _teacher = "Teacher";
        private string _admin = "Admin";
        private string _result = "sucsess";
        private readonly UniversityDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            UniversityDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        #region Index
        // GET: Courses
        public async Task<IActionResult> Index(string searchString, int pageSize = 10, int page = 1)
        {
            ViewBag.CurrentFilter = searchString;
            ViewBag.Student = _student;
            ViewBag.Teacher = _teacher;
            ViewBag.Admin = _admin;

            IQueryable<Course> source = from s in _context.Courses
                select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                source = source.Where(s => s.Name.ToLower().Contains(searchString)
                                           || s.Hours.ToString().Contains(searchString)
                                           || s.Subject.ToLower().Contains(searchString));
            }

            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            
            FilterViewModel filterViewModel = new FilterViewModel(searchString);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            ItemIndexViewModel<Course> viewModel = new ItemIndexViewModel<Course>()
            {
                PageViewModel = pageViewModel,
                Items = await _setHasInUserList(items),
                FilterViewModel = filterViewModel
            };

            return View(viewModel);
        }

        private async Task<List<Course>> _setHasInUserList(List<Course> list)
        {
            var user = await _userManager.GetUserAsync(User);

            var student = await _context.Students
                .Include(t => t.StudentCourses)
                .FirstOrDefaultAsync(t => t.AppUserId == user.Id);

            var teacher = await _context.Teachers
                .Include(t => t.TeacherCourses)
                .FirstOrDefaultAsync(t => t.AppUserId == user.Id);

            if (student != null)
            {
                foreach (var studCourse in student.StudentCourses)
                {
                    var cours = list.FirstOrDefault(t => t.Id == studCourse.CourseId);
                    if (cours != null)
                    {
                        cours.HasInUserList = true;
                    }
                }
            }

            if (teacher != null)
            {
                foreach (var teacherCourse in teacher.TeacherCourses)
                {
                    var cours = list.FirstOrDefault(t => t.Id == teacherCourse.CourseId);
                    if (cours != null)
                    {
                        cours.HasInUserList = true;
                    }
                }
            }
            return list;
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> AddInTeacherCourses(int? idCourse)
        {
            if (idCourse == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var theacter = await _context.Teachers
                .Include(t => t.TeacherCourses)
                .FirstOrDefaultAsync(t => t.AppUserId == user.Id);

            var course = await _context.Courses.FirstOrDefaultAsync(t => t.Id == idCourse);

            var newCourse = new TeacherCourse()
            {
                Course = course,
                CourseId = course.Id,
                Teacher = theacter,
                TeacherId = theacter.AppUserId
            };

            _context.Add(newCourse);
            await _context.SaveChangesAsync();
            return Json(_result);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> RemoveFromTeacherList(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var theacter = await _context.Teachers
                .Include(t => t.TeacherCourses)
                .FirstOrDefaultAsync(t => t.AppUserId == user.Id);

            var course = theacter.TeacherCourses.FirstOrDefault(t => t.CourseId == id);
            if (course != null)
                _context.TeacherCourses.Remove(course);

            await _context.SaveChangesAsync();
            return Json(_result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeachersForCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.Include(t => t.TeacherCourses).FirstOrDefaultAsync(t => t.Id == id);
            var theachers = course.TeacherCourses;
            var result = new List<object>();
            foreach (var theacher in theachers)
            {
                var theacherId = theacher.TeacherId;
                var user = await _userManager.FindByIdAsync(theacherId);
                result.Add(new
                {
                    name = user.FirstName + " " + user.LastName,
                    id = theacherId
                });
            }

            return Json(result.ToArray());
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromStudentList(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var student = await _context.Students
                .Include(t => t.StudentCourses)
                .FirstOrDefaultAsync(t => t.AppUserId == user.Id);

            var course = student.StudentCourses.FirstOrDefault(t => t.CourseId == id);
            if (course != null)
                _context.StudentCourse.Remove(course);

            await _context.SaveChangesAsync();
            return Json(_result);
        }

        [HttpPost]
        public async Task<IActionResult> AddInStudentCourse(string idTheacher, int? idCourse)
        {
            if (idCourse == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var student = await _context.Students
                .Include(t => t.StudentCourses)
                .FirstOrDefaultAsync(t => t.AppUserId == user.Id);

            var course = await _context.Courses.FirstOrDefaultAsync(t => t.Id == idCourse);
            var theacter = await _context.Teachers.FirstOrDefaultAsync(t => t.AppUserId == idTheacher);

            var newCourse = new StudentCourse()
            {
                Student = student,
                StudentId = student.AppUserId,
                Course = course,
                CourseId = course.Id,
                Teacher = theacter,
                TeacherId = idTheacher
            };

            _context.Add(newCourse);
            await _context.SaveChangesAsync();
            return Json(_result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FirstOrDefaultAsync(t => t.Id == id);
            var result = new
            {
                name = course.Name,
                id = course.Id,
                subject = course.Subject,
                hours = course.Hours
            };

            return Json(result);
        }
        #endregion

        #region Create
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Subject,Hours")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();

                await _createDependenceTeacherCourse(course);
               
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        private async Task _createDependenceTeacherCourse(Course course)
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                switch (role)
                {
                    case "Teacher":
                        var teacher = await _context.Teachers
                            .Include(t => t.TeacherCourses)
                            .FirstOrDefaultAsync(t => t.AppUserId == user.Id);

                        var newCourse = new TeacherCourse()
                        {
                            Teacher = teacher,
                            TeacherId = teacher.AppUserId,
                            Course = course,
                            CourseId = course.Id,
                        };

                        _context.Add(newCourse);
                        await _context.SaveChangesAsync();
                        break;
                }
            }
        }
        #endregion

        #region Edit
        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(t => t.StudentCourses)
                .Include(t => t.TeacherCourses)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> TeacherForCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.CourseId = id;
            ViewBag.Admin = _admin;
            var course = await _context.Courses
                .Include(t => t.StudentCourses)
                .Include(t => t.TeacherCourses)
                .SingleOrDefaultAsync(m => m.Id == id);
            List<ApplicationUser> result = new List<ApplicationUser>();
            foreach (var student in course.TeacherCourses)
            {
                var ttt = await _userManager.FindByIdAsync(student.TeacherId);
                result.Add(ttt);
            }
            return PartialView(result);
        }

        [Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> StudentForCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.CourseId = id;
            var course = await _context.Courses
                .Include(t => t.StudentCourses)
                .Include(t => t.TeacherCourses)
                .SingleOrDefaultAsync(m => m.Id == id);
            List<ApplicationUser> result = new List<ApplicationUser>();
            foreach (var student in course.StudentCourses)
            {
                var ttt = await _userManager.FindByIdAsync(student.StudentId);
                result.Add(ttt);
            }
            return PartialView(result);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> RemoveStudentFromCourse(string idSudent, int? courseId)
        {
            if (courseId==null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(t => t.StudentCourses)
                .SingleOrDefaultAsync(m => m.Id == courseId);

            var student = course.StudentCourses.FirstOrDefault(t => t.StudentId == idSudent);
            if (student != null)
            {
                _context.Remove(student);
                await _context.SaveChangesAsync();
            }
            return Json(_result);
        }

        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> RemoveTeacherFromCourse(string idTeacher, int? courseId)
        {
            if (courseId == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(t => t.TeacherCourses)
                .SingleOrDefaultAsync(m => m.Id == courseId);

            var teacher = course.TeacherCourses.FirstOrDefault(t => t.TeacherId == idTeacher);
            if (teacher != null)
            {
                _context.Remove(teacher);
                await _context.SaveChangesAsync();
            }
            return Json(_result);
        }

        [Authorize(Roles = "Admin,Teacher")]
        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Subject,Hours")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }
        #endregion

        [Authorize(Roles = "Admin,Teacher")]
        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .SingleOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses.SingleOrDefaultAsync(m => m.Id == id);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return Json(_result);
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }

        public async Task<IActionResult> SearchString(string term)
        {
            var courses = await _context.Courses.ToListAsync();
            string[] result = new string[courses.Count()];
            for (int i = 0; i < courses.Count(); i++)
            {
                result[i] = courses[i].Name;
            }

            return Json(result.Where(x => !String.IsNullOrEmpty(x) &&
                                          x.ToLower().Contains(term.ToLower())).ToArray());
        }
    }
}
