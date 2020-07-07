using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entity;

namespace WebApplication.DAO
{
    public class StudentContext : DbContext
    {
        public StudentContext(DbContextOptions<StudentContext> opt) 
            : base(opt)
        {
        }

        public DbSet<Student> Students { get; set; }
    }
}
