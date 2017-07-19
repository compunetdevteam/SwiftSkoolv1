using SwiftSkoolv1.WebUI.BusinessLogic;
using SwiftSkoolv1.WebUI.Models;
using SwiftSkoolv1.WebUI.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace SwiftSkoolv1.WebUI.Controllers.Web_Api
{
    public class BaseApiController : ApiController
    {
        protected readonly SwiftSkoolDbContext _db;
        protected readonly QueryManager _qmgr;
        protected readonly ResultCommandManager _rcmg;

        public BaseApiController(SwiftSkoolDbContext Db)
        {
            _db = Db;
            SetSchoolByUserId();
        }

        public BaseApiController()
        {

        }

        /// <summary>
        /// Get the currently logged in user and find the schoolId
        /// of the logged in users school
        /// </summary>
        protected async void SetSchoolByUserId()
        {
            var user = string.Empty;

            if (User.Identity.IsAuthenticated)
            {
                if(User.IsInRole("Student") || User.IsInRole("Teacher"))
                {
                    user = User.Identity.Name;
                }
                else if (User.IsInRole("SuperAdmin"))
                {
                    user = User.Identity.Name;
                }
                else
                {
                    
                }
            }
                                         

            var schoolId = await _db.Users.AsNoTracking().Where(u => u.UserName.Equals(user))
                                          .Select(u => u.SchoolId)
                                          .SingleOrDefaultAsync();

            var school = await _db.Schools.FindAsync(schoolId);

            var model = new RestViewModel();

            if (school != null)
            {
                model.Alias = school.Alias;
                model.SchoolName = school.Name;
                model.SchoolId = school.SchoolId;
                model.Color = school.Color;
                model.ImageId = school.SchoolId;
            }
            else
            {
                model.Alias = "SwiftSkool";
                model.SchoolName = "SwiftSkool";
                model.SchoolId = "SwiftSkool";
                model.Color = "";
            }

            model.SchoolId = "";
        }
    }
}