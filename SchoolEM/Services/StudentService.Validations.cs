using System;
using System.Linq;
using FluentValidation;
using SchoolEM.Models.Students;
using SchoolEM.Models.Students.Exceptions;

namespace SchoolEM.Services
{
    public partial class StudentService
    {
        public void ValidateStudent(Student student)
        {
            ValidateStudentIsNotNull(student);
            this.studentValidator.ValidateAndThrow(student);
        }

        private void ValidateStudentIsNotNull(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }
        }

        private void ValidateStudentId(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                throw new InvalidStudentException(
                    parameterName: nameof(Student.Id),
                    parameterValue: studentId);
            }
        }

        private static void ValidateStorageStudent(Student storageStudent, Guid studentId)
        {
            if (storageStudent == null)
            {
                throw new NotFoundStudentException(studentId);
            }
        }

        private void ValidateStorageStudents(IQueryable<Student> storageStudents)
        {
            if (storageStudents.Count() == 0)
            {
                this.loggingBroker.LogWarning("Students storage is empty.");
            }
        }

        public class StudentValidator : AbstractValidator<Student>
        {
            public StudentValidator()
            {
                RuleFor(student => student).NotNull();
                RuleFor(student => student.Id).NotEmpty();
                RuleFor(student => student.Name).NotEmpty();
            }
        }
    }
}
