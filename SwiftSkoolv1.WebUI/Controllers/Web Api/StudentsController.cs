using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.Models;
using SwiftSkoolv1.WebUI.ViewModels;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace SwiftSkoolv1.WebUI.Controllers.Web_Api
{
    public class StudentsController : BaseApiController
    {

        public StudentsController()
        {

        }

        public StudentsController(SwiftSkoolDbContext db) : base(db)
        {
            
        }

        // GET: api/Students
        /// <summary>
        /// Return a List-Like structure with students.
        /// Uses a public property from BaseApiController called CurrentUser
        /// which captures and returns the current user that is logged in.
        /// </summary>
        /// <returns>Task<IQueryable<StudentClientViewModel>></StudentClientViewModel></returns>
        public async Task<IQueryable<StudentClientViewModel>> GetStudents()
        {
            var students = await _db.Students.AsNoTracking().Where(s => s.UserName.Equals(User.Identity.Name))
                              .Select(s => new StudentClientViewModel
                              {
                                  StudentId = s.StudentId,
                                  FirstName = s.FirstName,
                                  MiddleName = s.MiddleName,
                                  LastName = s.LastName,
                                  PhoneNumber = s.PhoneNumber,
                                  DateOfBirth = s.DateOfBirth,
                                  StateOfOrigin = s.StateOfOrigin,
                                  Gender = s.Gender,
                                  Religion = s.Religion,
                                  AdmissionDate = s.AdmissionDate,
                                  UserName = s.UserName
                              }).ToListAsync();

            return students.AsQueryable();
        }

        // GET: api/Students/5
        [ResponseType(typeof(StudentClientViewModel))]
        public async Task<IHttpActionResult> GetStudent(string id)
        {
            var student = await _db.Students.AsNoTracking().Where(s => s.StudentId.Equals(id))
                                   .Select(s => new StudentClientViewModel
                                   {
                                       StudentId = s.StudentId,
                                       FirstName = s.FirstName,
                                       MiddleName = s.MiddleName,
                                       LastName = s.LastName,
                                       PhoneNumber = s.PhoneNumber,
                                       DateOfBirth = s.DateOfBirth,
                                       StateOfOrigin = s.StateOfOrigin,
                                       Gender = s.Gender,
                                       Religion = s.Religion,
                                       AdmissionDate = s.AdmissionDate,
                                       UserName = s.UserName
                                   }).SingleOrDefaultAsync();
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/Students/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStudent(string id, Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.StudentId)
            {
                return BadRequest();
            }

            _db.Entry(student).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Students
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> PostStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Students.Add(student);

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StudentExists(student.StudentId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = student.StudentId }, student);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> DeleteStudent(string id)
        {
            Student student = await _db.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _db.Students.Remove(student);
            await _db.SaveChangesAsync();

            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(string id)
        {
            return _db.Students.Count(e => e.StudentId == id) > 0;
        }
    }
}