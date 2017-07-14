using Microsoft.AspNet.Identity;
using SwiftSkool.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SwiftSkool.BusinessLogic
{
    public class QueryManager : IDisposable
    {
        private readonly ApplicationDbContext _db;

        public QueryManager()
        {
            _db = new ApplicationDbContext();
        }


        //public static Func<ApplicationDbContext, IQueryable<Session>>
        //    CompliedQueryForSessionList = CompiledQuery.Compile(
        //        (ApplicationDbContext context) =>
        //            from c in context.Sessions select c);




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

            //var query = from pro in _db.Classes.AsNoTracking()
            //            where pro.SchoolId.Equals(schoolId)
            //            select new Class { FullClassName = pro.FullClassName };

            //return query.ToList();
        }
        public async Task<List<Staff>> StaffListAsync(string schoolId)
        {
            return await _db.Staffs.AsNoTracking().Where(x => x.SchoolId.Equals(schoolId)).ToListAsync();

            //var query = from pro in _db.Classes.AsNoTracking()
            //            where pro.SchoolId.Equals(schoolId)
            //            select new Class { FullClassName = pro.FullClassName };

            //return query.ToList();
        }

        public async Task<List<Subject>> SubjectListAsync(string schoolId)
        {
            return await _db.Subjects.AsNoTracking().Where(x => x.SchoolId.Equals(schoolId)).ToListAsync();

            //var query = from pro in _db.Classes.AsNoTracking()
            //            where pro.SchoolId.Equals(schoolId)
            //            select new Class { FullClassName = pro.FullClassName };

            //return query.ToList();
        }

        public List<SchoolClass> SchoolClassListAsync()
        {
            return _db.SchoolClasses.AsNoTracking().ToList()
                .Select(x => new SchoolClass { ClassCode = x.ClassCode }).ToList();
        }

        public List<Session> SessionList()
        {
            // return await _db.Sessions.AsNoTracking().ToListAsync();
            return _db.Sessions.AsNoTracking().ToList()
                     .Select(x => new Session { SessionName = x.SessionName }).ToList();
        }



        public List<Term> TermList()
        {
            //return await _db.Terms.AsNoTracking().ToListAsync();
            return _db.Terms.AsNoTracking().ToList()
                .Select(x => new Term { TermName = x.TermName }).ToList();
        }

        public string GetId()
        {
            var user = HttpContext.Current.User.Identity.GetUserId();
            return _db.Users.AsNoTracking().Where(x => x.Id.Equals(user))
                 .Select(s => s.SchoolId).FirstOrDefault();
        }

        public void Dispose()
        {
            _db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}