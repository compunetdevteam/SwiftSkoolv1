using SwiftSkoolv1.Domain;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class SessionsController : BaseController
    {


        // GET: Sessions
        public async Task<ActionResult> Index()
        {
            return View(await Db.Sessions.AsNoTracking().ToListAsync());
        }
        public async Task<ActionResult> GetIndex()
        {
            #region Server Side filtering
            //Get parameter for sorting from grid table
            // get Start (paging start index) and length (page size for paging)
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns values when we click on Header Name of column
            //getting column name
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            //Soring direction(either desending or ascending)
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int totalRecords = 0;

            var v = Db.Sessions.AsNoTracking().Select(x => new { x.SessionId, x.SessionName, x.ActiveSession }).ToList();


            if (!string.IsNullOrEmpty(search))
            {
                //v = v.OrderBy(sortColumn + " " + sortColumnDir);
                v = Db.Sessions.Where(x => x.SessionName.Equals(search))
                    .Select(s => new { s.SessionId, s.SessionName, s.ActiveSession }).ToList();
            }
            totalRecords = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
            #endregion
            // return Json(new { data = await Db.Sessions.AsNoTracking().ToListAsync() }, JsonRequestBehavior.AllowGet);
        }

        public async Task<PartialViewResult> Save(int id)
        {
            var session = await Db.Sessions.FindAsync(id);
            return PartialView(session);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(Session session)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (session.SessionId > 0)
                {
                    Db.Entry(session).State = EntityState.Modified;
                    message = "Session Updated Successfully...";
                }
                else
                {

                    Db.Sessions.Add(session);
                    message = "Session Created Successfully...";

                }
                await Db.SaveChangesAsync();
                status = true;
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }



        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = await Db.Sessions.FindAsync(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // GET: Sessions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SessionId,SessionName,ActiveSession")] Session session)
        {
            if (ModelState.IsValid)
            {
                Db.Sessions.Add(session);
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Session Created Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }

            return View(session);
        }

        // GET: Sessions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session session = await Db.Sessions.FindAsync(id);
            if (session == null)
            {
                return HttpNotFound();
            }
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SessionId,SessionName,ActiveSession")] Session session)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(session).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Session Updated Successfully.";
                TempData["Title"] = "Success.";
                return RedirectToAction("Index");
            }
            return View(session);
        }

        //// GET: Sessions/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Session session = await Db.Sessions.FindAsync(id);
        //    if (session == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(session);
        //}

        //// POST: Sessions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Session session = await Db.Sessions.FindAsync(id);
        //    if (session != null) Db.Sessions.Remove(session);
        //    await Db.SaveChangesAsync();
        //    TempData["UserMessage"] = "Session Deleted Successfully.";
        //    TempData["Title"] = "Error.";
        //    return RedirectToAction("Index");
        //}

        public async Task<PartialViewResult> Delete(int id)
        {
            var session = await Db.Sessions.FindAsync(id);
            return PartialView(session);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            bool status = false;
            string message = string.Empty;
            var session = await Db.Sessions.FindAsync(id);
            if (session != null)
            {
                Db.Sessions.Remove(session);
                await Db.SaveChangesAsync();
                status = true;
                message = "Session Deleted Successfully...";
            }

            return new JsonResult { Data = new { status = status, message = message } };
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
