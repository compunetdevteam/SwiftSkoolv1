using Microsoft.AspNet.Identity;
using SwiftSkoolv1.Domain.JambPractice;
using SwiftSkoolv1.WebUI.Services;
using SwiftSkoolv1.WebUI.ViewModels.JambExam;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class JambExamController : BaseController
    {

        public async Task<ActionResult> ShowNumbers()
        {
            var studentName = User.Identity.GetUserName();
            var questionNumber = await Db.JambStudentQuestions.AsNoTracking().Where(x => x.StudentId.Equals(studentName))
                .OrderBy(o => o.QuestionNumber).ToListAsync();
            return View(questionNumber);
        }
        public PartialViewResult Menu(string studentId, int jambsubjectId)
        {
            var questionNumber = Db.JambStudentQuestions.AsNoTracking().Where(x => x.StudentId.Equals(studentId)
                            && x.JambSubjectId.Equals(jambsubjectId)).OrderBy(o => o.QuestionNumber);

            return PartialView(questionNumber);
        }

        // GET: JambExam
        public ActionResult SelectSubject()
        {
            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects.AsNoTracking(), "JambSubjectId", "SubjectName");
            var yearCategory = YearCategory();
            var myStudentType = from s in yearCategory
                                select new { ID = s, Name = s.ToString() };


            ViewBag.ExamYear = new SelectList(myStudentType, "Name", "Name");
            Session["Rem_Time"] = null;
            ViewBag.Time = Session["Rem_Time"];
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SelectSubject(JambSelectSubjectVm model)
        {
            if (ModelState.IsValid)
            {
                string studentId = User.Identity.GetUserId();

                var questionExist = await Db.JambStudentQuestions.Where(x => x.StudentId.Equals(studentId)
                                            && x.JambSubjectId.Equals(model.JambSubjectId)
                                            && x.ExamYear.Equals(model.ExamYear)).ToListAsync();

                if (questionExist.Count >= 1)
                {
                    //return RedirectToAction("Exam", new
                    //{
                    //    questionNo = 1,
                    //    subjectId = model.JambSubjectId,
                    //    studentId = studentId
                    //});
                    Db.JambStudentQuestions.RemoveRange(questionExist);
                    await Db.SaveChangesAsync();
                }

                //var r = new Random();

                var myquestion = await Db.JambQuestionAnswers.Where(x => x.JambSubjectId.Equals(model.JambSubjectId)
                                                                         && x.ExamYear.Equals(model.ExamYear))
                    .Take(model.TotalQuestion).ToListAsync();


                int count = 1;
                if (myquestion.Count > 0)
                {
                    foreach (var question in myquestion)
                    {
                        var jambStudentQuestion = new JambStudentQuestion()
                        {
                            StudentId = studentId,
                            JambSubjectId = question.JambSubjectId,
                            Question = question.Question,
                            Option1 = question.Option1,
                            Option2 = question.Option2,
                            Option3 = question.Option3,
                            Option4 = question.Option4,
                            FilledAnswer = String.Empty,
                            Answer = question.Answer,
                            QuestionHint = question.QuestionHint,
                            IsFillInTheGag = question.IsFillInTheGag,
                            IsMultiChoiceAnswer = question.IsMultiChoiceAnswer,
                            QuestionNumber = count,
                            TotalQuestion = model.TotalQuestion,
                            ExamTime = model.ExamTime,
                            SchoolId = userSchool
                        };
                        Db.JambStudentQuestions.Add(jambStudentQuestion);
                        count++;

                    }
                    await Db.SaveChangesAsync();
                    return RedirectToAction("Exam", new
                    {
                        questionNo = 1,
                        subjectId = model.JambSubjectId,
                        studentId = studentId,

                    });
                }
                ViewBag.Message = "There is no Question for the Selected Year";
            }

            ViewBag.JambSubjectId = new SelectList(Db.JambSubjects.AsNoTracking(), "JambSubjectId", "SubjectName");
            var yearCategory = YearCategory();
            var myStudentType = from s in yearCategory
                                select new { ID = s, Name = s.ToString() };

            ViewBag.ExamYear = new SelectList(myStudentType, "Name", "Name");

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Exam(int questionNo, int subjectId, string studentId)
        {
            int myno = questionNo;
            var question = Db.JambStudentQuestions.FirstOrDefault(s => s.StudentId.Equals(studentId)
                                    && s.QuestionNumber.Equals(myno));
            if (question != null)
            {
                if (Session["Rem_Time"] == null)
                {
                    int time = question.ExamTime + 60;
                    Session["Rem_Time"] = DateTime.Now.AddMinutes(time).ToString("MM-dd-yyyy h:mm:ss tt");
                }
                //Session["Rem_Time"] = DateTime.Now.AddMinutes(2).ToString("dd-MM-yyyy h:mm:ss tt");
                // Session["Rem_Time"] = DateTime.Now.AddMinutes(2).ToString("MM-dd-yyyy h:mm:ss tt");
                ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
                ViewBag.Rem_Time = Session["Rem_Time"];
                ViewBag.Course = await Db.JambSubjects.AsNoTracking().Where(x => x.JambSubjectId.Equals(subjectId))
                                        .Select(c => c.SubjectName).FirstOrDefaultAsync();
            }
            ViewBag.JambSubjectId = question.JambSubjectId;
            ViewBag.StudentId = question.StudentId;
            return View(question);
        }


        [HttpPost]
        [ValidateInput(false)]
        [MultipleButton(Name = "action", Argument = "Next")]
        public async Task<ActionResult> Next(JambDisplayQuestionVm model, string fiiledAnswer,
                    string Check1, string Check2, string Check3, string Check4)
        {
            var studentId = model.StudentId;
            int questionId = model.QuestionNo;
            var questionType = CheckQuestionType(model);
            if (questionType != null)
            {

                if (questionType.IsFillInTheGag)
                {
                    if (!String.IsNullOrEmpty(fiiledAnswer))
                    {
                        await SaveAnswer(model, studentId, questionId, fiiledAnswer);
                        return RedirectToAction("Exam", "JambExam",
                            new
                            {
                                questionNo = ++questionId,
                                subjectId = model.JambSubjectId,
                                studentId = model.StudentId,

                            });
                    }
                }
                else if (questionType.IsMultiChoiceAnswer)
                {
                    StringBuilder builder = new StringBuilder();

                    if (model.Check1.Equals(true))
                    {
                        builder.Append("A");
                    }
                    if (model.Check2.Equals(true))
                    {
                        builder.Append("B");
                    }
                    if (model.Check3.Equals(true))
                    {
                        builder.Append("C");
                    }
                    if (model.Check4.Equals(true))
                    {
                        builder.Append("D");
                    }
                    string value = builder.ToString();

                    string checkedAnswer = SortStringAlphabetically(builder.ToString());

                    await SaveMultiChoiceAnswer(model, checkedAnswer);
                }
                else
                {
                    if (!String.IsNullOrEmpty(model.SelectedAnswer))
                    {
                        string answer = CheckAnswerForSingleChoice(model);
                        await SaveAnswer(model, studentId, questionId, answer);
                        return RedirectToAction("Exam", "JambExam",
                            new
                            {
                                questionNo = ++questionId,
                                subjectId = model.JambSubjectId,
                                studentId = model.StudentId
                            });
                    }
                }
            }

            ViewBag.SubjectName = new SelectList(Db.JambSubjects, "JambSubjectId", "SubjectName");
            return RedirectToAction("Exam", new { questionNo = ++questionId, subjectId = model.JambSubjectId, studentId = model.StudentId });
        }




        [HttpPost]
        [ValidateInput(false)]
        [MultipleButton(Name = "action", Argument = "Previous")]
        public async Task<ActionResult> Previous(JambDisplayQuestionVm model, string fiiledAnswer,
                    string Check1, string Check2, string Check3, string Check4)
        {
            var studentId = model.StudentId;
            int questionId = model.QuestionNo;
            var questionType = CheckQuestionType(model);
            if (questionType != null)
            {
                if (questionType.IsFillInTheGag)
                {
                    if (!String.IsNullOrEmpty(fiiledAnswer))
                    {
                        await SaveAnswer(model, studentId, questionId, fiiledAnswer);
                        return RedirectToAction("Exam", "JambExam", new
                        {
                            questionNo = ++questionId,
                            subjectId = model.JambSubjectId,
                            studentId = model.StudentId,

                        });
                    }
                }
                else if (questionType.IsMultiChoiceAnswer)
                {
                    StringBuilder builder = new StringBuilder();

                    if (model.Check1.Equals(true))
                    {
                        builder.Append("A");
                    }
                    if (model.Check2.Equals(true))
                    {
                        builder.Append("B");
                    }
                    if (model.Check3.Equals(true))
                    {
                        builder.Append("C");
                    }
                    if (model.Check4.Equals(true))
                    {
                        builder.Append("D");
                    }
                    string value = builder.ToString();

                    string checkedAnswer = SortStringAlphabetically(builder.ToString());

                    await SaveMultiChoiceAnswer(model, checkedAnswer);
                }
                else
                {
                    if (!String.IsNullOrEmpty(model.SelectedAnswer))
                    {
                        string answer = CheckAnswerForSingleChoice(model);
                        await SaveAnswer(model, studentId, questionId, answer);
                        return RedirectToAction("Exam", "JambExam",
                            new
                            {
                                questionNo = --questionId,
                                subjectId = model.JambSubjectId,
                                studentId = model.StudentId
                            });
                    }
                }
            }

            ViewBag.SubjectName = new SelectList(Db.JambSubjects, "JambSubjectId", "SubjectName");
            return RedirectToAction("Exam", new { questionNo = --questionId, subjectId = model.JambSubjectId, studentId = model.StudentId });
        }


        [HttpPost]
        [ValidateInput(false)]
        [MultipleButton(Name = "action", Argument = "SubmitExam")]
        public async Task<ActionResult> SubmitExam(JambDisplayQuestionVm model, string fiiledAnswer,
                    string Check1, string Check2, string Check3, string Check4)
        {
            double scoreCount = 0;

            var studentId = model.StudentId;
            int questionId = model.QuestionNo;

            var questionType = CheckQuestionType(model);
            #region Submit Answer
            if (questionType != null)
            {
                if (questionType.IsFillInTheGag)
                {
                    if (!String.IsNullOrEmpty(fiiledAnswer))
                    {
                        await SaveAnswer(model, studentId, questionId, fiiledAnswer);
                        scoreCount = Db.JambStudentQuestions.Count(x => x.IsCorrect.Equals(true));
                        return RedirectToAction("ExamIndex", "JambExam",
                            new { score = scoreCount, subjectId = model.JambSubjectId, studentId = model.StudentId });
                    }
                }
                else if (questionType.IsMultiChoiceAnswer)
                {
                    StringBuilder builder = new StringBuilder();

                    if (model.Check1.Equals(true))
                    {
                        builder.Append("A");
                    }
                    if (model.Check2.Equals(true))
                    {
                        builder.Append("B");
                    }
                    if (model.Check3.Equals(true))
                    {
                        builder.Append("C");
                    }
                    if (model.Check4.Equals(true))
                    {
                        builder.Append("D");
                    }
                    string value = builder.ToString();

                    string checkedAnswer = SortStringAlphabetically(builder.ToString());

                    await SaveMultiChoiceAnswer(model, checkedAnswer);
                }
                else
                {
                    if (!String.IsNullOrEmpty(model.SelectedAnswer))
                    {
                        string answer = CheckAnswerForSingleChoice(model);
                        await SaveAnswer(model, studentId, questionId, answer);
                        scoreCount = Db.JambStudentQuestions.Count(x => x.IsCorrect.Equals(true));
                    }
                }

            }
            #endregion

            scoreCount = await Db.JambStudentQuestions.Where(x => x.StudentId.Equals(model.StudentId)
                                            && x.JambSubjectId.Equals(model.JambSubjectId))
                                            .CountAsync(c => c.IsCorrect.Equals(true));
            var studentdetails = Db.JambStudentQuestions.FirstOrDefault(x => x.StudentId.Equals(model.StudentId)
                                            && x.JambSubjectId.Equals(model.JambSubjectId));

            if (studentdetails != null)
            {
                await ProcessResult(model, studentdetails, scoreCount);

            }

            return RedirectToAction("Index", "JambExamLogs", new
            {
                studentId = model.StudentId,
                JambSubjectId = model.JambSubjectId,

            });
        }

        private async Task ProcessResult(JambDisplayQuestionVm model, JambStudentQuestion studentdetails, double scoreCount)
        {
            var examRule = await Db.JambExamRules.AsNoTracking().Where(x => x.JambSubjectId.Equals(model.JambSubjectId))
                                    .Select(s => new { scorePerQuestion = s.ScorePerQuestion, totalQuestion = s.TotalQuestion })
                                    .FirstOrDefaultAsync();
            double sum = examRule.scorePerQuestion * studentdetails.TotalQuestion;
            double total = scoreCount * examRule.scorePerQuestion;
            var jambExamLog = new JambExamLog()
            {
                StudentId = studentdetails.StudentId,
                JambSubjectId = studentdetails.JambSubjectId,
                Score = total,
                TotalScore = sum,
                ExamTaken = true
            };
            // Db.ExamLogs.AddOrUpdate(examLog);
            Db.Set<JambExamLog>().AddOrUpdate(jambExamLog);
            await Db.SaveChangesAsync();

            Session["Rem_Time"] = null;

            string subjectName = await Db.JambSubjects.AsNoTracking().Where(x => x.JambSubjectId.Equals(model.JambSubjectId))
                .Select(c => c.SubjectName).FirstOrDefaultAsync();

        }

        private async Task ProcessResultTimeOut(JambExamLogVm model, JambStudentQuestion studentdetails, double scoreCount)
        {
            var examRule = await Db.JambExamRules.AsNoTracking().Where(x => x.JambSubjectId.Equals(studentdetails.JambSubjectId))
                                    .Select(s => new { scorePerQuestion = s.ScorePerQuestion, totalQuestion = s.TotalQuestion })
                                    .FirstOrDefaultAsync();
            double sum = examRule.scorePerQuestion * studentdetails.TotalQuestion;
            double total = scoreCount * examRule.scorePerQuestion;
            var examLog = new JambExamLog()
            {
                StudentId = studentdetails.StudentId,
                JambSubjectId = studentdetails.JambSubjectId,
                Score = total,
                TotalScore = sum,
                ExamTaken = true
            };

            // Db.ExamLogs.AddOrUpdate(examLog);
            Db.Set<JambExamLog>().AddOrUpdate(examLog);
            await Db.SaveChangesAsync();

            Session["Rem_Time"] = null;

            string subjectName = await Db.JambSubjects.AsNoTracking().Where(x => x.JambSubjectId.Equals(model.JambSubjectId))
                .Select(c => c.SubjectName).FirstOrDefaultAsync();

        }


        public async Task<ActionResult> SubmitExam(string studentId, int JambSubjectId, int levelId, int examType)
        {
            string myStudentId = studentId.Trim();

            double scoreCount = await Db.JambStudentQuestions.Where(x => x.StudentId.Equals(myStudentId)
                        && x.JambSubjectId.Equals(JambSubjectId))
                        .CountAsync(c => c.IsCorrect.Equals(true));

            var studentdetails =
                Db.JambStudentQuestions.FirstOrDefault(x => x.StudentId.Equals(studentId) && x.JambSubjectId.Equals(JambSubjectId));

            if (studentdetails != null)
            {
                var model = new JambExamLogVm()
                {
                    StudentId = studentId,
                    JambSubjectId = JambSubjectId
                };
                await ProcessResultTimeOut(model, studentdetails, scoreCount);

            }

            return RedirectToAction("Index", "JambExamLogs", new
            {
                studentId = studentId,
                subjectId = JambSubjectId,
            });
        }

        public ActionResult ExamIndex(string studentId, int? JambSubjectId, string score)
        {

            ViewBag.StudentId = studentId;
            ViewBag.JambSubjectId = JambSubjectId;
            ViewBag.Score = score;

            Session["Rem_Time"] = null;
            return View();
            //return View(studentList.ToList());
        }

        [ValidateInput(false)]
        private async Task SaveAnswer(JambDisplayQuestionVm model, string studentId, int questionId, string answer)
        {
            var question = Db.JambStudentQuestions.FirstOrDefault(s => s.StudentId.Equals(studentId)
                                                                   && s.QuestionNumber.Equals(questionId)
                                                                   && s.JambSubjectId.Equals(model.JambSubjectId));
            if (question.Answer.ToUpper().Equals(answer.ToUpper()))
            {
                question.IsCorrect = true;
                question.SelectedAnswer = model.SelectedAnswer;
                question.Check1 = model.Check1;
                question.Check2 = model.Check2;
                question.Check3 = model.Check3;
                question.Check4 = model.Check4;
                Db.Entry(question).State = EntityState.Modified;
                await Db.SaveChangesAsync();
            }
            else
            {
                question.IsCorrect = false;
                question.SelectedAnswer = model.SelectedAnswer;
                question.Check1 = model.Check1;
                question.Check2 = model.Check2;
                question.Check3 = model.Check3;
                question.Check4 = model.Check4;
                Db.Entry(question).State = System.Data.Entity.EntityState.Modified;
                await Db.SaveChangesAsync();
            }
        }

        private async Task SaveMultiChoiceAnswer(JambDisplayQuestionVm model, string checkedAnswer)
        {
            var question = Db.JambStudentQuestions.FirstOrDefault(s => s.StudentId.Equals(model.StudentId)
                                                                   && s.QuestionNumber.Equals(model.QuestionNo)
                                                                   && s.JambSubjectId.Equals(model.JambSubjectId));
            string[] myAnswer = question.Answer.Split(',');
            StringBuilder answerbuilder = new StringBuilder();

            foreach (var item in myAnswer)
            {
                answerbuilder.Append(item);
            }

            string value = answerbuilder.ToString();
            string answer = SortStringAlphabetically(answerbuilder.ToString());


            if (answer.ToUpper().Equals(checkedAnswer.ToUpper()))
            {
                question.IsCorrect = true;
                question.SelectedAnswer = model.SelectedAnswer;
                question.Check1 = model.Check1;
                question.Check2 = model.Check2;
                question.Check3 = model.Check3;
                question.Check4 = model.Check4;
                Db.Entry(question).State = EntityState.Modified;
                await Db.SaveChangesAsync();
            }
            else
            {
                question.IsCorrect = false;
                question.SelectedAnswer = model.SelectedAnswer;
                question.Check1 = model.Check1;
                question.Check2 = model.Check2;
                question.Check3 = model.Check3;
                question.Check4 = model.Check4;
                Db.Entry(question).State = System.Data.Entity.EntityState.Modified;
                await Db.SaveChangesAsync();
            }
        }

        static string SortStringAlphabetically(string str)
        {
            char[] foo = str.ToArray();
            Array.Sort(foo);
            return new string(foo);
        }

        private string CheckAnswerForSingleChoice(JambDisplayQuestionVm model)
        {
            if (model.SelectedAnswer.Equals(model.Option1))
            {
                return "A";
            }
            if (model.SelectedAnswer.Equals(model.Option2))
            {
                return "B";
            }
            if (model.SelectedAnswer.Equals(model.Option3))
            {
                return "C";
            }
            if (model.SelectedAnswer.Equals(model.Option4))
            {
                return "D";
            }
            return "";
        }

        private JambStudentQuestion CheckQuestionType(JambDisplayQuestionVm model)
        {
            var questionType = Db.JambStudentQuestions.FirstOrDefault(x => x.QuestionNumber.Equals(model.QuestionNo)
                                                                       && x.StudentId.Equals(model.StudentId) &&
                                                                       x.JambSubjectId.Equals(model.JambSubjectId));
            return questionType;
        }

    }
}