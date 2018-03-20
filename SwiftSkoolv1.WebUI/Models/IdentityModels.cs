using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SwiftSkoolv1.Domain;
using SwiftSkoolv1.Domain.Calender;
using SwiftSkoolv1.Domain.CBT;
using SwiftSkoolv1.Domain.ClassRoom;
using SwiftSkoolv1.Domain.JambPractice;
using SwiftSkoolv1.Domain.Objects;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SwiftSkoolv1.WebUI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string SchoolId { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class SwiftSkoolDbContext : IdentityDbContext<ApplicationUser>
    {
        public SwiftSkoolDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static SwiftSkoolDbContext Create()
        {
            return new SwiftSkoolDbContext();
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Staff> Staffs { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<ContinuousAssessment> ContinuousAssessments { get; set; }
        //public DbSet<SubjectPositions> SubjectPositions { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<FeePayment> FeePayments { get; set; }
        public DbSet<FeeType> FeeTypes { get; set; }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookIssue> BookIssues { get; set; }
        public DbSet<Guardian> Guardians { get; set; }
        public DbSet<AssignedClass> AssignedClasses { get; set; }
        public DbSet<AssignSubject> AssignSubjects { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Affective> Affectives { get; set; }
        public DbSet<Psychomotor> Psychomotors { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<StudentQuestion> StudentQuestions { get; set; }

        public DbSet<ExamRule> ExamRules { get; set; }

        public DbSet<AssignFormTeacherToClass> AssignFormTeacherToClasses { get; set; }

        public DbSet<AssignSubjectTeacher> AssignSubjectTeachers { get; set; }

        public DbSet<PrincipalComment> PrincipalComments { get; set; }

        public DbSet<TeacherComment> TeacherComments { get; set; }

        public DbSet<Term> Terms { get; set; }

        public DbSet<AppointmentDiary> AppointmentDiary { get; set; }

        public DbSet<SubjectRegistration> SubjectRegistrations { get; set; }

        public DbSet<SchoolClass> SchoolClasses { get; set; }

        public DbSet<BehaviorSkillCategory> BehaviorSkillCategories { get; set; }

        public DbSet<BehaviouralSkill> BehaviouralSkills { get; set; }

        public DbSet<AssignBehavior> AssignBehaviors { get; set; }

        public DbSet<ReportCard> ReportCards { get; set; }

        public DbSet<HomePageSetUp> HomePageSetUps { get; set; }

        public DbSet<CaSetUp> CaSetUps { get; set; }

        public DbSet<CaList> CaLists { get; set; }

        public DbSet<School> Schools { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<ResultAvailability> ResultAvailabilities { get; set; }
        public DbSet<TimeTable> TimeTables { get; set; }

        public DbSet<Module> Modules { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TopicMaterial> TopicMaterials { get; set; }
        public DbSet<SchoolEvent> SchoolEvents { get; set; }

        public DbSet<ReportCardSetting> ReportCardSettings { get; set; }

        public DbSet<AssignReportCard> AssignReportCards { get; set; }

        public DbSet<ResultUpload> ResultUploads { get; set; }

        public DbSet<LessonNote> LessonNotes { get; set; }

        public DbSet<JambQuestionAnswer> JambQuestionAnswers { get; set; }
        public DbSet<JambStudentQuestion> JambStudentQuestions { get; set; }
        public DbSet<JambExamRule> JambExamRules { get; set; }
        public DbSet<JambExamLog> JambExamLogs { get; set; }
        public DbSet<JambSubject> JambSubjects { get; set; }
        public DbSet<RemitaPaymentLog> RemitaPaymentLogs { get; set; }

        public DbSet<RemitaFeeSetting> RemitaFeeSettings { get; set; }
        public DbSet<ParentEmailAddress> ParentEmailAddresses { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    //Configure domain classes using modelBuilder here
        //    //disable automatic change tracking
        //    ApplicationDbContext.Configuration.AutoDetectChangesEnabled = false;

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}