using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Models
{
    public class TodoContext : DbContext
    {
        // 必须要有一个空参构造器
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        // 用于操作数据库
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
