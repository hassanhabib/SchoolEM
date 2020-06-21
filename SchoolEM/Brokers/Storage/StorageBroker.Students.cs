using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolEM.Models.Students;

namespace SchoolEM.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Student> Students { get; set; }

        public async ValueTask<Student> InsertStudentAsync(Student student)
        {
            EntityEntry<Student> storageStudent = await this.Students.AddAsync(student);
            await this.SaveChangesAsync();

            return storageStudent.Entity;
        }

        public async ValueTask<Student> SelectStudentByIdAsync(Guid studentId) =>
            await this.Students.FindAsync(studentId);

        public IQueryable<Student> SelectAllStudents() =>
            this.Students.AsQueryable();

        public async ValueTask<Student> DeleteStudentAsync(Student student)
        {
            EntityEntry<Student> storageStudent = this.Students.Remove(student);
            await this.SaveChangesAsync();

            return storageStudent.Entity;
        }

        public async ValueTask<Student> UpdateStudentAsync(Student student)
        {
            EntityEntry<Student> storageStudent = this.Students.Update(student);
            await this.SaveChangesAsync();

            return storageStudent.Entity;
        }
    }
}
