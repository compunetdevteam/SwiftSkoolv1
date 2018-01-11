using Microsoft.AspNet.Identity;
using Rotativa;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.WebUI.BusinessLogic;
using SwiftSkoolv1.WebUI.Models;
using SwiftSkoolv1.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class ResultsController : BaseController
    {
        private readonly GradeRemark _gradeRemark;
        private ResultCommandManager _resultCommand;

        public ResultsController()
        {
            _gradeRemark = new GradeRemark();
        }

        public ActionResult ThirdTermResult()
        {
            return View();
        }

        // GET: Results
        public async Task<ActionResult> SearhResult()
        {
            ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentId", "FullName");
            return View();
        }
        public async Task<ActionResult> SearhResultMain()
        {
            ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            ViewBag.StudentId = new SelectList(await _query.StudentListAsync(userSchool), "StudentId", "FullName");
            return View();
        }
        public ActionResult StudentSearch()
        {
            ViewBag.SessionName = new SelectList(_query.SessionList(), "SessionName", "SessionName");
            ViewBag.TermName = new SelectList(_query.TermList(), "TermName", "TermName");
            return View();
        }
        #region Result Display
        public async Task<ActionResult> DownloadResult(string StudentId, string TermName, string SessionName)
        {
            if (String.IsNullOrEmpty(StudentId))
            {
                StudentId = User.Identity.GetUserId();
            }
            var id = StudentId.Trim();
            var termName = TermName.Trim();
            var sessionName = SessionName.Trim();

            var reportModel = await GenerateTermReport(id, termName, sessionName);

            var cardSetting = await Db.AssignReportCards.AsNoTracking().Where(x => x.ClassName.Equals(reportModel.ClassName))
                                    .Select(s => s.ReportCardType).FirstOrDefaultAsync();
            if (cardSetting.ToString().Equals(ReportCardType.WithPositionPrimary.ToString()))
            {
                return new ViewAsPdf("WithPositionPrimary", reportModel);
            }
            if (cardSetting.ToString().Equals(ReportCardType.WithoutPositionPrimary.ToString()))
            {
                return new ViewAsPdf("WithoutPositionPrimary", reportModel);
            }
            if (cardSetting.ToString().Equals(ReportCardType.WithPositionSecondary.ToString()))
            {
                return new ViewAsPdf("WithPositionSecondary", reportModel);
            }
            if (cardSetting.ToString().Equals(ReportCardType.WithoutPositionSecondary.ToString()))
            {
                return new ViewAsPdf("WithoutPositionSecondary", reportModel);
            }

            return new ViewAsPdf("DownloadResult", reportModel);
        }
        #region Result Templates
        public ActionResult WithoutPositionPrimary(ReportCardVm reportModel)
        {
            return new ViewAsPdf("WithoutPositionPrimary", reportModel);
        }
        public ActionResult WithPositionPrimary(ReportCardVm reportModel)
        {
            return new ViewAsPdf("WithPositionPrimary", reportModel);
        }
        public ActionResult WithoutPositionSecondary(ReportCardVm reportModel)
        {

            return new ViewAsPdf("WithoutPositionSecondary", reportModel);
        }
        public ActionResult WithPositionSecondary(ReportCardVm reportModel)
        {
            return new ViewAsPdf("WithPositionSecondary", reportModel);
        }


        #endregion

        public async Task<ActionResult> ViewResult(string StudentId, string TermName, string SessionName)
        {
            var id = StudentId;
            var termName = TermName;
            var sessionName = SessionName;
            var reportModel = await GenerateTermReport(id, termName, sessionName);
            return View(reportModel);

        }
        private async Task<ReportVm> GenerateTermReport(string id, string termName, string sessionName)
        {
            _resultCommand = new ResultCommandManager(id, termName, sessionName, userSchool);
            var reportModel = new ReportVm();
            var newCalist = new List<ContinuousAssesmentVm>();

            reportModel.ReportCardSetting = await Db.ReportCardSettings.AsNoTracking()
                                    .Where(x => x.SchoolId.Equals(userSchool)).FirstOrDefaultAsync();

            var mySchoolClassName = Db.Classes.AsNoTracking().Where(x => x.SchoolId.ToUpper().Trim().Equals(userSchool)
                                        && x.FullClassName.Equals(_resultCommand._className))
                                        .Select(s => s.ClassName).FirstOrDefault();
            reportModel.SchoolClassName = mySchoolClassName;


            foreach (var ca in _resultCommand._studentCa)
            {
                var caVm = new ContinuousAssesmentVm
                {
                    SubjectName = ca.Subject.SubjectName,
                    SubjectPosition = _resultCommand.FindSubjectPosition(ca.SubjectId),
                    SubjectHighest = _resultCommand.SubjectHighest(ca.SubjectId),
                    SubjectLowest = _resultCommand.SubjectLowest(ca.SubjectId),
                    ClassAverage = await _resultCommand.CalculateClassAverage(ca.SubjectId),
                    FirstCa = ca.FirstCa,
                    SecondCa = ca.SecondCa,
                    ThirdCa = ca.ThirdCa,
                    ForthCa = ca.ForthCa,
                    FifthCa = ca.FifthCa,
                    SixthCa = ca.SixthCa,
                    SeventhCa = ca.SeventhCa,
                    EightCa = ca.EightCa,
                    NinthtCa = ca.NinthtCa,
                    ExamCa = ca.ExamCa,
                    Total = ca.Total,
                    Grading = ca.Grading,
                    Remark = ca.Remark,
                    StaffName = ca.StaffName
                };
                newCalist.Add(caVm);
            }
            reportModel.ContinuousAssesmentVms = newCalist;

            reportModel.NoOfStudentPerClass = await _resultCommand.NumberOfStudentPerClass();
            reportModel.NoOfSubjectOffered = await _resultCommand.SubjectOfferedByStudent();
            reportModel.AggregateScore = _resultCommand.TotalScorePerStudent();
            reportModel.Average = await _resultCommand.CalculateAverage();
            reportModel.OverAllGrade = _gradeRemark.Grading(reportModel.Average, _resultCommand._className, userSchool);


            var myOtherSkills = await Db.Psychomotors.AsNoTracking().Where(s => s.StudentId.Contains(id)
                                                                                && s.TermName.Contains(termName)
                                                                                && s.SessionName.Contains(sessionName)
                                                                                && s.ClassName.Equals(_resultCommand
                                                                                    ._className))
                .Select(c => c.Id).FirstOrDefaultAsync();


            reportModel.BehaviorCategory = await Db.BehaviorSkillCategories.AsNoTracking()
                .Where(s => s.SchoolId.Equals(userSchool))
                .Select(x => x.Name).ToListAsync();
            reportModel.AssignBehaviors = await Db.AssignBehaviors.Where(s => s.SchoolId.Equals(userSchool)
                                                                              && s.StudentId.Contains(id)
                                                                              && s.TermName.Contains(termName)
                                                                              && s.SessionName.Contains(sessionName))
                .ToListAsync();


            reportModel.AssignBehavior = reportModel.AssignBehaviors.FirstOrDefault();

            reportModel.ReportCard = await Db.ReportCards.FirstOrDefaultAsync(x => x.SchoolId.Equals(userSchool)
                                                                                   && x.TermName.ToUpper().Equals(termName)
                                                                                   && x.SessionName.Equals(sessionName));


            //ViewBag.Class = 
            reportModel.PrincipalComment = _gradeRemark.PrincipalRemark(reportModel.Average, _resultCommand._className, userSchool);
            reportModel.TermName = termName;
            reportModel.SessionName = sessionName;
            reportModel.ClassName = _resultCommand._className;
            reportModel.Student = await Db.Students.FindAsync(id);
            reportModel.CaSetUp = await Db.CaSetUps.AsNoTracking().Where(x => x.SchoolId.ToUpper().Trim().Equals(userSchool) &&
                                                                              x.IsTrue.Equals(true) && x.TermName.ToUpper().Equals(termName.ToUpper())
                                                                              && x.ClassName.Equals(mySchoolClassName))
                .OrderBy(o => o.CaOrder).ToListAsync();
            reportModel.CaSetUpCount = reportModel.CaSetUp.Count();

            var myAggregateList = new List<AggregateList>();

            var classMate = Db.AssignedClasses.AsNoTracking().Where(x => x.ClassName.Equals(_resultCommand._className)
                                    && x.SchoolId.Equals(userSchool))
                                .Select(s => s.StudentId).ToList();
            foreach (var student in classMate)
            {
                var aggregateList = new AggregateList();
                var sumValue = await Db.CaLists.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool) &&
                                                                          x.StudentId.Equals(student) && x.ClassName.Equals(_resultCommand._className)
                                                                          && x.TermName.Equals(termName) && x.SessionName.Equals(sessionName))
                                   .SumAsync(s => (double?)s.Total) ?? 0;
                aggregateList.Score = Convert.ToDouble(sumValue);
                aggregateList.StudentId = student;

                myAggregateList.Add(aggregateList);
            }

            reportModel.AggregatePosition = _resultCommand.FindAggregatePosition(myAggregateList);
            return reportModel;
        }



        public async Task<ActionResult> SummaryResult(string StudentId, string SessionName)
        {
            var cModel = new CummulativeReportVm();
            var summaryCaList = new List<SummaryCa>();
            cModel.Student = await Db.Students.FindAsync(StudentId);
            var noOfStudentInClass = 0;
            var className = Db.AssignedClasses.AsNoTracking().Where(x => x.SchoolId.Equals(userSchool)
                                           && x.StudentId.ToUpper().Trim().Equals(StudentId)
                                           && x.TermName.ToUpper().Equals("THIRD")
                                           && x.SessionName.ToUpper().Equals(SessionName))
                                            .Select(y => y.ClassName).FirstOrDefault();

            var subjectOffered = await GetSubjectId(className, StudentId);
            foreach (var subject in subjectOffered)
            {
                var resultSummaryCmd = new ResultSummaryCmd(StudentId, SessionName, subject, userSchool);
                var summaryCa = new SummaryCa
                {
                    SubjectName = await Db.Subjects.Where(x => x.SubjectId.Equals(subject)).Select(s => s.SubjectName).FirstOrDefaultAsync(),
                    FirstTermScore = resultSummaryCmd.FirstTermScore,
                    FirstTermGrade = resultSummaryCmd.FirstTermSubjectGrade,
                    FirstTermPosition = resultSummaryCmd.FirstTermSubjectPosition,
                    SecondTermScore = resultSummaryCmd.SecondTermScore,
                    SecondTermPosition = resultSummaryCmd.FirstTermSubjectPosition,
                    SeondTermGrade = resultSummaryCmd.SecondTermSubjectGrade,
                    ThirdTermScore = resultSummaryCmd.ThirdTermScore,
                    ThirdTermGrade = resultSummaryCmd.ThirdTermSubjectGrade,
                    ThirdTermPosition = resultSummaryCmd.ThirdTermSubjectPosition,
                    SubjectGrade = resultSummaryCmd.SummaryGrading,
                    WeightedScore = resultSummaryCmd.WeightedScores,
                    SubjectRemark = resultSummaryCmd.SummaryRemark,
                    SubjectAverage = resultSummaryCmd.ClassAverage
                };
                noOfStudentInClass = resultSummaryCmd.NoOfStudentPerClass;
                className = resultSummaryCmd.ClassName;

                summaryCaList.Add(summaryCa);
            }

            cModel.SummaryCas = summaryCaList;
            cModel.NoOfSubjectOffered = subjectOffered.Count();
            cModel.NoOfStudentPerClass = noOfStudentInClass;
            cModel.SessionName = SessionName;
            cModel.ClassName = className;
            cModel.AggregateScore = cModel.SummaryCas.Sum(s => s.WeightedScore);

            return View(cModel);

        }

        #endregion
        public async Task<List<int>> GetSubjectId(string _className, string _studentId)
        {
            var subjectAssigned = await Db.AssignSubjects.AsNoTracking().Where(c => c.SchoolId.ToUpper().Trim().Equals(userSchool)
                                                    && c.ClassName.ToUpper().Trim().Equals(_className)
                                                    && c.TermName.ToUpper().Equals("FIRST"))
                                                    .Select(s => s.Subject.SubjectId).ToListAsync();
            var subjectregistration = await Db.SubjectRegistrations.AsNoTracking().Where(x => x.SchoolId.ToUpper().Trim().Equals(userSchool)
                                                    && x.StudentId.ToUpper().Trim().Equals(_studentId.ToUpper().Trim()))
                                                    .Select(s => s.Subject.SubjectId).ToListAsync();
            if (subjectregistration.Count > 1)
            {
                return subjectregistration;
            }
            return subjectAssigned;
            //var noOfSubjectPerStudent = _db.AssignSubjects.Count(x => x.ClassName.Equals(className));

        }

        //public async Task<ActionResult> PrintSecondTerm(string FileId)
        //{
        //    DownloadFiles obj = new DownloadFiles();
        //    string NewFileName = FileId + ".pdf";
        //    var filesCol = obj.GetFiles();
        //    string CurrentFileName = (from fls in filesCol
        //                              where fls.FileName == NewFileName
        //                              select fls.FilePath).First();

        //    string contentType = string.Empty;

        //    if (CurrentFileName.Contains(".pdf"))
        //    {
        //        contentType = "application/pdf";
        //    }

        //    else if (CurrentFileName.Contains(".docx"))
        //    {
        //        contentType = "application/docx";
        //    }
        //    return File(CurrentFileName, contentType, CurrentFileName);
        //}


        //public ActionResult PrintTest(string id, string term, string sessionName)
        //{


        //    var className = Db.AssignedClasses.Where(x => x.StudentId.Equals(id) && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
        //                                             && x.SessionName.ToUpper().Trim().Equals(sessionName.ToUpper().Trim()))
        //                                        .Select(y => y.ClassName)
        //                                        .FirstOrDefault();
        //    string subject = "MATHEMATICS";

        //    // var className = "JSS1 A";

        //    ViewBag.Subject = _resultCommand.SubjectOfferedByStudent(id, term, sessionName);
        //    var sumPerSubject = Db.ContinuousAssessments.Where(x => x.SubjectCode.ToUpper().Trim().Equals(subject.ToUpper().Trim())
        //                                                           && x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim())
        //                                                            && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
        //                                                    && x.SessionName.ToUpper().Trim().Equals(sessionName.ToUpper().Trim()))
        //                                                    .Sum(y => y.Total);
        //    double classAverage = _resultCommand.CalculateClassAverage(className, term, sessionName, subject.ToUpper().Trim());
        //    var studentPerClass = Db.AssignedClasses.Count(x => x.ClassName.ToUpper().Trim().Equals(className.ToUpper().Trim())
        //                                                        && x.TermName.ToUpper().Trim().Equals(term.ToUpper().Trim())
        //                                                     && x.SessionName.ToUpper().Trim().Equals(sessionName.ToUpper().Trim()));

        //    double average = _resultCommand.CalculateAverage(id, className, term, sessionName);
        //    double totalScore = _resultCommand.TotalScorePerStudent(id, className, term, sessionName);
        //    // return Math.Round(sumPerSubject, 2);
        //    ViewBag.Term = term;
        //    ViewBag.Session = sessionName;
        //    ViewBag.ClassName = className;
        //    ViewBag.SubjectTotal = Math.Round(sumPerSubject, 2);

        //    ViewBag.ClassAverage = classAverage;
        //    ViewBag.StudentPerClass = studentPerClass;
        //    ViewBag.Average = average;
        //    ViewBag.TotalScore = totalScore;

        //    return View();

        //}
        //public ActionResult PrintSummaryReport(string id, string sessionName)
        //{
        //    var summary = new SummaryReportViewModel()
        //    {
        //        Results = Db.Results.Where(s => s.StudentId.Contains(id)
        //                                && s.SessionName.Contains(sessionName)).ToList(),
        //        ReportSummaries = Db.ReportSummarys.Where(s => s.StudentId.Equals(id)
        //                                            && s.SessionName.Equals(sessionName)).ToList()
        //    };
        //    //foreach (var item in studentResults.Where(c => c.))
        //    //{

        //    //}
        //    return View(summary);
        //}


    }
}