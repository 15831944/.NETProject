using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Entity
{
    [Table("Student")]
    public class Student
    {
        [Column("student_id")]
        [Key]
        public int student_id { get; set; }

        [Column("student_name")]
        public string student_name { get; set; }

        [Column("student_gender")]
        public string student_gender { get; set; }
    }
}
