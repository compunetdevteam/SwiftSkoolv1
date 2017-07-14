using SwiftSkool.BusinessLogic;
using SwiftSkool.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using SwiftSkoolv1.WebUI.ViewModels;

namespace HopeAcademySMS.Controllers.Web_Api
{
    public class BaseApiController : ApiController
    {
        protected readonly ApplicationDbContext _db;
        protected readonly QueryManager _qmgr;
        protected readonly ResultCommandManager _rcmg;


        public BaseApiController(ApplicationDbContext db)
        {
            _db = db;
        }


        protected async override void Initialize(HttpControllerContext context)
        {
            string user = string.Empty;

            base.Initialize(context);

            if (context.RequestContext.Principal.Identity.IsAuthenticated)
            {
                user = context.RequestContext.Principal.Identity.Name;
            }

            var schoolId = await _db.Users.AsNoTracking().Where(u => u.UserName.Contains(user))
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
        }
    }
}