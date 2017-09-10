using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTaskUniversity.Models.Users;

namespace TestTaskUniversity.ViewModels
{
    public class AllUsersViewModel
    {
        public IList<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public List<Teacher> Teachers { get; set; } = new List<Teacher>();
        public List<Student> Students { get; set; } = new List<Student>();
    }
}
