using SwiftSkoolv1.Domain;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class BooksController : BaseController
    {
        // GET: Books
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetIndex()
        {
            // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            var data = await Db.Books.AsNoTracking().Select(s => new
            {
                s.ClassName,
                s.SubjectName,
                s.Author,
                s.Title,
                s.Edition
            }).ToListAsync();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public async Task<PartialViewResult> Save(int id)
        {
            var book = await Db.Books.FindAsync(id);
            return PartialView(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(BookVm model)
        {
            bool status = false;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                if (model.BookId > 0)
                {
                    var book = await Db.Books.FindAsync(model.BookId);
                    if (book != null)
                    {
                        book.Author = model.Author;
                        book.SubjectName = model.SubjectName;
                        book.ClassName = model.ClassName[0];
                        book.Title = model.Title;
                        book.SchoolId = model.SchoolId;
                        book.BookLocation = model.BookLocation;
                        book.Edition = model.Edition;
                        Db.Entry(book).State = EntityState.Modified;
                        await Db.SaveChangesAsync();
                        message = $"{model.Title} Updated Successfully...";
                        return new JsonResult { Data = new { status = true, message = message } };
                    }
                }
                else
                {
                    int count = 0;
                    foreach (var className in model.ClassName)
                    {
                        var newBook = new Book()
                        {
                            Author = model.Author,
                            SubjectName = model.SubjectName,
                            ClassName = model.ClassName[count],
                            Title = model.Title,
                            SchoolId = model.SchoolId,
                            BookLocation = model.BookLocation,
                            Edition = model.Edition
                        };
                        count = count + 1;
                        Db.Books.Add(newBook);
                    }
                    await Db.SaveChangesAsync();
                    message = $"{model.Title} Added Successfully.";
                    return new JsonResult { Data = new { status = true, message = message } };
                }
            }
            return new JsonResult { Data = new { status = status, message = message } };
            //return View(subject);
        }


        // GET: Books/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await Db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.SubjectName = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectName", "SubjectName");
            ViewBag.ClassName = new MultiSelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookVm model)
        {
            string _FileName = String.Empty;
            if (ModelState.IsValid)
            {
                if (model.File.FileName.ToLower().EndsWith("pdf"))
                {

                    int count = 0;
                    if (model.File.ContentLength > 0)
                    {
                        _FileName = Path.GetFileName(model.File.FileName);
                        string _path = HostingEnvironment.MapPath("~/UploadedFiles/") + _FileName;
                        model.BookLocation = _path;
                        var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/UploadedFiles/"));
                        if (directory.Exists == false)
                        {
                            directory.Create();
                        }
                        model.File.SaveAs(_path);
                    }
                    foreach (var className in model.ClassName)
                    {
                        var newBook = new Book
                        {
                            Author = model.Author,
                            SubjectName = model.SubjectName,
                            ClassName = className,
                            Title = model.Title,
                            SchoolId = model.SchoolId,
                            BookLocation = model.BookLocation,
                            Edition = model.Edition
                        };
                        count = count + 1;
                        Db.Books.Add(newBook);
                    }
                    //Db.Books.Add(newBook);
                    await Db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.SubjectName = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectName", "SubjectName", model.SubjectName);
                ViewBag.ClassName = new MultiSelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName", model.ClassName);
                ViewBag.Message = "File upload format is not supported, Only PDF files are supported";
                return View(model);
            }
            ViewBag.SubjectName = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectName", "SubjectName", model.SubjectName);
            ViewBag.ClassName = new MultiSelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName", model.ClassName);
            return View(model);
        }

        // GET: Books/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await Db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            var model = new BookVm()
            {
                Author = book.Author,
                Edition = book.Edition,
                Title = book.Title
            };
            ViewBag.SubjectName = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectName", "SubjectName", book.SubjectName);
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName", book.ClassName);
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BookVm model)
        {
            if (ModelState.IsValid)
            {
                var book = await Db.Books.FindAsync(model.BookId);
                if (book != null)
                {
                    book.Author = model.Author;
                    book.SubjectName = model.SubjectName;
                    book.ClassName = model.ClassName[0];
                    book.Title = model.Title;
                    book.SchoolId = model.SchoolId;
                    book.BookLocation = model.BookLocation;
                    book.Edition = model.Edition;
                }
                Db.Entry(book).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectName = new SelectList(await _query.SubjectListAsync(userSchool), "SubjectName", "SubjectName", model.SubjectName);
            ViewBag.ClassName = new SelectList(await _query.ClassListAsync(userSchool), "FullClassName", "FullClassName", model.ClassName[0]);
            return View(model);
        }

        // GET: Books/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await Db.Books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Book book = await Db.Books.FindAsync(id);
            Db.Books.Remove(book);
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
