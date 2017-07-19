using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.APIRepository.RepoAbstractions;
using SwiftSkoolv1.WebUI.Models;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace SwiftSkoolv1.WebUI.APIRepository.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SwiftSkoolDbContext _db;


        public StudentRepository(SwiftSkoolDbContext db)
        {
            _db = db;
        }


        public void Add(Student entity)
        {
            if(entity != null)
                _db.Students.Add(entity);
            throw new DbEntityValidationException("You must pass a valid Student for you to even try to save it in the application");
        }

        public void Delete(Student entity)
        {
            do
            {
                _db.Students.Remove(entity);
            } while (entity != null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Student> GetAll()
        {
            return _db.Students;
        }

        public async Task<Student> GetByIdUnTrackedAsync(string id)
        {
            return await _db.Students.AsNoTracking()
                            .Where(s => s.StudentId.Equals(id)).SingleOrDefaultAsync();
        }

        public async Task<Student> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id.Trim()) && string.IsNullOrWhiteSpace(id.Trim()))
                throw new ArgumentException("You must pass a valid Student Id!");
            return await _db.Students.FindAsync(id);
        }

        //public StudentClientViewModel MapToVM(Student entity)
        //{
        //    var student = GetAll();
        //    StudentId = s.StudentId,
        //    FirstName = s.FirstName,
        //    MiddleName = student.MiddleName,
        //    LastName = s.LastName,
        //    PhoneNumber = s.PhoneNumber,
        //    DateOfBirth = s.DateOfBirth,
        //    StateOfOrigin = s.StateOfOrigin,
        //    Gender = s.Gender,
        //    Religion = s.Religion,
        //    AdmissionDate = s.AdmissionDate,
        //    UserName = s.UserName
        //}

        public IList<StudentClientViewModel> MapToVMSequence(Student entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Student entity)
        {
            throw new NotImplementedException();
        }
    }
}