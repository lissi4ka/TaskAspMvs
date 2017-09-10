using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestTaskUniversity.Models.Users;

namespace TestTaskUniversity.Models.DbContext
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }
        public DbSet<TestTaskUniversity.Models.Users.ApplicationUser> ApplicationUser { get; set; }
    }
}
