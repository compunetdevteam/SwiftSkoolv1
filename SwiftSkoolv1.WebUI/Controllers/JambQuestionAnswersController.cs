using OfficeOpenXml;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.Domain.JambPractice;
using SwiftSkoolv1.WebUI.Services;
using SwiftSkoolv1.WebUI.ViewModels.JambExam;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class JambQuestionAnswersController : BaseController
    {
        public ActionResult GetData()
        {
            // dc.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            var data = Db.JambQuestionAnswers.Select(s => new
            {
                s.JambQuestionAnswerId,
                s.JambSubject.SubjectName,
                s.ExamYear,
                s.ExamType,
                s.Question,
                s.Option1,
                s.Option2,
                s.Option3,
                s.Option4
            }).ToList();
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);

        }

        // GET: JambQuestionAnswers
        public async Task<ActionResult> Index()
        {
            var questionAnswers = Db.JambQuestionAnswers.AsNoTracking().Include(q => q.JambSubject);
            return View(await questionAnswers.ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> DownloadTranscript()
        {
            ViewBag.JambSubjectId = new SelectList(Db.Subjects.AsNoTracking(), "JambSubjectId", "SubjectName");
            return View();
        }

        public async Task DownloadQuestionTranscript(int JambSubjectId, int ExamTypeId)
        {
            //var facilityList = Db.Communications.AsNoTracking().ToList();
            var studentQuestion = await Db.JambStudentQuestions.AsNoTracking().Where(x => x.JambSubjectId.Equals(JambSubjectId)
                                    && ExamTypeId.Equals(ExamTypeId)).OrderBy(o => o.StudentId).ToListAsync();
            char c1 = 'A';
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Report");


            worksheet.Cells[$"A1"].Value = "Full Name";
            worksheet.Cells[$"B1"].Value = "Reg No";
            worksheet.Cells[$"C1"].Value = "JambSubject Name";
            worksheet.Cells[$"D1"].Value = "JambSubject Id";

            worksheet.Cells[$"G1"].Value = "Question";
            worksheet.Cells[$"H1"].Value = "Question No";
            worksheet.Cells[$"I1"].Value = "Question Hint";
            worksheet.Cells[$"J1"].Value = "Option 1";
            worksheet.Cells[$"K1"].Value = "Option 2";
            worksheet.Cells[$"L1"].Value = "Option 3";
            worksheet.Cells[$"M1"].Value = "Option 4";
            worksheet.Cells[$"N1"].Value = "Answer";
            worksheet.Cells[$"O1"].Value = "Selected Answer";
            worksheet.Cells[$"P1"].Value = "Filled Answer";
            worksheet.Cells[$"Q1"].Value = "Is Correct";
            worksheet.Cells[$"R1"].Value = "Check 1";
            worksheet.Cells[$"S1"].Value = "Check 2";
            worksheet.Cells[$"T1"].Value = "Check 3";
            worksheet.Cells[$"U1"].Value = "Check 4";
            worksheet.Cells[$"V1"].Value = "Fill in the Gap";
            worksheet.Cells[$"W1"].Value = "Multi Choice";
            worksheet.Cells[$"X1"].Value = "Total Question";
            worksheet.Cells[$"Y1"].Value = "Exam Time";


            int rowStart = 2;

            foreach (JambStudentQuestion t in studentQuestion)
            {
                worksheet.Cells[$"A{rowStart}"].Value = t.Student.FullName;
                worksheet.Cells[$"B{rowStart}"].Value = t.StudentId;
                worksheet.Cells[$"C{rowStart}"].Value = t.JambSubject.SubjectName;
                worksheet.Cells[$"D{rowStart}"].Value = t.JambSubjectId;

                worksheet.Cells[$"G{rowStart}"].Value = t.Question;
                worksheet.Cells[$"H{rowStart}"].Value = t.QuestionNumber;
                worksheet.Cells[$"I{rowStart}"].Value = t.QuestionHint;
                worksheet.Cells[$"J{rowStart}"].Value = t.Option1;
                worksheet.Cells[$"K{rowStart}"].Value = t.Option2;
                worksheet.Cells[$"L{rowStart}"].Value = t.Option3;
                worksheet.Cells[$"M{rowStart}"].Value = t.Option4;
                worksheet.Cells[$"N{rowStart}"].Value = t.Answer;
                worksheet.Cells[$"O{rowStart}"].Value = t.SelectedAnswer;
                worksheet.Cells[$"P{rowStart}"].Value = t.FilledAnswer;
                worksheet.Cells[$"Q{rowStart}"].Value = t.IsCorrect;
                worksheet.Cells[$"R{rowStart}"].Value = t.Check1;
                worksheet.Cells[$"S{rowStart}"].Value = t.Check2;
                worksheet.Cells[$"T{rowStart}"].Value = t.Check3;
                worksheet.Cells[$"U{rowStart}"].Value = t.Check4;
                worksheet.Cells[$"V{rowStart}"].Value = t.IsFillInTheGag;
                worksheet.Cells[$"W{rowStart}"].Value = t.IsMultiChoiceAnswer;
                worksheet.Cells[$"X{rowStart}"].Value = t.TotalQuestion;
                worksheet.Cells[$"Y{rowStart}"].Value = t.ExamTime;


                rowStart++;
            }
            // var info = results.FirstOrDefault();
            worksheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + $"PreDegreeExamResult.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();

        }
        // GET: JambQuestionAnswers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var questionAnswer = await Db.JambQuestionAnswers.FindAsync(id);
            if (questionAnswer == null)
            {
                return HttpNotFound();
            }
            return View(questionAnswer);
        }

        // GET: JambQuestionAnswers/Create
        public ActionResult Create()
        {
            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects.AsNoTracking(), "JambSubjectId", "SubjectName");
            var yearCategory = YearCategory();
            var myStudentType = from s in yearCategory
                                select new { ID = s, Name = s.ToString() };

            var examTypeCategory = ExamTypeList();
            var myExamType = from s in examTypeCategory
                             select new { ID = s, Name = s.ToString() };

            ViewBag.ExamType = new SelectList(myExamType, "Name", "Name");
            ViewBag.ExamYear = new SelectList(myStudentType, "Name", "Name");
            return View();
        }

        // POST: JambQuestionAnswers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JambQuestionAnswerVm model)
        {
            if (ModelState.IsValid)
            {
                var questionAnswer = new JambQuestionAnswer
                {
                    JambSubjectId = model.JambSubjectId,
                    Question = model.Question,
                    Option1 = model.Option1,
                    Option2 = model.Option2,
                    Option3 = model.Option3,
                    Option4 = model.Option4,
                    Answer = model.Answer,
                    ExamYear = model.ExamYear,
                    ExamType = model.ExamType,
                    QuestionHint = model.QuestionHint,
                    QuestionType = model.QuestionType,

                };
                Db.JambQuestionAnswers.Add(questionAnswer);
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Question is Added Successfully.";
                TempData["Title"] = "Success.";

                return RedirectToAction("Create");

            }

            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects.AsNoTracking(), "JambSubjectId", "SubjectName", model.JambSubjectId);
            var yearCategory = YearCategory();
            var myStudentType = from s in yearCategory
                                select new { ID = s, Name = s.ToString() };

            var examTypeCategory = ExamTypeList();
            var myExamType = from s in examTypeCategory
                             select new { ID = s, Name = s.ToString() };

            ViewBag.ExamType = new SelectList(myExamType, "Name", "Name", model.ExamType);
            ViewBag.ExamYear = new SelectList(myStudentType, "Name", "Name", model.ExamYear);
            return View(model);
        }

        // GET: JambQuestionAnswers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var questionAnswer = await Db.JambQuestionAnswers.FindAsync(id);
            if (questionAnswer == null)
            {
                return HttpNotFound();
            }
            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects.AsNoTracking(), "JambSubjectId", "SubjectName", questionAnswer.JambSubjectId);
            var yearCategory = YearCategory();
            var myStudentType = from s in yearCategory
                                select new { ID = s, Name = s.ToString() };

            var examTypeCategory = ExamTypeList();
            var myExamType = from s in examTypeCategory
                             select new { ID = s, Name = s.ToString() };

            ViewBag.ExamType = new SelectList(myExamType, "Name", "Name", questionAnswer.ExamType);
            ViewBag.ExamYear = new SelectList(myStudentType, "Name", "Name", questionAnswer.ExamYear);
            var model = new JambQuestionAnswerVm()
            {
                JambQuestionAnswerId = questionAnswer.JambQuestionAnswerId,
                Question = questionAnswer.Question,
                Option1 = questionAnswer.Option1,
                Option2 = questionAnswer.Option2,
                Option3 = questionAnswer.Option3,
                Option4 = questionAnswer.Option4,
                Answer = questionAnswer.Answer,
                QuestionHint = questionAnswer.QuestionHint
            };
            return View(model);
        }

        // POST: JambQuestionAnswers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JambQuestionAnswerVm model)
        {
            if (ModelState.IsValid)
            {
                var questionAnswer = new JambQuestionAnswer
                {
                    JambQuestionAnswerId = model.JambQuestionAnswerId,
                    JambSubjectId = model.JambSubjectId,
                    Question = model.Question,
                    Option1 = model.Option1,
                    Option2 = model.Option2,
                    Option3 = model.Option3,
                    Option4 = model.Option4,
                    Answer = model.Answer,
                    ExamYear = model.ExamYear,
                    ExamType = model.ExamType,
                    QuestionHint = model.QuestionHint,
                    QuestionType = model.QuestionType,

                };
                Db.Entry(questionAnswer).State = EntityState.Modified;
                await Db.SaveChangesAsync();
                TempData["UserMessage"] = "Question is Updated Successfully.";
                TempData["Title"] = "Success.";

                return RedirectToAction("Index");
            }
            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects.AsNoTracking(), "JambSubjectId", "SubjectName", model.JambSubjectId);
            var yearCategory = YearCategory();
            var myStudentType = from s in yearCategory
                                select new { ID = s, Name = s.ToString() };

            var examTypeCategory = ExamTypeList();
            var myExamType = from s in examTypeCategory
                             select new { ID = s, Name = s.ToString() };

            ViewBag.ExamType = new SelectList(myExamType, "Name", "Name", model.ExamType);
            ViewBag.ExamYear = new SelectList(myStudentType, "Name", "Name", model.ExamYear);
            return View(model);
        }

        // GET: JambQuestionAnswers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var questionAnswer = await Db.JambQuestionAnswers.FindAsync(id);
            if (questionAnswer == null)
            {
                return HttpNotFound();
            }
            return View(questionAnswer);
        }

        // POST: JambQuestionAnswers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var questionAnswer = await Db.JambQuestionAnswers.FindAsync(id);
            if (questionAnswer != null) Db.JambQuestionAnswers.Remove(questionAnswer);
            await Db.SaveChangesAsync();
            TempData["UserMessage"] = "Question is Deleted Successfully.";
            TempData["Title"] = "Error.";

            return RedirectToAction("Index");
        }

        public PartialViewResult UploadQuestion()
        {
            return PartialView();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> UploadQuestion(HttpPostedFileBase excelfile)
        {
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please Select a excel file <br/>";
                TempData["UserMessage"] = "Please Select a excel file.";
                TempData["Title"] = "Error.";

                return View("Index");
            }
            HttpPostedFileBase file = Request.Files["excelfile"];
            if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
            {
                string lastrecord = "";
                int recordCount = 0;
                string message = string.Empty;
                string fileContentType = file.ContentType;
                byte[] fileBytes = new byte[file.ContentLength];
                var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                // Read data from excel file
                using (var package = new ExcelPackage(file.InputStream))
                {
                    ExcelValidation myExcel = new ExcelValidation();
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet.First();
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    int requiredField = 10;

                    string validCheck = myExcel.ValidateExcel(noOfRow, workSheet, requiredField);
                    if (!validCheck.Equals("Success"))
                    {
                        //string row = "";
                        //string column = "";
                        string[] ssizes = validCheck.Split(' ');
                        string[] myArray = new string[2];
                        for (int i = 0; i < ssizes.Length; i++)
                        {
                            myArray[i] = ssizes[i];
                            // myArray[i] = ssizes[];
                        }
                        string lineError = $"Line/Row number {myArray[0]}  and column {myArray[1]} is not rightly formatted, Please Check for anomalies ";
                        //ViewBag.LineError = lineError;
                        TempData["UserMessage"] = lineError;
                        TempData["Title"] = "Error.";
                        return View();
                    }
                    for (int row = 2; row <= noOfRow; row++)
                    {
                        var examType = string.Empty;


                        string excelExamType = workSheet.Cells[row, 2].Value.ToString().ToUpper().Trim();
                        string subjectCode = workSheet.Cells[row, 1].Value.ToString().Trim();
                        string examYear = workSheet.Cells[row, 3].Value.ToString().Trim().ToUpper();
                        var subject = await Db.JambSubjects.AsNoTracking()
                            .Where(x => x.SubjectCode.ToUpper().Equals(subjectCode.ToUpper()))
                            .FirstOrDefaultAsync();

                        if (subject == null)
                        {
                            ViewBag.ErrorInfo = "The Subject doesn't Exist Exam Type in the excel doesn't exist";
                            ViewBag.ErrorMessage = "Error.";
                            return View("ErrorException");
                        }
                        if (excelExamType.ToUpper().Trim().Equals("JAMB"))
                        {
                            examType = "JAMB";
                        }
                        else if (excelExamType.ToUpper().Trim().Equals("WAEC"))
                        {
                            examType = "WAEC";
                        }
                        else if (excelExamType.ToUpper().Trim().Equals("NECO"))
                        {
                            examType = "NECO";
                        }
                        else if (excelExamType.ToUpper().Trim().Equals("GCE"))
                        {
                            examType = "GCE";
                        }
                        else
                        {
                            ViewBag.ErrorInfo = "The Exam type in the excel doesn't exist";
                            ViewBag.ErrorMessage = "The Exam type supported is JAMB, WAEC, NECO, GCE in the excel doesn't exist";
                            return View("ErrorException");
                        }
                        try
                        {

                            var questionAnswer = new JambQuestionAnswer
                            {
                                JambSubjectId = subject.JambSubjectId,
                                Question = workSheet.Cells[row, 4].Value.ToString().Trim(),
                                Option1 = workSheet.Cells[row, 5].Value.ToString().Trim(),
                                Option2 = workSheet.Cells[row, 6].Value.ToString().Trim(),
                                Option3 = workSheet.Cells[row, 7].Value.ToString().Trim(),
                                Option4 = workSheet.Cells[row, 8].Value.ToString().Trim(),
                                Answer = workSheet.Cells[row, 9].Value.ToString().Trim(),
                                QuestionType = QuestionType.SingleChoice,
                                ExamYear = examYear,
                                ExamType = examType,
                                QuestionHint = workSheet.Cells[row, 10].Value.ToString().Trim(),

                            };
                            Db.JambQuestionAnswers.Add(questionAnswer);
                            recordCount++;
                            lastrecord = $"The last Updated record has the Course  {subject.SubjectName} and Exam Type is {examType}";

                        }
                        catch (Exception ex)
                        {
                            ViewBag.ErrorInfo = "The Department code in the excel doesn't exist";
                            ViewBag.ErrorMessage = ex.Message;
                            return View("ErrorException");
                        }

                    }
                    await Db.SaveChangesAsync();
                    message = $"You have successfully Uploaded {recordCount} records...  and {lastrecord}";
                    TempData["UserMessage"] = message;
                    TempData["Title"] = "Success.";

                }
                return RedirectToAction("Index", "JambQuestionAnswers");
            }

            ViewBag.Error = $"File type is Incorrect <br/>";
            return View("Index");
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