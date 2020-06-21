using System;
using System.Linq;
using System.Threading.Tasks;
using SchoolEM.Models.Students;

namespace SchoolEM.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        ValueTask<Student> InsertStudentAsync(Student student);
        ValueTask<Student> SelectStudentByIdAsync(Guid Id);
        IQueryable<Student> SelectAllStudents();
        ValueTask<Student> DeleteStudentAsync(Student student);
        ValueTask<Student> UpdateStudentAsync(Student student);
    }
}
