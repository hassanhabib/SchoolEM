using System;
using System.Threading.Tasks;
using SchoolEM.Acceptance.Tests.Models.Students;

namespace SchoolEM.Acceptance.Tests.Brokers
{
    public partial class SchoolEMApiBroker
    {
        private const string StudentsRelativeUrl = "api/students";

        public async ValueTask<Student> PostStudentAsync(Student student) =>
            await this.apiFactoryClient.PostContentAsync(StudentsRelativeUrl, student);

        public async ValueTask<Student> GetStudentByIdAsync(Guid studentId) =>
            await this.apiFactoryClient.GetContentAsync<Student>($"{StudentsRelativeUrl}/{studentId}");

        public async ValueTask<Student> DeleteStudentByIdAsync(Guid studentId) =>
            await this.apiFactoryClient.DeleteContentAsync<Student>($"{StudentsRelativeUrl}/{studentId}");
    }
}
