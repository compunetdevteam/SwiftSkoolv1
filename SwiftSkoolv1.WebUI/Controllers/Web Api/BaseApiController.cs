using SwiftSkoolv1.WebUI.BusinessLogic;
using SwiftSkoolv1.WebUI.Models;
using SwiftSkoolv1.WebUI.ViewModels;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

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