using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using SchoolEM.Brokers.Logging;
using SchoolEM.Brokers.Storage;
using SchoolEM.Models.Students;
using SchoolEM.Services;
using Tynamix.ObjectFiller;
using static SchoolEM.Services.StudentService;

namespace SchoolEM.Tests.Services.StudentServiceTests
{
    public partial class StudentServiceTests
    {
        private readonly Mock<IStorageBroker> storageBrokerMock;
        private readonly IStudentService studentService;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly StudentValidator studentValidator;
        
        public StudentServiceTests()
        {
            this.storageBrokerMock = new Mock<IStorageBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();
            this.studentValidator = new StudentValidator();

            this.studentService = new StudentService(
                storageBroker: this.storageBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private Expression<Func<Exception, bool>> SameExceptionAs(
            Exception expectedException)
        {
            return actualException => actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private SqlException GetSqlException() =>
            FormatterServices.GetUninitializedObject(typeof(SqlException)) as SqlException;

        private Student CreateRandomStudent()
        {
            return new Filler<Student>().Create();
        }

        private IQueryable<Student> CreateRandomStudents()
        {
            int randomNumber = new IntRange(min: 2, max: 10).GetValue();

            return new Filler<Student>().Create(randomNumber).AsQueryable();
        }
    }
}
