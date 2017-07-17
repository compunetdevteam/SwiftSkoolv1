﻿using SwiftSkoolv1.Domain;
using System;
using System.Data.Entity;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class HomePageSetUpsController : BaseController
    {


        // GET: HomePageSetUps
        public async Task<ActionResult> Index()
        {
            return View(await Db.HomePageSetUps.ToListAsync());
        }

        // GET: HomePageSetUps/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomePageSetUp homePageSetUp = await Db.HomePageSetUps.FindAsync(id);
            if (homePageSetUp == null)
            {
                return HttpNotFound();
            }
            return View(homePageSetUp);
        }

        // GET: HomePageSetUps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomePageSetUps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HomePageSetUp homePageSetUp)
        {
            if (ModelState.IsValid)
            {
                string _FileName = String.Empty;
                try
                {
                    if (homePageSetUp.File?.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(homePageSetUp.File.FileName);
                        string _path = HostingEnvironment.MapPath("~/Content/Images/") + _FileName;
                        var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/Content/Images/"));
                        if (directory.Exists == false)
                        {
                            directory.Create();
                        }
                        homePageSetUp.File.SaveAs(_path);
                    }
                }
                catch
                {
                    ViewBag.Message = "File upload failed!!";
                    ViewBag.CourseId = new SelectList(Db.HomePageSetUps, "CourseId", "CourseCode");
                    return View(homePageSetUp);
                }
                homePageSetUp.FileLocation = _FileName;
                Db.HomePageSetUps.Add(homePageSetUp);
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(homePageSetUp);
        }

        // GET: HomePageSetUps/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomePageSetUp homePageSetUp = await Db.HomePageSetUps.FindAsync(id);
            if (homePageSetUp == null)
            {
                return HttpNotFound();
            }
            return View(homePageSetUp);
        }

        // POST: HomePageSetUps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "HomePagesetUpId,Title,DescriptiveText,FileLocation")] HomePageSetUp homePageSetUp)
        {
            if (ModelState.IsValid)
            {
                Db.Entry(homePageSetUp).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(homePageSetUp);
        }

        // GET: HomePageSetUps/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HomePageSetUp homePageSetUp = await Db.HomePageSetUps.FindAsync(id);
            if (homePageSetUp == null)
            {
                return HttpNotFound();
            }
            return View(homePageSetUp);
        }

        // POST: HomePageSetUps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            HomePageSetUp homePageSetUp = await Db.HomePageSetUps.FindAsync(id);
            Db.HomePageSetUps.Remove(homePageSetUp);
            await Db.SaveChangesAsync();
            return RedirectToAction("Index");
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
