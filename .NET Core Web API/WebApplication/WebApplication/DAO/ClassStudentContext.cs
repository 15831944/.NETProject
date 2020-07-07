using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entity;

namespace WebApplication.DAO
{
    public class ClassStudentContext : DbContext
    {
        public ClassStudentContext(DbContextOptions<ClassStudentContext> opt)
             : base(opt)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            EntityTypeBuilder<ClassStudent> entityTypeBuilder = modelBuilder.Entity<ClassStudent>();
            entityTypeBuilder.ToTable("Class_Student");
            entityTypeBuilder.HasNoKey();
        }
        
        public DbSet<ClassStudent> ClassStudents { get; set; }
    }
}
