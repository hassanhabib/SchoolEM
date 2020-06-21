using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using SchoolEM.Models.Students;
using SchoolEM.Models.Students.Exceptions;
using Xunit;

namespace SchoolEM.Tests.Services.StudentServiceTests
{
    public partial class StudentServiceTests
    {
        [Fact]
        public async Task ShouldThrowServiceExceptionOnRegisterIfServiceFailureOcurredAndLogItAsync()
        {
            // given
            Student someStudent = CreateRandomStudent();
            var serviceException = new Exception();

            var expectedStudentServiceException =
                new StudentServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAsync(someStudent))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<Student> registerStudentTask =
                this.studentService.RegisterStudentAsync(someStudent);

            // then
            await Assert.ThrowsAsync<StudentServiceException>(() =>
                registerStudentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(someStudent),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentServiceException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRegisterIfDatabaseUpdateExceptionThrownAndLogItAsync()
        {
            //given
            Student someStudent = CreateRandomStudent();
            var dbUpdateException = new DbUpdateException();

            var expectedStudentDependencyException =
               new StudentDependencyException(dbUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAsync(someStudent))
                    .ThrowsAsync(dbUpdateException);

            //when
            ValueTask<Student> registerStudentTask =
                this.studentService.RegisterStudentAsync(someStudent);

            // then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                registerStudentTask.AsTask());


            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(someStudent),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveIfDatabaseUpdateExceptionThrownAndLogItAsync()
        {
            // given
            Guid someId = Guid.NewGuid();

            var databaseUpdateException = new DbUpdateException();
            var expectedStudentDependencyException = new StudentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentByIdAsync(someId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<Student> retrieveStudentTask =
                this.studentService.RetrieveStudentAsync(someId);

            // then
            await Assert.ThrowsAsync<StudentDependencyException>(() => retrieveStudentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(someId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowStudentServiceExceptionnOnRetrieveIfGenericExceptionThrownAndLogItAsync()
        {
            //given
            Guid someId = Guid.NewGuid();

            var serviceException = new Exception();
            var expectedServiceException = new StudentServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentByIdAsync(someId))
                    .ThrowsAsync(serviceException);

            //when
            ValueTask<Student> retrieveStudentTask =
                this.studentService.RetrieveStudentAsync(someId);

            //then
            await Assert.ThrowsAsync<StudentServiceException>(() => retrieveStudentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
              broker.SelectStudentByIdAsync(someId),
                 Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedServiceException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRetrieveIfSqlExceptionWasThrownAndLogItAsync()
        {
            // given
            Guid someStudentId = Guid.NewGuid();
            SqlException sqlException = GetSqlException();

            var expectedDependencyException =
                new StudentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentByIdAsync(someStudentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Student> retrieveStudentByIdTask =
                this.studentService.RetrieveStudentAsync(someStudentId);

            // then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                retrieveStudentByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(someStudentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowCriticalDependencyExceptionOnRegisterIfSqlExceptionWasThrownAndLogItAsync()
        {
            // given
            Student student = CreateRandomStudent();
            SqlException sqlException = GetSqlException();

            var expectedDependencyException =
                new StudentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAsync(student))
                    .ThrowsAsync(sqlException);

            //when
            ValueTask<Student> registerStudentById =
                this.studentService.RegisterStudentAsync(student);

            //then
            await Assert.ThrowsAsync<StudentDependencyException>(() =>
                registerStudentById.AsTask());

            this.storageBrokerMock.Verify(broker =>
               broker.InsertStudentAsync(student),
                   Times.Once);

            this.loggingBrokerMock.Verify(broker =>
              broker.LogCritical(It.Is(SameExceptionAs(expectedDependencyException))),
                  Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowCriticalDependencyExceptionOnRetrieveAllIfSqlExceptionWasThrownAndLogItAsync()
        {
            // given
            SqlException sqlException = GetSqlException();
            var expectedStudentDependencyException = new StudentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudents())
                    .Throws(sqlException);

            // when . then
            Assert.Throws<StudentDependencyException>(() =>
                this.studentService.RetrieveAllStudents());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudents(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
