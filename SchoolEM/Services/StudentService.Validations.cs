using System;
using System.Linq;
using SchoolEM.Models.Students;
using SchoolEM.Models.Students.Exceptions;

namespace SchoolEM.Services
{
    public partial class StudentService
    {

        public void ValidateStudent(Student student)
        {
            ValidateStudentIsNotNull(student);
            ValidateStudentId(student.Id);
            ValidateStudentName(student);
        }

        private void ValidateStudentIsNotNull(Student student)
        {
            if (student is null)
            {
                throw new NullStudentException();
            }
        }

        private static void ValidateStudentName(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.Name))
            {
                throw new InvalidStudentException(
                    parameterName: nameof(Student.Name),
                    parameterValue: student.Name);
            }
        }

        private static void ValidateStudentId(Guid studentId)
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
    }
}
