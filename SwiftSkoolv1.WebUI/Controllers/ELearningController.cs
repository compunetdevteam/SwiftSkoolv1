using Microsoft.AspNet.Identity;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.Domain.ClassRoom;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    [Authorize]

    public class ELearningController : BaseController
    {

        // GET: ELearning
        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        public ActionResult StudentsIndex(string message)
        {
            ViewBag.Message = message;
            return View();
        }


        public async Task<ActionResult> MyCourses()
        {
            var courseList = new List<Subject>();
            //var courses = await Db.Courses.AsNoTracking().Include(c => c.Department)
            //    .Include(c => c.Level).Include(c => c.Programme).Include(c => c.Semester).ToListAsync();
            if (User.IsInRole("Teacher"))
            {
                var id = User.Identity.GetUserId();
                var staff = await Db.Staffs.FindAsync(id);
                var assignedCourse = await Db.AssignSubjectTeachers.Include(i => i.Subject).AsNoTracking()
                                        .Where(x => x.StaffName.Equals(staff.Username))
                                        //.Where(x => x.SchoolId.Equals(userSchool))
                                        .Select(s => s.Subject).ToListAsync();
                foreach (var courseId in assignedCourse)
                {
                    courseList.Add(courseId);
                }
            }
            if (User.IsInRole("Student"))
            {
                var id = User.Identity.GetUserId();
                string currentterm = _query.CurrentTerm();
                var currentClass = _query.GetMyClass(userSchool, id);
                var student = await Db.Students.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool) && x.StudentId.Equals(id)).FirstOrDefaultAsync();
                courseList = _query.GetStudentSubject(id, userSchool, currentClass, currentterm);
            }
            if (User.IsInRole("Admin"))
            {
                var assignedCourse = await Db.Subjects.AsNoTracking()
                    .Where(x => x.SchoolId.Equals(userSchool)).ToListAsync();
                foreach (var courseId in assignedCourse)
                {
                    courseList.Add(courseId);
                }
            }
            var data = courseList.Select(s => new
            {
                s.SubjectId,
                s.SubjectCode,
                s.SubjectName
            }).ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public async Task<PartialViewResult> CourseDetails(int id)
        {
            var course = await Db.Subjects.AsNoTracking().Include(i => i.Modules)
                .Where(x => x.SubjectId.Equals(id)).FirstOrDefaultAsync();
            return PartialView(course);
        }


        public async Task<PartialViewResult> SaveModule(int id)
        {
            var course = await Db.Subjects.AsNoTracking().Where(x => x.SubjectId.Equals(id))
                     .ToListAsync();
            ViewBag.SubjectId = new SelectList(course, "SubjectId", "SubjectName");
            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
            ViewBag.TermId = new SelectList(_query.TermList(), "TermId", "TermName");
            return PartialView();
        }
        public async Task<PartialViewResult> EditModule(int id)
        {
            var module = await Db.Modules.FindAsync(id);

            var course = await Db.Subjects.AsNoTracking().Where(x => x.SubjectId.Equals(module.SubjectId))
                                .ToListAsync();
            ViewBag.SubjectId = new SelectList(course, "SubjectId", "SubjectName");
            ViewBag.ClassId = new SelectList(await _query.ClassListAsync(userSchool), "ClassId", "FullClassName");
            ViewBag.TermId = new SelectList(_query.TermList(), "TermId", "TermName");
            return PartialView(module);
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveModule(Module model)
        {
            if (ModelState.IsValid)
            {
                if (model.ModuleId > 0)
                {
                    var module = await Db.Modules.FindAsync(model.ModuleId);
                    if (module != null)
                    {
                        module.SubjectId = model.SubjectId;
                        module.ModuleName = model.ModuleName;
                        module.ModuleDescription = model.ModuleDescription;
                        module.ExpectedTime = model.ExpectedTime;
                        module.TermId = model.TermId;
                        model.ClassId = model.ClassId;
                        module.SchoolId = userSchool;
                        Db.Entry(module).State = EntityState.Modified;
                    }
                    model.SchoolId = userSchool;
                    await Db.SaveChangesAsync();
                    var message = $"{model.ModuleName} Updated Successfully";
                    return new JsonResult { Data = new { status = true, message = message, id = model.SubjectId } };

                }
                else
                {
                    Db.Modules.Add(model);
                    await Db.SaveChangesAsync();
                    var message = $"{model.ModuleName} Added Successfully";
                    return new JsonResult { Data = new { status = true, message = message, id = model.SubjectId } };
                }

            }
            return new JsonResult { Data = new { status = false, message = "Model Not Correct", id = model.SubjectId } };
        }

        public async Task<PartialViewResult> SaveTopic(int id)
        {
            var module = await Db.Modules.AsNoTracking().Where(x => x.ModuleId.Equals(id)).ToListAsync();
            ViewBag.ModuleId = new SelectList(module, "ModuleId", "ModuleName");
            return PartialView();
        }
        public async Task<PartialViewResult> EditTopic(int id)
        {
            var topic = await Db.Topics.AsNoTracking().Where(x => x.TopicId.Equals(id)).FirstOrDefaultAsync();
            if (topic != null)
            {
                var modules = await Db.Modules.AsNoTracking().Where(x => x.ModuleId.Equals(topic.ModuleId)).ToListAsync();
                ViewBag.ModuleId = new SelectList(modules, "ModuleId", "ModuleName");
            }
            return PartialView(topic);
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTopic(Topic model)
        {
            if (ModelState.IsValid)
            {
                if (model.TopicId > 0)
                {
                    var topic = await Db.Topics.AsNoTracking().Where(x => x.TopicId.Equals(model.TopicId)).FirstOrDefaultAsync();
                    topic.TopicName = model.TopicName;
                    topic.ExpectedTime = model.ExpectedTime;
                    topic.ModuleId = model.ModuleId;
                    topic.SchoolId = userSchool;

                    Db.Entry(topic).State = EntityState.Modified;
                    await Db.SaveChangesAsync();
                    return new JsonResult { Data = new { status = true, message = $"{model.TopicName} is updated successfully", id = model.ModuleId } };
                }
                model.SchoolId = userSchool;
                Db.Topics.Add(model);
                await Db.SaveChangesAsync();
                var message = $"{model.TopicName} Added Successfully";
                return new JsonResult { Data = new { status = true, message = message, id = model.ModuleId } };
            }
            return new JsonResult { Data = new { status = false, message = "Model Not Correct" } };
        }

        public async Task<PartialViewResult> DetailTopic(int id)
        {
            var topics = await Db.Modules.Include(i => i.Topics)
                            .AsNoTracking().Where(x => x.ModuleId.Equals(id)).FirstOrDefaultAsync();
            return PartialView(topics);
        }


        public async Task<PartialViewResult> SaveTopicContent(int id)
        {
            var topic = await Db.Topics.AsNoTracking().Where(x => x.TopicId.Equals(id)).ToListAsync();
            ViewBag.TopicId = new SelectList(topic, "TopicId", "TopicName");
            return PartialView();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveTopicContent(TopicMaterial model)
        {
            if (ModelState.IsValid)
            {
                string _FileName = String.Empty;
                if (model.File.ContentLength > 0)
                {
                    _FileName = Path.GetFileName(model.File.FileName);
                    string _path = HostingEnvironment.MapPath("~/MaterialUpload/") + _FileName;
                    model.FileLocation = _path;
                    var directory = new DirectoryInfo(HostingEnvironment.MapPath("~/MaterialUpload/"));
                    if (directory.Exists == false)
                    {
                        directory.Create();
                    }
                    model.File.SaveAs(_path);
                }
                model.FileLocation = _FileName;
                if (model.TopicMaterialId > 0)
                {
                    var topicMaterial = await Db.TopicMaterials.AsNoTracking().Where(x => x.TopicId.Equals(model.TopicMaterialId)).FirstOrDefaultAsync();
                    topicMaterial.Author = model.Author;
                    topicMaterial.Description = model.Description;
                    topicMaterial.Name = model.Name;
                    topicMaterial.NoteBody = model.NoteBody;
                    topicMaterial.FileLocation = model.FileLocation;
                    topicMaterial.TopicId = model.TopicId;
                    topicMaterial.SchoolId = userSchool;
                    Db.Entry(topicMaterial).State = EntityState.Modified;
                    await Db.SaveChangesAsync();
                    return new JsonResult { Data = new { status = true, message = $"{model.Name} is updated successfully", id = model.TopicId } };
                }

                model.SchoolId = userSchool;
                Db.TopicMaterials.Add(model);
                await Db.SaveChangesAsync();
                var message = $"{model.Name} Added Successfully";
                //return new JsonResult { Data = new { status = true, message = message, id = model.TopicId } };
                return RedirectToAction("Index", new { message = message });
            }
            return new JsonResult { Data = new { status = false, message = "Model Not Correct" } };
        }

        public async Task<PartialViewResult> ViewTopic(int id)
        {
            var topic = await Db.Topics.Include(i => i.TopicMaterials).Include(i => i.Module).AsNoTracking()
                                .Where(x => x.TopicId.Equals(id)).FirstOrDefaultAsync();
            return PartialView(topic);
        }

        public async Task<PartialViewResult> DetailTopicContent(int id)
        {
            var content = await Db.TopicMaterials.Include(i => i.Topic).Include(i => i.Topic.Module).AsNoTracking()
                            .Where(x => x.TopicMaterialId.Equals(id)).FirstOrDefaultAsync();
            return PartialView(content);
        }

        public async Task<PartialViewResult> SaveLesssonNote(int id)
        {
            var topic = await Db.Topics.AsNoTracking().Where(x => x.TopicId.Equals(id)).ToListAsync();
            ViewBag.TopicId = new SelectList(topic, "TopicId", "TopicName");
            return PartialView();
        }

        // POST: Modules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveLesssonNote(LessonNote model)
        {
            if (ModelState.IsValid)
            {

                if (model.TopicId > 0)
                {
                    var lessonNote = await Db.LessonNotes.AsNoTracking().Where(x => x.TopicId.Equals(model.TopicId)).FirstOrDefaultAsync();
                    lessonNote.Note = model.Note;
                    lessonNote.SchoolId = userSchool;
                    Db.Entry(lessonNote).State = EntityState.Modified;
                    await Db.SaveChangesAsync();
                    return new JsonResult { Data = new { status = true, message = $"{model.TopicId} is updated successfully", id = model.TopicId } };
                }

                model.SchoolId = userSchool;
                Db.LessonNotes.Add(model);
                await Db.SaveChangesAsync();
                var message = $"{model.TopicId} Added Successfully";
                return new JsonResult { Data = new { status = true, message = message, id = model.TopicId } };
                // return RedirectToAction("Index", new { message = message });
            }
            return new JsonResult { Data = new { status = false, message = "Model Not Correct" } };
        }


    }
}