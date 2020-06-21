using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using FluentAssertions;
using Moq;
using SchoolEM.Models.Students;
using SchoolEM.Models.Students.Exceptions;
using Tynamix.ObjectFiller;
using Xunit;

namespace SchoolEM.Tests.Services.StudentServiceTests
{
    public partial class StudentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRegisterIfStudentAlreadyExistsAndLogItAsync()
        {
            // given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student storageStudent = inputStudent;
            string someMessage = new MnemonicString().GetValue();
            var duplicateKeyException = new DuplicateKeyException(someMessage);
            var alreadyExistsStudentException = new AlreadyExistsStudentException(duplicateKeyException);
            var expectedStudentValidationException = new StudentValidationException(alreadyExistsStudentException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentAsync(inputStudent))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Student> submitStudentTask =
                this.studentService.RegisterStudentAsync(inputStudent);

            // then
            await Assert.ThrowsAsync<StudentValidationException>(() => submitStudentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(inputStudent),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThorwValidationExceptionOnRegisterIfStudentNameWasInvalidAndLogItAsync()
        {
            // given
            Student randomStudent = CreateRandomStudent();
            Student invalidStudent = randomStudent;
            string invalidName = string.Empty;
            invalidStudent.Name = invalidName;

            var invalidStudentException = new InvalidStudentException(
                parameterName: nameof(Student.Name),
                parameterValue: invalidStudent.Name);

            var expectedStudentValidationException =
                new StudentValidationException(invalidStudentException);

            // when
            ValueTask<Student> registerStudentTask =
                this.studentService.RegisterStudentAsync(invalidStudent);

            // then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                registerStudentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRegisterIfStudentIdWasInvalidAndLogItAsync()
        {
            //given
            Student randomStudent = CreateRandomStudent();
            Student invalidStudent = randomStudent;
            Guid invalidId = Guid.Empty;
            invalidStudent.Id = invalidId;

            var invalidStudentException = new InvalidStudentException(
                parameterName: nameof(Student.Id),
                parameterValue: invalidStudent.Id);

            var expectedStudentValidationException =
                new StudentValidationException(invalidStudentException);

            // when
            ValueTask<Student> registerStudentTask =
                this.studentService.RegisterStudentAsync(invalidStudent);

            // then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                registerStudentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveIfIdWasInvalidAndLogItAsync()
        {
            // given
            Guid invalidId = Guid.Empty;

            var invalidStudentException = new InvalidStudentException(
                parameterName: nameof(Student.Id),
                parameterValue: invalidId);

            var expectedStudentValidationException =
                new StudentValidationException(invalidStudentException);

            // when
            ValueTask<Student> retrieveStudentByIdTask =
                this.studentService.RetrieveStudentAsync(invalidId);

            // then
            await Assert.ThrowsAsync<StudentValidationException>(() => retrieveStudentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveIfStudentWasNotFoundAndLogItAsync()
        {
            // given 
            Student noStudent = null;
            Guid nonExistentStudentId = Guid.NewGuid();

            var notFoundStudentException =
                new NotFoundStudentException(studentId: nonExistentStudentId);

            var expectedStudentValidationException =
                new StudentValidationException(notFoundStudentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentByIdAsync(nonExistentStudentId))
                    .ReturnsAsync(noStudent);

            // when
            ValueTask<Student> retrieveStudentByIdTask =
                this.studentService.RetrieveStudentAsync(nonExistentStudentId);

            // then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                retrieveStudentByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(nonExistentStudentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentValidationException))),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ThrowValidationExceptionOnDeleteIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidId = default;
            Student randomStudent = CreateRandomStudent();
            Student storageStudent = randomStudent;
            Student expectedStudent = storageStudent;

            var studentValidationException =
                new InvalidStudentException(nameof(Student.Id), parameterValue: invalidId);

            var expectedStudentValidationException =
                new StudentValidationException(studentValidationException);

            //when
            ValueTask<Student> deleteStudentTask =
                this.studentService.DeleteStudentAsync(invalidId);

            //then
            await Assert.ThrowsAsync<StudentValidationException>(() => deleteStudentTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(It.IsAny<Guid>()), Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentAsync(It.IsAny<Student>()), Times.Never);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentValidationException))),
                    Times.Once);

        }

        [Fact]
        public void ShouldLogWarningOnRetrieveAllIfStudentsStorageWasEmptyAsync()
        {
            // given
            IQueryable<Student> emptyStudentsList = new List<Student>().AsQueryable();
            IQueryable<Student> expectedStudents = emptyStudentsList;

            string warningMessage = "Students storage is empty.";

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudents())
                    .Returns(emptyStudentsList);

            // when
            IQueryable<Student> actualStudents = this.studentService.RetrieveAllStudents();

            // then
            actualStudents.Should().BeEquivalentTo(expectedStudents);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudents(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning(warningMessage),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStudentIsNullAndLogItAsync()
        {
            //given
            Student noStudent = null;
            var nullStudentException = new NullStudentException();

            var expectedStudentValidationException =
                new StudentValidationException(nullStudentException);

            //when
            ValueTask<Student> modifyStudentTask = this.studentService.ModifyStudentAsync(noStudent);

            //then
            await Assert.ThrowsAsync<StudentValidationException>(() => modifyStudentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStudentIdIsInvalidAndLogItAsync()
        {
            //given
            Student randomStudent = CreateRandomStudent();
            Guid invalidStudentId = Guid.Empty;
            Student invalidStudent = randomStudent;
            invalidStudent.Id = invalidStudentId;

            var invalidStudentException = new InvalidStudentException(
                parameterName: nameof(Student.Id),
                parameterValue: invalidStudent.Id);

            var expectedStudentValidationException =
                new StudentValidationException(invalidStudentException);

            //when
            ValueTask<Student> modifyStudentTask =
                this.studentService.ModifyStudentAsync(invalidStudent);

            //then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                modifyStudentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }


        public static IEnumerable<object[]> InvalidStudentNameCases()
        {
            string emptyLabel = string.Empty;
            string noLabel = null;
            string whiteSpacelabel = "     ";

            return new List<object[]>
            {
                new object[] {emptyLabel},
                new object[] {noLabel},
                new object[] {whiteSpacelabel}
            };
        }

        [Theory]
        [MemberData(nameof(InvalidStudentNameCases))]
        public async Task ShouldThrowValidatioNExceptionOnModifyIfStudetnNameIsInvalidAndLogItAsync(string invalidName)
        {
            //given
            Student randomStudent = CreateRandomStudent();
            Student invalidStudent = randomStudent;
            invalidStudent.Name = invalidName;

            var invalidStudentException = new InvalidStudentException(
                parameterName: nameof(Student.Name),
                parameterValue: invalidStudent.Name);

            var expectedStudentValidationException =
                new StudentValidationException(invalidStudentException);

            //when
            ValueTask<Student> modifyStudentTask =
                this.studentService.ModifyStudentAsync(invalidStudent);

            //then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                modifyStudentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentAsync(It.IsAny<Student>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
;