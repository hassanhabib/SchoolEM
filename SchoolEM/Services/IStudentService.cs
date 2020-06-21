using System;
using System.Linq;
using System.Threading.Tasks;
using SchoolEM.Models.Students;

namespace SchoolEM.Services
{
    public interface IStudentService
    {
        ValueTask<Student> RegisterStudentAsync(Student student);
        ValueTask<Student> RetrieveStudentAsync(Guid studentId);
        IQueryable<Student> RetrieveAllStudents();
        ValueTask<Student> DeleteStudentAsync(Guid studentId);
        ValueTask<Student> ModifyStudentAsync(Student student);
    }
}
