using System;
using System.Linq;
using System.Threading.Tasks;
using SchoolEM.Brokers.Logging;
using SchoolEM.Brokers.Storage;
using SchoolEM.Models.Students;

namespace SchoolEM.Services
{
    public partial class StudentService : IStudentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly StudentValidator studentValidator;

        public StudentService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.studentValidator = new StudentValidator();
        }

        public ValueTask<Student> RegisterStudentAsync(Student student) =>
        TryCatch(async () =>
        {
            ValidateStudent(student);

            return await this.storageBroker.InsertStudentAsync(student);
        });

        public ValueTask<Student> RetrieveStudentAsync(Guid studentId) =>
        TryCatch(async () =>
        {
            ValidateStudentId(studentId);

            Student maybeStudent =
                await this.storageBroker.SelectStudentByIdAsync(studentId);

            ValidateStorageStudent(maybeStudent, studentId);

            return maybeStudent;
        });

        public IQueryable<Student> RetrieveAllStudents() =>
        TryCatch(() =>
        {
            IQueryable<Student> maybeStudents = this.storageBroker.SelectAllStudents();
            ValidateStorageStudents(maybeStudents);

            return maybeStudents;
        });

        public ValueTask<Student> DeleteStudentAsync(Guid studentId) =>
        TryCatch(async () =>
        {
            ValidateStudentId(studentId);

            Student maybeStudent =
                await this.storageBroker.SelectStudentByIdAsync(studentId);

            ValidateStorageStudent(maybeStudent, studentId);

            return await this.storageBroker.DeleteStudentAsync(maybeStudent);
        });

        public ValueTask<Student> ModifyStudentAsync(Student student) =>
        TryCatch(async () =>
        {
            ValidateStudent(student);

            Student maybeStudent =
                await this.storageBroker.SelectStudentByIdAsync(student.Id);

            return await this.storageBroker.UpdateStudentAsync(student);
        });
    }
}
