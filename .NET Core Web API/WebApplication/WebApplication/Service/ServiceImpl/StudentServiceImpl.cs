using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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

        public ClassStudentContext _context1;

        public StudentServiceImpl(StudentContext context, ClassStudentContext context1)
        {
            _context = context;
            _context1 = context1;
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
            
            return _context.Students.FromSqlRaw("SELECT * FROM Student WHERE student_id = @id", new SqlParameter("id", id)).First();
            // return  _context.Students.SingleOrDefault(s => s.student_id == id);
        }

        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.FromSqlRaw("SELECT * FROM Student")
                .OrderBy(b => b.student_id)
                .ToListAsync();
            // return await _context.Students.ToListAsync();
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

        public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByClassId(int id)
        {
            /*
            TRUNCATE TABLE Class;
            TRUNCATE TABLE Class_Student;
            INSERT INTO Class(class_id, class_name) VALUES (1, 'C language');
            INSERT INTO Class(class_id, class_name) VALUES (2, 'JAVA language');
            INSERT INTO Class_Student(class_id, student_id) VALUES (1, 1);
            INSERT INTO Class_Student(class_id, student_id) VALUES (1, 2);
            INSERT INTO Class_Student(class_id, student_id) VALUES (2, 1);
            INSERT INTO Class_Student(class_id, student_id) VALUES (2, 3);
            SELECT * FROM Student;
            SELECT * FROM Class;
            SELECT * FROM Class_Student;
            SELECT Student.* FROM Student JOIN Class_Student ON (Student.student_id = Class_Student.student_id) WHERE class_id = 1;
             */
            /*
             var query = _context1.Students
                    .Join(
                        _context1.ClassStudents,
                        student => student.student_id,
                        classStudent => classStudent.student_id,
                        (student, classStudent) => new 
                        { 
                            student_id = student.student_id,
                            student_gender = student.student_gender,
                            student_name = student.student_name,
                            class_id = classStudent.class_id
                        }
                    )
                    .Where(table => table.class_id == id)
                    .Select(table => new Student 
                    {
                        student_id = table.student_id,
                        student_name = table.student_name,
                        student_gender = table.student_gender
                    });
            */
            var query2 = _context1.Students
                    .Join(
                        _context1.ClassStudents,
                        student => student.student_id,
                        classStudent => classStudent.student_id,
                        (student, classStudent) => new { S = student, C = classStudent }
                    )
                    .Where(table => table.C.class_id == id)
                    .Select(table => table.S);

            /*
            var query1 = from student in _context1.Students
                         join classStudent in _context1.ClassStudents
                            on student.student_id equals classStudent.student_id
                         where classStudent.class_id == id
                         select student;
            */
            return await query2.ToListAsync();
        }
    }
}
