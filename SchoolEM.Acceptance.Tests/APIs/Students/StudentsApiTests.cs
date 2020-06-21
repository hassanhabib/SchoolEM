using System;
using System.Threading.Tasks;
using FluentAssertions;
using SchoolEM.Acceptance.Tests.Brokers;
using SchoolEM.Acceptance.Tests.Models.Students;
using Tynamix.ObjectFiller;
using Xunit;

namespace SchoolEM.Acceptance.Tests.APIs.Students
{
    [Collection(nameof(ApiTestCollection))]
    public class StudentsApiTests
    {
        private readonly SchoolEMApiBroker schoolEMApiBroker;

        public StudentsApiTests(SchoolEMApiBroker schoolEMApiBroker) =>
            this.schoolEMApiBroker = schoolEMApiBroker;

        private Student CreateRandomStudent() =>
            new Filler<Student>().Create();

        [Fact]
        public async Task ShouldPostStudentAsync()
        {
            // given
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;
            Student expectedStudent = inputStudent;

            // when 
            await this.schoolEMApiBroker.PostStudentAsync(inputStudent);

            Student actualStudent =
                await this.schoolEMApiBroker.GetStudentByIdAsync(inputStudent.Id);

            // then
            actualStudent.Should().BeEquivalentTo(expectedStudent);

            await this.schoolEMApiBroker.DeleteStudentByIdAsync(actualStudent.Id);
        }

    }
}
