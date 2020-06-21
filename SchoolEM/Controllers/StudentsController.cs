using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using SchoolEM.Models.Students;
using SchoolEM.Models.Students.Exceptions;
using SchoolEM.Services;

namespace SchoolEM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService studentService;
        public StudentsController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Student>> PostStudentAsync([FromBody] Student student)
        {
            try
            {
                Student storageStudent = await this.studentService.RegisterStudentAsync(student);
                return Ok(storageStudent);
            }
            catch (StudentValidationException studentValidationException)
                when (studentValidationException.InnerException is AlreadyExistsStudentException)
            {
                return Conflict(studentValidationException.InnerException.Message);
            }
            catch (StudentValidationException studentValidationException)
            {
                return BadRequest(studentValidationException.InnerException.Message);
            }
            catch (StudentDependencyException studentDependencyException)
            {
                return Problem(studentDependencyException.Message);
            }
            catch (StudentServiceException studentServiceException)
            {
                return Problem(studentServiceException.Message);
            }
        }

        [HttpGet("{studentId}")]
        public async ValueTask<ActionResult<Student>> GetStudentAsync(Guid studentId)
        {
            try
            {
                Student storageStudent =
                    await this.studentService.RetrieveStudentAsync(studentId);

                return Ok(storageStudent);
            }
            catch (StudentValidationException studentValidationException)
                when (studentValidationException.InnerException is NotFoundStudentException)
            {
                return NotFound(studentValidationException.InnerException.Message);
            }
            catch (StudentValidationException studentValidationException)
            {
                return BadRequest(studentValidationException.Message);
            }
            catch (StudentDependencyException studentDependencyException)
            {
                return Problem(studentDependencyException.Message);
            }
            catch (StudentServiceException studentServiceException)
            {
                return Problem(studentServiceException.Message);
            }
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IQueryable<Student>> GetStudents()
        {
            try
            {
                IQueryable<Student> allStudents = this.studentService.RetrieveAllStudents();

                return Ok(allStudents);
            }
            catch (StudentValidationException studentValidationException)
            {
                return BadRequest(studentValidationException.Message);
            }
            catch (StudentDependencyException studentDependencyException)
            {
                return Problem(studentDependencyException.Message);
            }
            catch (StudentServiceException studentServiceException)
            {
                return Problem(studentServiceException.Message);
            }
        }

        [HttpDelete("{studentId}")]
        public async ValueTask<ActionResult<Student>> DeleteStudentAsync(Guid studentId)
        {
            try
            {
                Student storageStudent =
                    await this.studentService.DeleteStudentAsync(studentId);

                return Ok(storageStudent);
            }
            catch (StudentValidationException studentValidationException)
                when (studentValidationException.InnerException is NotFoundStudentException)
            {
                return NotFound(studentValidationException.InnerException.Message);
            }
            catch (StudentValidationException studentValidationException)
            {
                return BadRequest(studentValidationException.Message);
            }
            catch (StudentDependencyException studentDependencyException)
            {
                return Problem(studentDependencyException.Message);
            }
            catch (StudentServiceException studentServiceException)
            {
                return Problem(studentServiceException.Message);
            }
        }
    }
}
