using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entity;

namespace WebApplication.Service
{
    public interface StudentService
    {
        bool CreateStudent(Student student);

        Task<ActionResult<IEnumerable<Student>>> GetStudents();

        Student GetStudentById(int id);

        bool UpdateStudent(Student student);

        bool UpdateNameByID(int id, string name);

        bool DeleteStudentByID(int id);
    }
}
