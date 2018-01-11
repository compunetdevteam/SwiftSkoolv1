using Microsoft.AspNet.Identity;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SwiftSkoolv1.WebUI.BusinessLogic
{
    public class QueryManager : IDisposable
    {
        private readonly SwiftSkoolDbContext _db;

        public QueryManager()
        {
            _db = new SwiftSkoolDbContext();
        }


        public async Task<List<Student>> StudentListAsync(string schoolId)
        {
            return await _db.Students.AsNoTracking().Where(x => x.SchoolId.Equals(schoolId))
                                    .ToListAsync();
            //return _db.Students.AsNoTracking().Where(x => x.SchoolId.Equals(schoolId)).ToList()
            //    .Select(x => new Student { StudentId = x.StudentId, FullName = x.FullName }).ToList();
        }

        public async Task<List<Class>> ClassListAsync(string schoolId)
        {
            return await _db.Classes.AsNoTracking().Where(x => x.SchoolId.Equals(schoolId)).ToListAsync();
        }
        public async Task<List<Staff>> StaffListAsync(string schoolId)
        {
            return await _db.Staffs.AsNoTracking().Where(x => x.SchoolId.Equals(schoolId)).ToListAsync();
        }

        public async Task<List<Subject>> SubjectListAsync(string schoolId)
        {
            return await _db.Subjects.AsNoTracking().Where(x => x.SchoolId.Equals(schoolId)).ToListAsync();
        }

        public List<SchoolClass> SchoolClassListAsync(string schoolId)
        {
            return _db.SchoolClasses.AsNoTracking().Where(x => x.SchoolId.Equals(schoolId)).ToList()
            .Select(x => new SchoolClass { ClassCode = x.ClassCode }).ToList();
        }

        public List<Session> SessionList()
        {
            return _db.Sessions.AsNoTracking().ToList()
                     .Select(x => new Session { SessionName = x.SessionName }).ToList();
        }



        public List<Term> TermList()
        {
            return _db.Terms.AsNoTracking().ToList()
                .Select(x => new Term { TermName = x.TermName }).ToList();
        }

        public string GetId()
        {
            var user = HttpContext.Current.User.Identity.GetUserId();
            return _db.Users.AsNoTracking().Where(x => x.Id.Equals(user))
                 .Select(s => s.SchoolId).FirstOrDefault();
        }

        public string CurrentTerm()
        {
            return _db.Terms.AsNoTracking().Where(x => x.ActiveTerm.Equals(true))
                .Select(s => s.TermName).FirstOrDefault();
        }
        public string CurrentSession()
        {
            return _db.Sessions.AsNoTracking().Where(x => x.ActiveSession.Equals(true))
                        .Select(s => s.SessionName).FirstOrDefault();
        }

        public string GetMyClass(string userSchool, string studentId)
        {
            var currentTerm = CurrentTerm();
            var currentSession = CurrentSession();
            return _db.AssignedClasses.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)
                            && x.StudentId.Equals(studentId) && x.TermName.Equals(currentTerm) &&
                            x.SessionName.Equals(currentSession))
                            .Select(s => s.ClassName).FirstOrDefault();

        }

        public List<Subject> GetStudentSubject(string _studentId, string _schoolId, string _className, string _termName)
        {
            var subjectAssigned = _db.AssignSubjects.AsNoTracking().Where(c => c.SchoolId.ToUpper().Trim().Equals(_schoolId)
                                    && c.ClassName.ToUpper().Trim().Equals(_className.ToUpper().Trim())
                                    && c.TermName.ToUpper().Trim().Equals(_termName.ToUpper().Trim()))
                                    .Select(s => s.Subject).ToList();

            var subjectregistration = _db.SubjectRegistrations.AsNoTracking()
                            .Where(x => x.SchoolId.ToUpper().Trim().Equals(_schoolId) &&
                            x.StudentId.ToUpper().Trim().Equals(_studentId))
                            .Select(s => s.Subject).ToList();

            if (subjectregistration.Count() <= 8)
            {
                return subjectAssigned;
            }
            return subjectregistration;
        }

        public void Dispose()
        {
            _db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}