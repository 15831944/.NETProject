using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Entity;
using WebApplication.Service;
using WebApplication.Service.ServiceImpl;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }


        [HttpPost]
        [Route("add")]
        public ActionResult<string> Create(Student student)
        {
    
            if (string.IsNullOrEmpty(student.student_name.Trim()))
            {
                return "姓名不能为空";
            }
            if (!student.student_gender.Equals("0") && !student.student_gender.Equals("1"))
            {
                return "性别数据有误";
            }
            var result = _studentService.CreateStudent(student);
            if (result)
            {
                return "学生插入成功";
            }
            else
            {
                return "学生插入失败";
            }

        }

        //取全部记录
        [HttpGet]
        [HttpPost]
        [Route("getAll")]
        public Task<ActionResult<IEnumerable<Student>>> Gets()
        {
            return _studentService.GetStudents();
        }


    }
}
