using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DAO;
using WebApplication.Entity;

namespace WebApplication.Service.ServiceImpl
{
    public class StudentServiceImpl : StudentService
    {

        public StudentContext _context;

        public StudentServiceImpl(StudentContext context)
        {
            _context = context;
        }


        public bool CreateStudent(Student student)
        {
            _context.Students.Add(student);
            return _context.SaveChanges() > 0;
        }

        public bool DeleteStudentByID(int id)
        {
            var student = _context.Students.SingleOrDefault(s => s.student_id == id);
            _context.Students.Remove(student);
            return _context.SaveChanges() > 0;
        }

        public Student GetStudentById(int id)
        {
            return _context.Students.SingleOrDefault(s => s.student_id == id);
        }

        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public bool UpdateNameByID(int id, string name)
        {
            var state = false;
            var student = _context.Students.SingleOrDefault(s => s.student_id == id);
            if (student != null)
            {
                student.student_name = name;
                state = _context.SaveChanges() > 0;
            }

            return state;
        }

        public bool UpdateStudent(Student student)
        {
            _context.Students.Update(student);
            return _context.SaveChanges() > 0;
        }
    }
}
