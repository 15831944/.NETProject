using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Entity
{
    [Table("Class_Student")]
    public class ClassStudent
    {
        [Column("class_id")]
        public int class_id;
        [Column("student_id")]
        public int student_id;
    }
}
