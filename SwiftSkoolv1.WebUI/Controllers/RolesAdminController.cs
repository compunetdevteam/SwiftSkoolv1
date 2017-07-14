using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SwiftSkoolv1.WebUI.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class RolesAdminController : BaseController
    {

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // GET: RolesAdmin
        public ActionResult Index()
        {
            var roles = Db.Roles.ToList();
            return View(roles);
        }

        // GET: RolesAdmin/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: RolesAdmin/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: RolesAdmin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                Db.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                Db.SaveChanges();
                //ViewBag.ResultMessage = "Role created successfully !";

                // TempData["UserMessage"] = new { CssClassName = "alert-sucess", Title = "Success!", Message = "Roles Added Successfully." };
                TempData["UserMessage"] = "Roles Added Successfully.";
                TempData["Title"] = "Success.";

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["UserMessage"] = "Role is not Added, Please try again later.";
                TempData["Title"] = "Error.";
                return View();
            }
        }
        // GET: RolesAdmin/Edit/5
        public ActionResult Edit(string roleName)
        {
            var thisRole = Db.Roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
            return View(thisRole);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                Db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                Db.SaveChanges();
                TempData["UserMessage"] = "Role Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["UserMessage"] = "Update is Unsuccessful, Please try again later.";
                TempData["Title"] = "Error.";
                return View();
            }
        }

        public ActionResult ManageUserRoles()
        {
            // prepopulat roles for the view dropdown
            //ViewBag.Roles = new SelectList(Db.Roles, "Name", "Name");
            var role = Db.Roles.SingleOrDefault(m => m.Name == "Teacher");
            var staff = UserManager.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id)).ToList();

            ViewBag.Roles = new SelectList(Db.Roles, "Name", "Name");
            ViewBag.Username = new SelectList(staff, "Username", "Username");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user = Db.Users.FirstOrDefault(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));
            //var account = new AccountController();
            UserManager.AddToRole(user.Id, RoleName);

            ViewBag.ResultMessage = "Role created successfully !";

            // prepopulat roles for the view dropdown
            var role = Db.Roles.SingleOrDefault(m => m.Name == "Teacher" || m.Name == "Form-Teacher");
            //var usersInRole = Db.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id));
            var staff = UserManager.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id)).ToList();

            ViewBag.Roles = new SelectList(Db.Roles, "Name", "Name");
            ViewBag.Username = new SelectList(staff, "Username", "Username");
            return View("ManageUserRoles");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string Username)
        {
            if (!string.IsNullOrWhiteSpace(Username))
            {
                var user = Db.Users.FirstOrDefault(c => c.UserName.Equals(Username));
                //ApplicationUser user = Db.Users.FirstOrDefault(u => u.UserName.Equals(Username.Trim(), StringComparison.CurrentCultureIgnoreCase));
                if (user == null)
                {
                    var mlist = Db.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                    ViewBag.Roles = new SelectList(Db.Roles, "Name", "Name");
                    ViewBag.Username = new SelectList(Db.Staffs, "Username", "Username");
                    ViewBag.ClassName = new SelectList(Db.Classes, "FullClassName", "FullClassName");
                    TempData["UserMessage"] = "Couldn't Find User.";
                    TempData["Title"] = "Error.";
                    return View("ManageUserRoles");
                }
                //var account = new AccountController();

                ViewBag.RolesForThisUser = UserManager.GetRoles(user.Id);
                ViewBag.Username = new SelectList(Db.Users, "Username", "Username");


                ViewBag.Roles = new SelectList(Db.Roles, "Name", "Name");
            }

            return View("ManageUserRoles");
        }

        // GET: RolesAdmin/Delete/5
        public ActionResult Delete(string RoleName)
        {
            var thisRole = Db.Roles.FirstOrDefault(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase));
            Db.Roles.Remove(thisRole);
            Db.SaveChanges();
            TempData["UserMessage"] = "Role deleted Successfully.";
            TempData["Title"] = "Delete.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            //var account = new AccountController();
            ApplicationUser user = Db.Users.FirstOrDefault(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));

            if (user != null && UserManager.IsInRole(user.Id, RoleName))
            {
                UserManager.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultMessage = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.ResultMessage = "This user doesn't belong to selected role.";
            }


            ViewBag.Roles = new SelectList(Db.Roles, "Name", "Name");
            ViewBag.Username = new SelectList(Db.Users, "Username", "Username");
            return View("ManageUserRoles");
        }
    }
}
