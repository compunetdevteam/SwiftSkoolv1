namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Affectives",
                c => new
                    {
                        AffectiveId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(),
                        TermName = c.String(),
                        SessionName = c.String(),
                        ClassName = c.String(),
                        Honesty = c.String(),
                        SelfConfidence = c.String(),
                        Sociability = c.String(),
                        Punctuality = c.String(),
                        Neatness = c.String(),
                        Initiative = c.String(),
                        Organization = c.String(),
                        AttendanceInClass = c.String(),
                        HonestyAndReliability = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.AffectiveId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.AppointmentDiaries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        SomeImportantKey = c.Int(nullable: false),
                        DateTimeScheduled = c.DateTime(nullable: false),
                        AppointmentLength = c.Int(nullable: false),
                        StatusENUM = c.Int(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.AssignBehaviors",
                c => new
                    {
                        AssignBehaviorId = c.Int(nullable: false, identity: true),
                        BehaviouralSkillId = c.String(),
                        SkillScore = c.String(maxLength: 15),
                        TeacherComment = c.String(maxLength: 50),
                        StudentId = c.String(maxLength: 25),
                        TermName = c.String(maxLength: 15),
                        SessionName = c.String(maxLength: 15),
                        SchoolOpened = c.Int(nullable: false),
                        NoOfAbsence = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        BehaviorCategory = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                        BehaviouralSkill_BehaviouralSkillId = c.Int(),
                    })
                .PrimaryKey(t => t.AssignBehaviorId)
                .ForeignKey("dbo.BehaviouralSkills", t => t.BehaviouralSkill_BehaviouralSkillId)
                .Index(t => t.SchoolId)
                .Index(t => t.BehaviouralSkill_BehaviouralSkillId);
            
            CreateTable(
                "dbo.BehaviouralSkills",
                c => new
                    {
                        BehaviouralSkillId = c.Int(nullable: false, identity: true),
                        SkillName = c.String(maxLength: 35),
                        BehaviorSkillCategoryId = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                        BehaviorSkillCategories_BehaviorSkillCategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.BehaviouralSkillId)
                .ForeignKey("dbo.BehaviorSkillCategories", t => t.BehaviorSkillCategories_BehaviorSkillCategoryId)
                .Index(t => t.SchoolId)
                .Index(t => t.BehaviorSkillCategories_BehaviorSkillCategoryId);
            
            CreateTable(
                "dbo.BehaviorSkillCategories",
                c => new
                    {
                        BehaviorSkillCategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.BehaviorSkillCategoryId)
                .Index(t => t.Name, unique: true)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.AssignedClasses",
                c => new
                    {
                        AssignedClassId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        ClassName = c.String(nullable: false, maxLength: 15),
                        TermName = c.String(nullable: false, maxLength: 15),
                        SessionName = c.String(nullable: false, maxLength: 15),
                        StudentName = c.String(maxLength: 35),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                        Result_ResultId = c.Int(),
                        Staff_StaffId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AssignedClassId)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .ForeignKey("dbo.Staffs", t => t.Staff_StaffId)
                .Index(t => t.StudentId)
                .Index(t => t.SchoolId)
                .Index(t => t.Result_ResultId)
                .Index(t => t.Staff_StaffId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        MiddleName = c.String(),
                        UserName = c.String(),
                        FullName = c.String(),
                        Age = c.Int(nullable: false),
                        PhoneNumber = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        PlaceOfBirth = c.String(),
                        StateOfOrigin = c.String(),
                        AdmissionDate = c.DateTime(nullable: false),
                        Religion = c.String(),
                        Gender = c.String(),
                        Tribe = c.String(),
                        CurrentClass = c.String(),
                        StudentPassport = c.Binary(),
                        Active = c.Boolean(nullable: false),
                        IsGraduated = c.Boolean(nullable: false),
                        ApplicationUserId = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                        Result_ResultId = c.Int(),
                        BookIssue_BookIssueId = c.Int(),
                    })
                .PrimaryKey(t => t.StudentId)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .ForeignKey("dbo.BookIssues", t => t.BookIssue_BookIssueId)
                .Index(t => t.SchoolId)
                .Index(t => t.Result_ResultId)
                .Index(t => t.BookIssue_BookIssueId);
            
            CreateTable(
                "dbo.CaLists",
                c => new
                    {
                        CaListId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        StudentName = c.String(maxLength: 45),
                        StudentId = c.String(maxLength: 128),
                        TermName = c.String(maxLength: 15),
                        SessionName = c.String(maxLength: 25),
                        ClassName = c.String(maxLength: 25),
                        FirstCa = c.Double(nullable: false),
                        SecondCa = c.Double(nullable: false),
                        ThirdCa = c.Double(nullable: false),
                        ForthCa = c.Double(nullable: false),
                        FifthCa = c.Double(nullable: false),
                        SixthCa = c.Double(nullable: false),
                        SeventhCa = c.Double(nullable: false),
                        EightCa = c.Double(nullable: false),
                        NinthtCa = c.Double(nullable: false),
                        ExamCa = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        Grading = c.String(),
                        Remark = c.String(),
                        StaffName = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.CaListId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.StudentId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        SubjectId = c.Int(nullable: false, identity: true),
                        SubjectCode = c.String(nullable: false, maxLength: 20),
                        SubjectName = c.String(nullable: false, maxLength: 35),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                        Class_ClassId = c.Int(),
                        ContinuousAssessment_ContinuousAssessmentId = c.Int(),
                        ContinuousAssessment_ContinuousAssessmentId1 = c.Int(),
                        Staff_StaffId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SubjectId)
                .ForeignKey("dbo.Classes", t => t.Class_ClassId)
                .ForeignKey("dbo.ContinuousAssessments", t => t.ContinuousAssessment_ContinuousAssessmentId)
                .ForeignKey("dbo.ContinuousAssessments", t => t.ContinuousAssessment_ContinuousAssessmentId1)
                .ForeignKey("dbo.Staffs", t => t.Staff_StaffId)
                .Index(t => t.SubjectCode)
                .Index(t => t.SchoolId)
                .Index(t => t.Class_ClassId)
                .Index(t => t.ContinuousAssessment_ContinuousAssessmentId)
                .Index(t => t.ContinuousAssessment_ContinuousAssessmentId1)
                .Index(t => t.Staff_StaffId);
            
            CreateTable(
                "dbo.AssignSubjects",
                c => new
                    {
                        AssignSubjectId = c.Int(nullable: false, identity: true),
                        ClassName = c.String(nullable: false, maxLength: 25),
                        SubjectId = c.Int(nullable: false),
                        TermName = c.String(nullable: false, maxLength: 15),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                        Class_ClassId = c.Int(),
                        Result_ResultId = c.Int(),
                    })
                .PrimaryKey(t => t.AssignSubjectId)
                .ForeignKey("dbo.Classes", t => t.Class_ClassId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .Index(t => t.SubjectId)
                .Index(t => t.SchoolId)
                .Index(t => t.Class_ClassId)
                .Index(t => t.Result_ResultId);
            
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        ClassId = c.Int(nullable: false, identity: true),
                        SchoolName = c.String(nullable: false, maxLength: 25),
                        ClassLevel = c.Int(nullable: false),
                        ClassType = c.String(nullable: false, maxLength: 15),
                        ClassName = c.String(),
                        FullClassName = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                        AssignClass_AssignedClassId = c.Int(),
                        Staff_StaffId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ClassId)
                .ForeignKey("dbo.AssignedClasses", t => t.AssignClass_AssignedClassId)
                .ForeignKey("dbo.Staffs", t => t.Staff_StaffId)
                .Index(t => t.SchoolId)
                .Index(t => t.AssignClass_AssignedClassId)
                .Index(t => t.Staff_StaffId);
            
            CreateTable(
                "dbo.ExamLogs",
                c => new
                    {
                        ExamLogId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(maxLength: 128),
                        SubjectId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                        TermId = c.Int(nullable: false),
                        SessionId = c.Int(nullable: false),
                        ExamTypeId = c.Int(nullable: false),
                        Score = c.Double(nullable: false),
                        TotalScore = c.Double(nullable: false),
                        ExamTaken = c.Boolean(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.ExamLogId)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.ExamTypes", t => t.ExamTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Terms", t => t.TermId, cascadeDelete: true)
                .ForeignKey("dbo.Sessions", t => t.SessionId, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.SubjectId)
                .Index(t => t.ClassId)
                .Index(t => t.TermId)
                .Index(t => t.SessionId)
                .Index(t => t.ExamTypeId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.ExamTypes",
                c => new
                    {
                        ExamTypeId = c.Int(nullable: false, identity: true),
                        ExamName = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.ExamTypeId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.ExamSettings",
                c => new
                    {
                        ExamSettingId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                        TermId = c.Int(nullable: false),
                        SessionId = c.Int(nullable: false),
                        ExamDate = c.DateTime(nullable: false),
                        ExamTypeId = c.Int(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.ExamSettingId)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.ExamTypes", t => t.ExamTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Sessions", t => t.SessionId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.Terms", t => t.TermId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.ClassId)
                .Index(t => t.TermId)
                .Index(t => t.SessionId)
                .Index(t => t.ExamTypeId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        SessionId = c.Int(nullable: false, identity: true),
                        SessionName = c.String(nullable: false, maxLength: 20),
                        ActiveSession = c.Boolean(nullable: false),
                        ContinuousAssessment_ContinuousAssessmentId = c.Int(),
                    })
                .PrimaryKey(t => t.SessionId)
                .ForeignKey("dbo.ContinuousAssessments", t => t.ContinuousAssessment_ContinuousAssessmentId)
                .Index(t => t.SessionName, unique: true)
                .Index(t => t.ContinuousAssessment_ContinuousAssessmentId);
            
            CreateTable(
                "dbo.Terms",
                c => new
                    {
                        TermId = c.Int(nullable: false, identity: true),
                        TermName = c.String(maxLength: 15),
                        ActiveTerm = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TermId)
                .Index(t => t.TermName, unique: true);
            
            CreateTable(
                "dbo.CaSetUps",
                c => new
                    {
                        CaSetUpId = c.Int(nullable: false, identity: true),
                        CaOrder = c.Int(nullable: false),
                        CaCaption = c.String(maxLength: 25),
                        MaximumScore = c.Double(nullable: false),
                        CaPercentage = c.Double(nullable: false),
                        IsTrue = c.Boolean(nullable: false),
                        ClassId = c.Int(nullable: false),
                        TermId = c.Int(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.CaSetUpId)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.Terms", t => t.TermId, cascadeDelete: true)
                .Index(t => t.ClassId)
                .Index(t => t.TermId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.QuestionAnswers",
                c => new
                    {
                        QuestionAnswerId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                        ExamTypeId = c.Int(nullable: false),
                        Question = c.String(nullable: false),
                        Option1 = c.String(),
                        Option2 = c.String(),
                        Option3 = c.String(),
                        Option4 = c.String(),
                        Answer = c.String(nullable: false),
                        QuestionHint = c.String(),
                        QuestionType = c.Int(nullable: false),
                        IsFillInTheGag = c.Boolean(nullable: false),
                        IsMultiChoiceAnswer = c.Boolean(nullable: false),
                        IsSingleChoiceAnswer = c.Boolean(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.QuestionAnswerId)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.ExamTypes", t => t.ExamTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.ClassId)
                .Index(t => t.ExamTypeId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.ExamRules",
                c => new
                    {
                        ExamRuleId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                        ResultDivision = c.Int(nullable: false),
                        ScorePerQuestion = c.Double(nullable: false),
                        TotalQuestion = c.Int(nullable: false),
                        MaximumTime = c.Int(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.ExamRuleId)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.ClassId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.ResultDivisions",
                c => new
                    {
                        ResultDivisionId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.ResultDivisionId)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.ClassId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.TimeTables",
                c => new
                    {
                        TimeTableId = c.Int(nullable: false, identity: true),
                        ClassId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        Days = c.Int(nullable: false),
                        StartDuration = c.Int(nullable: false),
                        EndDuration = c.Int(nullable: false),
                        Classes_ClassId = c.Int(),
                        Subjects_SubjectId = c.Int(),
                        Class_ClassId = c.Int(),
                        Class_ClassId1 = c.Int(),
                        Subject_SubjectId = c.Int(),
                        Subject_SubjectId1 = c.Int(),
                    })
                .PrimaryKey(t => t.TimeTableId)
                .ForeignKey("dbo.Classes", t => t.Classes_ClassId)
                .ForeignKey("dbo.Subjects", t => t.Subjects_SubjectId)
                .ForeignKey("dbo.Classes", t => t.Class_ClassId)
                .ForeignKey("dbo.Classes", t => t.Class_ClassId1)
                .ForeignKey("dbo.Subjects", t => t.Subject_SubjectId)
                .ForeignKey("dbo.Subjects", t => t.Subject_SubjectId1)
                .Index(t => t.Classes_ClassId)
                .Index(t => t.Subjects_SubjectId)
                .Index(t => t.Class_ClassId)
                .Index(t => t.Class_ClassId1)
                .Index(t => t.Subject_SubjectId)
                .Index(t => t.Subject_SubjectId1);
            
            CreateTable(
                "dbo.AssignSubjectTeachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        ClassName = c.String(),
                        StaffName = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.ContinuousAssessments",
                c => new
                    {
                        ContinuousAssessmentId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false, maxLength: 25, unicode: false),
                        TermName = c.String(nullable: false, maxLength: 15),
                        SessionName = c.String(nullable: false, maxLength: 15),
                        SubjectId = c.Int(nullable: false),
                        ClassName = c.String(nullable: false, maxLength: 15),
                        FirstTest = c.Double(nullable: false),
                        SecondTest = c.Double(nullable: false),
                        ThirdTest = c.Double(nullable: false),
                        ExamScore = c.Double(nullable: false),
                        StaffName = c.String(nullable: false, maxLength: 35),
                        Total = c.Double(nullable: false),
                        Grading = c.String(),
                        Remark = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                        Result_ResultId = c.Int(),
                        Subject_SubjectId = c.Int(),
                    })
                .PrimaryKey(t => t.ContinuousAssessmentId)
                .ForeignKey("dbo.Results", t => t.Result_ResultId)
                .ForeignKey("dbo.Subjects", t => t.Subject_SubjectId)
                .Index(t => t.StudentId)
                .Index(t => t.SchoolId)
                .Index(t => t.Result_ResultId)
                .Index(t => t.Subject_SubjectId);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        ResultId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(maxLength: 25),
                        ClassName = c.String(maxLength: 25),
                        Term = c.String(maxLength: 25),
                        SessionName = c.String(maxLength: 15),
                        SubjectName = c.Int(nullable: false),
                        SubjectHighest = c.Double(nullable: false),
                        SubjectLowest = c.Double(nullable: false),
                        SubjectPosition = c.Int(nullable: false),
                        AggretateScore = c.Double(nullable: false),
                        Average = c.Double(nullable: false),
                        ClassAverage = c.Double(nullable: false),
                        TotalQualityPoint = c.Double(nullable: false),
                        TotalCreditUnit = c.Double(nullable: false),
                        GradePointAverage = c.Double(nullable: false),
                        GPA = c.Double(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                        Subject_SubjectId = c.Int(),
                    })
                .PrimaryKey(t => t.ResultId)
                .ForeignKey("dbo.Subjects", t => t.Subject_SubjectId)
                .Index(t => t.SchoolId)
                .Index(t => t.Subject_SubjectId);
            
            CreateTable(
                "dbo.SubjectRegistrations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        StudentName = c.String(nullable: false, maxLength: 65),
                        SubjectId = c.Int(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.SubjectId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.FeePayments",
                c => new
                    {
                        FeePaymentId = c.Int(nullable: false),
                        StudentId = c.String(nullable: false, maxLength: 128),
                        FeeName = c.String(maxLength: 25),
                        Term = c.String(nullable: false, maxLength: 15),
                        Session = c.String(nullable: false, maxLength: 15),
                        PaidFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentMode = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Remaining = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                        FeeType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.FeePaymentId)
                .ForeignKey("dbo.FeeTypes", t => t.FeeType_Id)
                .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.SchoolId)
                .Index(t => t.FeeType_Id);
            
            CreateTable(
                "dbo.FeeTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FeeName = c.String(maxLength: 25),
                        Description = c.String(maxLength: 45),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.Guardians",
                c => new
                    {
                        GuardianId = c.Int(nullable: false, identity: true),
                        Salutation = c.String(maxLength: 15),
                        FirstName = c.String(maxLength: 25),
                        MiddleName = c.String(maxLength: 25),
                        LastName = c.String(maxLength: 25),
                        Gender = c.String(maxLength: 10),
                        PhoneNumber = c.String(maxLength: 15),
                        Email = c.String(maxLength: 35),
                        Address = c.String(maxLength: 75),
                        Occupation = c.String(maxLength: 35),
                        Relationship = c.String(maxLength: 25),
                        Religion = c.String(maxLength: 15),
                        LGAOforigin = c.String(maxLength: 25),
                        StateOfOrigin = c.String(maxLength: 25),
                        MotherName = c.String(maxLength: 25),
                        MotherMaidenName = c.String(maxLength: 25),
                        UserName = c.String(),
                        FullName = c.String(),
                        StudentId = c.String(maxLength: 128),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.GuardianId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.InviteStudents",
                c => new
                    {
                        InviteStudentId = c.Int(nullable: false, identity: true),
                        JoinClassRoomId = c.Int(nullable: false),
                        ClassRoomId = c.String(maxLength: 128),
                        StudentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.InviteStudentId)
                .ForeignKey("dbo.ClassRooms", t => t.ClassRoomId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .Index(t => t.ClassRoomId)
                .Index(t => t.StudentId);
            
            CreateTable(
                "dbo.ClassRooms",
                c => new
                    {
                        ClassRoomId = c.String(nullable: false, maxLength: 128),
                        ClassRoomName = c.String(),
                        SubjectName = c.String(),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.ClassRoomId);
            
            CreateTable(
                "dbo.ClassRoomAnouncements",
                c => new
                    {
                        ClassRoomAnouncementId = c.Int(nullable: false, identity: true),
                        ClassRoomId = c.String(maxLength: 128),
                        AnnouncementTitle = c.String(),
                        AnnouncementBody = c.String(),
                        AnnouncementDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ClassRoomAnouncementId)
                .ForeignKey("dbo.ClassRooms", t => t.ClassRoomId)
                .Index(t => t.ClassRoomId);
            
            CreateTable(
                "dbo.ClassRoomComments",
                c => new
                    {
                        ClassroomCommentId = c.Int(nullable: false, identity: true),
                        ClassRoomId = c.String(maxLength: 128),
                        ClassRoomTopicId = c.Int(nullable: false),
                        CommentMessage = c.String(),
                    })
                .PrimaryKey(t => t.ClassroomCommentId)
                .ForeignKey("dbo.ClassRooms", t => t.ClassRoomId)
                .ForeignKey("dbo.ClassRoomTopics", t => t.ClassRoomTopicId, cascadeDelete: true)
                .Index(t => t.ClassRoomId)
                .Index(t => t.ClassRoomTopicId);
            
            CreateTable(
                "dbo.ClassRoomCommentReplies",
                c => new
                    {
                        ClassroomCommentReplyId = c.Int(nullable: false, identity: true),
                        ClassroomCommentId = c.Int(nullable: false),
                        ReplyMessage = c.String(),
                    })
                .PrimaryKey(t => t.ClassroomCommentReplyId)
                .ForeignKey("dbo.ClassRoomComments", t => t.ClassroomCommentId, cascadeDelete: true)
                .Index(t => t.ClassroomCommentId);
            
            CreateTable(
                "dbo.ClassRoomTopics",
                c => new
                    {
                        ClassRoomTopicId = c.Int(nullable: false, identity: true),
                        ClassRoomId = c.String(maxLength: 128),
                        TopicName = c.String(),
                        ShortDescription = c.String(),
                    })
                .PrimaryKey(t => t.ClassRoomTopicId)
                .ForeignKey("dbo.ClassRooms", t => t.ClassRoomId)
                .Index(t => t.ClassRoomId);
            
            CreateTable(
                "dbo.TopicAssignments",
                c => new
                    {
                        TopicAssignmentId = c.Int(nullable: false, identity: true),
                        ClassRoomTopicId = c.Int(nullable: false),
                        AssignmentTitle = c.String(),
                        AssignmentDescription = c.String(),
                        AssignmentDueDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TopicAssignmentId)
                .ForeignKey("dbo.ClassRoomTopics", t => t.ClassRoomTopicId, cascadeDelete: true)
                .Index(t => t.ClassRoomTopicId);
            
            CreateTable(
                "dbo.TopicQuestions",
                c => new
                    {
                        TopicQuestionId = c.Int(nullable: false, identity: true),
                        ClassRoomTopicId = c.Int(nullable: false),
                        AssignmentTitle = c.String(),
                        AssignmentDescription = c.String(),
                        AssignmentDueDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TopicQuestionId)
                .ForeignKey("dbo.ClassRoomTopics", t => t.ClassRoomTopicId, cascadeDelete: true)
                .Index(t => t.ClassRoomTopicId);
            
            CreateTable(
                "dbo.ClassroomMaterials",
                c => new
                    {
                        ClassroomMaterialId = c.Int(nullable: false, identity: true),
                        ClassRoomId = c.String(maxLength: 128),
                        MaterialName = c.String(),
                        MaterialLocation = c.String(),
                    })
                .PrimaryKey(t => t.ClassroomMaterialId)
                .ForeignKey("dbo.ClassRooms", t => t.ClassRoomId)
                .Index(t => t.ClassRoomId);
            
            CreateTable(
                "dbo.InviteTeachers",
                c => new
                    {
                        InviteTeacherId = c.Int(nullable: false, identity: true),
                        JoinClassRoomId = c.Int(nullable: false),
                        ClassRoomId = c.String(maxLength: 128),
                        StaffId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.InviteTeacherId)
                .ForeignKey("dbo.ClassRooms", t => t.ClassRoomId)
                .ForeignKey("dbo.Staffs", t => t.StaffId)
                .Index(t => t.ClassRoomId)
                .Index(t => t.StaffId);
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        StaffId = c.String(nullable: false, maxLength: 128),
                        Salutation = c.String(maxLength: 15),
                        FirstName = c.String(maxLength: 35),
                        MiddleName = c.String(maxLength: 35),
                        LastName = c.String(maxLength: 35),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        Gender = c.String(),
                        Address = c.String(),
                        StateOfOrigin = c.String(),
                        Designation = c.String(),
                        StaffPassport = c.Binary(),
                        DateOfBirth = c.DateTime(nullable: false),
                        MaritalStatus = c.String(),
                        Qualifications = c.String(),
                        Password = c.String(),
                        ApplicationUserId = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.StaffId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.AssignFormTeacherToClasses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClassName = c.String(),
                        Username = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.BookIssues",
                c => new
                    {
                        BookIssueId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false, maxLength: 25),
                        AccessionNo = c.String(nullable: false, maxLength: 25),
                        IssueDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.BookIssueId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        AccessionNo = c.String(nullable: false, maxLength: 25),
                        BookId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Author = c.String(nullable: false, maxLength: 50),
                        JointAuthor = c.String(maxLength: 50),
                        Subject = c.String(nullable: false, maxLength: 30),
                        ISBN = c.String(nullable: false, maxLength: 35),
                        Edition = c.String(nullable: false),
                        Publisher = c.String(nullable: false, maxLength: 35),
                        PlaceOfPublish = c.String(maxLength: 35),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                        BookIssue_BookIssueId = c.Int(),
                    })
                .PrimaryKey(t => t.AccessionNo)
                .ForeignKey("dbo.BookIssues", t => t.BookIssue_BookIssueId)
                .Index(t => t.SchoolId)
                .Index(t => t.BookIssue_BookIssueId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PostID = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Posts", t => t.PostID, cascadeDelete: true)
                .Index(t => t.PostID);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        DateTime = c.DateTime(nullable: false),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        IsHoliday = c.Boolean(nullable: false),
                        IsCommonToAll = c.Boolean(nullable: false),
                        ThemeColor = c.String(),
                        IsFullDay = c.Byte(nullable: false),
                        StartingDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.FeeCategories",
                c => new
                    {
                        FeeCategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(maxLength: 25),
                        CategoryDescription = c.String(maxLength: 45),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.FeeCategoryId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        GradeId = c.Int(nullable: false, identity: true),
                        GradeName = c.String(maxLength: 15),
                        MinimumValue = c.Int(nullable: false),
                        MaximumValue = c.Int(nullable: false),
                        Remark = c.String(maxLength: 25),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.GradeId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.HomePageSetUps",
                c => new
                    {
                        HomePagesetUpId = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        DescriptiveText = c.String(),
                        FileLocation = c.String(),
                    })
                .PrimaryKey(t => t.HomePagesetUpId);
            
            CreateTable(
                "dbo.PrincipalComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MinimumGrade = c.Double(nullable: false),
                        MaximumGrade = c.Double(nullable: false),
                        Remark = c.String(nullable: false, maxLength: 35),
                        ClassName = c.String(maxLength: 15),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.Psychomotors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentId = c.String(),
                        TermName = c.String(),
                        SessionName = c.String(),
                        ClassName = c.String(),
                        Sports = c.String(),
                        ExtraCurricularActivity = c.String(),
                        Assignment = c.String(),
                        HelpingOthers = c.String(),
                        ManualDuty = c.String(),
                        LevelOfCommitment = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.ReportCards",
                c => new
                    {
                        ReportCardId = c.Int(nullable: false, identity: true),
                        TermName = c.String(),
                        SessionName = c.String(),
                        SchoolOpened = c.Int(nullable: false),
                        NextTermBegin = c.DateTime(nullable: false),
                        NextTermEnd = c.DateTime(nullable: false),
                        PrincipalSignature = c.Binary(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.ReportCardId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SchoolClasses",
                c => new
                    {
                        SchoolClassId = c.Int(nullable: false, identity: true),
                        ClassCode = c.String(nullable: false, maxLength: 20),
                        ClassName = c.String(nullable: false, maxLength: 35),
                    })
                .PrimaryKey(t => t.SchoolClassId);
            
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        SchoolId = c.String(nullable: false, maxLength: 25),
                        Name = c.String(maxLength: 45),
                        Alias = c.String(maxLength: 15),
                        Color = c.String(maxLength: 15),
                        OwernshipType = c.String(maxLength: 35),
                        DateOfEstablishment = c.DateTime(),
                        Address = c.String(maxLength: 75),
                        LocalGovtArea = c.String(maxLength: 25),
                        Logo = c.Binary(),
                        SchoolBanner = c.Binary(),
                    })
                .PrimaryKey(t => t.SchoolId);
            
            CreateTable(
                "dbo.StudentQuestions",
                c => new
                    {
                        StudentQuestionId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(),
                        SubjectId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                        TermId = c.Int(nullable: false),
                        SessionId = c.Int(nullable: false),
                        ExamTypeId = c.Int(nullable: false),
                        Question = c.String(),
                        Option1 = c.String(),
                        Option2 = c.String(),
                        Option3 = c.String(),
                        Option4 = c.String(),
                        Check1 = c.Boolean(nullable: false),
                        Check2 = c.Boolean(nullable: false),
                        Check3 = c.Boolean(nullable: false),
                        Check4 = c.Boolean(nullable: false),
                        FilledAnswer = c.String(),
                        Answer = c.String(),
                        QuestionHint = c.String(),
                        QuestionNumber = c.Int(nullable: false),
                        IsCorrect = c.Boolean(nullable: false),
                        IsFillInTheGag = c.Boolean(nullable: false),
                        IsMultiChoiceAnswer = c.Boolean(nullable: false),
                        SelectedAnswer = c.String(),
                        TotalQuestion = c.Int(nullable: false),
                        ExamTime = c.Int(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.StudentQuestionId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.TeacherComments",
                c => new
                    {
                        TeacherCommentId = c.Int(nullable: false, identity: true),
                        StudentId = c.String(nullable: false, maxLength: 25),
                        TermName = c.String(nullable: false, maxLength: 15),
                        SessionName = c.String(nullable: false),
                        Remark = c.String(nullable: false, maxLength: 15),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TeacherCommentId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SchoolId = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TagPosts",
                c => new
                    {
                        Tag_ID = c.Int(nullable: false),
                        Post_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_ID, t.Post_ID })
                .ForeignKey("dbo.Tags", t => t.Tag_ID, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.Post_ID, cascadeDelete: true)
                .Index(t => t.Tag_ID)
                .Index(t => t.Post_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TagPosts", "Post_ID", "dbo.Posts");
            DropForeignKey("dbo.TagPosts", "Tag_ID", "dbo.Tags");
            DropForeignKey("dbo.Comments", "PostID", "dbo.Posts");
            DropForeignKey("dbo.Students", "BookIssue_BookIssueId", "dbo.BookIssues");
            DropForeignKey("dbo.Books", "BookIssue_BookIssueId", "dbo.BookIssues");
            DropForeignKey("dbo.InviteStudents", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Subjects", "Staff_StaffId", "dbo.Staffs");
            DropForeignKey("dbo.InviteTeachers", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.Classes", "Staff_StaffId", "dbo.Staffs");
            DropForeignKey("dbo.AssignedClasses", "Staff_StaffId", "dbo.Staffs");
            DropForeignKey("dbo.InviteTeachers", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.InviteStudents", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.ClassroomMaterials", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.TopicQuestions", "ClassRoomTopicId", "dbo.ClassRoomTopics");
            DropForeignKey("dbo.TopicAssignments", "ClassRoomTopicId", "dbo.ClassRoomTopics");
            DropForeignKey("dbo.ClassRoomComments", "ClassRoomTopicId", "dbo.ClassRoomTopics");
            DropForeignKey("dbo.ClassRoomTopics", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.ClassRoomComments", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.ClassRoomCommentReplies", "ClassroomCommentId", "dbo.ClassRoomComments");
            DropForeignKey("dbo.ClassRoomAnouncements", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.Guardians", "StudentId", "dbo.Students");
            DropForeignKey("dbo.FeePayments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.FeePayments", "FeeType_Id", "dbo.FeeTypes");
            DropForeignKey("dbo.TimeTables", "Subject_SubjectId1", "dbo.Subjects");
            DropForeignKey("dbo.TimeTables", "Subject_SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.SubjectRegistrations", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.SubjectRegistrations", "StudentId", "dbo.Students");
            DropForeignKey("dbo.Subjects", "ContinuousAssessment_ContinuousAssessmentId1", "dbo.ContinuousAssessments");
            DropForeignKey("dbo.Subjects", "ContinuousAssessment_ContinuousAssessmentId", "dbo.ContinuousAssessments");
            DropForeignKey("dbo.ContinuousAssessments", "Subject_SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Sessions", "ContinuousAssessment_ContinuousAssessmentId", "dbo.ContinuousAssessments");
            DropForeignKey("dbo.Results", "Subject_SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Students", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.ContinuousAssessments", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.AssignSubjects", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.AssignedClasses", "Result_ResultId", "dbo.Results");
            DropForeignKey("dbo.CaLists", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.AssignSubjectTeachers", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.AssignSubjects", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.AssignSubjects", "Class_ClassId", "dbo.Classes");
            DropForeignKey("dbo.TimeTables", "Class_ClassId1", "dbo.Classes");
            DropForeignKey("dbo.TimeTables", "Class_ClassId", "dbo.Classes");
            DropForeignKey("dbo.TimeTables", "Subjects_SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.TimeTables", "Classes_ClassId", "dbo.Classes");
            DropForeignKey("dbo.Subjects", "Class_ClassId", "dbo.Classes");
            DropForeignKey("dbo.ResultDivisions", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ResultDivisions", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.ExamRules", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ExamRules", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.ExamLogs", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ExamLogs", "StudentId", "dbo.Students");
            DropForeignKey("dbo.ExamLogs", "SessionId", "dbo.Sessions");
            DropForeignKey("dbo.QuestionAnswers", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.QuestionAnswers", "ExamTypeId", "dbo.ExamTypes");
            DropForeignKey("dbo.QuestionAnswers", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.CaSetUps", "TermId", "dbo.Terms");
            DropForeignKey("dbo.CaSetUps", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.ExamSettings", "TermId", "dbo.Terms");
            DropForeignKey("dbo.ExamLogs", "TermId", "dbo.Terms");
            DropForeignKey("dbo.ExamSettings", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.ExamSettings", "SessionId", "dbo.Sessions");
            DropForeignKey("dbo.ExamSettings", "ExamTypeId", "dbo.ExamTypes");
            DropForeignKey("dbo.ExamSettings", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.ExamLogs", "ExamTypeId", "dbo.ExamTypes");
            DropForeignKey("dbo.ExamLogs", "ClassId", "dbo.Classes");
            DropForeignKey("dbo.Classes", "AssignClass_AssignedClassId", "dbo.AssignedClasses");
            DropForeignKey("dbo.CaLists", "StudentId", "dbo.Students");
            DropForeignKey("dbo.AssignedClasses", "StudentId", "dbo.Students");
            DropForeignKey("dbo.BehaviouralSkills", "BehaviorSkillCategories_BehaviorSkillCategoryId", "dbo.BehaviorSkillCategories");
            DropForeignKey("dbo.AssignBehaviors", "BehaviouralSkill_BehaviouralSkillId", "dbo.BehaviouralSkills");
            DropIndex("dbo.TagPosts", new[] { "Post_ID" });
            DropIndex("dbo.TagPosts", new[] { "Tag_ID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.StudentQuestions", new[] { "SchoolId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ReportCards", new[] { "SchoolId" });
            DropIndex("dbo.Psychomotors", new[] { "SchoolId" });
            DropIndex("dbo.PrincipalComments", new[] { "SchoolId" });
            DropIndex("dbo.Grades", new[] { "SchoolId" });
            DropIndex("dbo.FeeCategories", new[] { "SchoolId" });
            DropIndex("dbo.Comments", new[] { "PostID" });
            DropIndex("dbo.Books", new[] { "BookIssue_BookIssueId" });
            DropIndex("dbo.Books", new[] { "SchoolId" });
            DropIndex("dbo.BookIssues", new[] { "SchoolId" });
            DropIndex("dbo.AssignFormTeacherToClasses", new[] { "SchoolId" });
            DropIndex("dbo.Staffs", new[] { "SchoolId" });
            DropIndex("dbo.InviteTeachers", new[] { "StaffId" });
            DropIndex("dbo.InviteTeachers", new[] { "ClassRoomId" });
            DropIndex("dbo.ClassroomMaterials", new[] { "ClassRoomId" });
            DropIndex("dbo.TopicQuestions", new[] { "ClassRoomTopicId" });
            DropIndex("dbo.TopicAssignments", new[] { "ClassRoomTopicId" });
            DropIndex("dbo.ClassRoomTopics", new[] { "ClassRoomId" });
            DropIndex("dbo.ClassRoomCommentReplies", new[] { "ClassroomCommentId" });
            DropIndex("dbo.ClassRoomComments", new[] { "ClassRoomTopicId" });
            DropIndex("dbo.ClassRoomComments", new[] { "ClassRoomId" });
            DropIndex("dbo.ClassRoomAnouncements", new[] { "ClassRoomId" });
            DropIndex("dbo.InviteStudents", new[] { "StudentId" });
            DropIndex("dbo.InviteStudents", new[] { "ClassRoomId" });
            DropIndex("dbo.Guardians", new[] { "SchoolId" });
            DropIndex("dbo.Guardians", new[] { "StudentId" });
            DropIndex("dbo.FeeTypes", new[] { "SchoolId" });
            DropIndex("dbo.FeePayments", new[] { "FeeType_Id" });
            DropIndex("dbo.FeePayments", new[] { "SchoolId" });
            DropIndex("dbo.FeePayments", new[] { "StudentId" });
            DropIndex("dbo.SubjectRegistrations", new[] { "SchoolId" });
            DropIndex("dbo.SubjectRegistrations", new[] { "SubjectId" });
            DropIndex("dbo.SubjectRegistrations", new[] { "StudentId" });
            DropIndex("dbo.Results", new[] { "Subject_SubjectId" });
            DropIndex("dbo.Results", new[] { "SchoolId" });
            DropIndex("dbo.ContinuousAssessments", new[] { "Subject_SubjectId" });
            DropIndex("dbo.ContinuousAssessments", new[] { "Result_ResultId" });
            DropIndex("dbo.ContinuousAssessments", new[] { "SchoolId" });
            DropIndex("dbo.ContinuousAssessments", new[] { "StudentId" });
            DropIndex("dbo.AssignSubjectTeachers", new[] { "SchoolId" });
            DropIndex("dbo.AssignSubjectTeachers", new[] { "SubjectId" });
            DropIndex("dbo.TimeTables", new[] { "Subject_SubjectId1" });
            DropIndex("dbo.TimeTables", new[] { "Subject_SubjectId" });
            DropIndex("dbo.TimeTables", new[] { "Class_ClassId1" });
            DropIndex("dbo.TimeTables", new[] { "Class_ClassId" });
            DropIndex("dbo.TimeTables", new[] { "Subjects_SubjectId" });
            DropIndex("dbo.TimeTables", new[] { "Classes_ClassId" });
            DropIndex("dbo.ResultDivisions", new[] { "SchoolId" });
            DropIndex("dbo.ResultDivisions", new[] { "ClassId" });
            DropIndex("dbo.ResultDivisions", new[] { "SubjectId" });
            DropIndex("dbo.ExamRules", new[] { "SchoolId" });
            DropIndex("dbo.ExamRules", new[] { "ClassId" });
            DropIndex("dbo.ExamRules", new[] { "SubjectId" });
            DropIndex("dbo.QuestionAnswers", new[] { "SchoolId" });
            DropIndex("dbo.QuestionAnswers", new[] { "ExamTypeId" });
            DropIndex("dbo.QuestionAnswers", new[] { "ClassId" });
            DropIndex("dbo.QuestionAnswers", new[] { "SubjectId" });
            DropIndex("dbo.CaSetUps", new[] { "SchoolId" });
            DropIndex("dbo.CaSetUps", new[] { "TermId" });
            DropIndex("dbo.CaSetUps", new[] { "ClassId" });
            DropIndex("dbo.Terms", new[] { "TermName" });
            DropIndex("dbo.Sessions", new[] { "ContinuousAssessment_ContinuousAssessmentId" });
            DropIndex("dbo.Sessions", new[] { "SessionName" });
            DropIndex("dbo.ExamSettings", new[] { "SchoolId" });
            DropIndex("dbo.ExamSettings", new[] { "ExamTypeId" });
            DropIndex("dbo.ExamSettings", new[] { "SessionId" });
            DropIndex("dbo.ExamSettings", new[] { "TermId" });
            DropIndex("dbo.ExamSettings", new[] { "ClassId" });
            DropIndex("dbo.ExamSettings", new[] { "SubjectId" });
            DropIndex("dbo.ExamTypes", new[] { "SchoolId" });
            DropIndex("dbo.ExamLogs", new[] { "SchoolId" });
            DropIndex("dbo.ExamLogs", new[] { "ExamTypeId" });
            DropIndex("dbo.ExamLogs", new[] { "SessionId" });
            DropIndex("dbo.ExamLogs", new[] { "TermId" });
            DropIndex("dbo.ExamLogs", new[] { "ClassId" });
            DropIndex("dbo.ExamLogs", new[] { "SubjectId" });
            DropIndex("dbo.ExamLogs", new[] { "StudentId" });
            DropIndex("dbo.Classes", new[] { "Staff_StaffId" });
            DropIndex("dbo.Classes", new[] { "AssignClass_AssignedClassId" });
            DropIndex("dbo.Classes", new[] { "SchoolId" });
            DropIndex("dbo.AssignSubjects", new[] { "Result_ResultId" });
            DropIndex("dbo.AssignSubjects", new[] { "Class_ClassId" });
            DropIndex("dbo.AssignSubjects", new[] { "SchoolId" });
            DropIndex("dbo.AssignSubjects", new[] { "SubjectId" });
            DropIndex("dbo.Subjects", new[] { "Staff_StaffId" });
            DropIndex("dbo.Subjects", new[] { "ContinuousAssessment_ContinuousAssessmentId1" });
            DropIndex("dbo.Subjects", new[] { "ContinuousAssessment_ContinuousAssessmentId" });
            DropIndex("dbo.Subjects", new[] { "Class_ClassId" });
            DropIndex("dbo.Subjects", new[] { "SchoolId" });
            DropIndex("dbo.Subjects", new[] { "SubjectCode" });
            DropIndex("dbo.CaLists", new[] { "SchoolId" });
            DropIndex("dbo.CaLists", new[] { "StudentId" });
            DropIndex("dbo.CaLists", new[] { "SubjectId" });
            DropIndex("dbo.Students", new[] { "BookIssue_BookIssueId" });
            DropIndex("dbo.Students", new[] { "Result_ResultId" });
            DropIndex("dbo.Students", new[] { "SchoolId" });
            DropIndex("dbo.AssignedClasses", new[] { "Staff_StaffId" });
            DropIndex("dbo.AssignedClasses", new[] { "Result_ResultId" });
            DropIndex("dbo.AssignedClasses", new[] { "SchoolId" });
            DropIndex("dbo.AssignedClasses", new[] { "StudentId" });
            DropIndex("dbo.BehaviorSkillCategories", new[] { "SchoolId" });
            DropIndex("dbo.BehaviorSkillCategories", new[] { "Name" });
            DropIndex("dbo.BehaviouralSkills", new[] { "BehaviorSkillCategories_BehaviorSkillCategoryId" });
            DropIndex("dbo.BehaviouralSkills", new[] { "SchoolId" });
            DropIndex("dbo.AssignBehaviors", new[] { "BehaviouralSkill_BehaviouralSkillId" });
            DropIndex("dbo.AssignBehaviors", new[] { "SchoolId" });
            DropIndex("dbo.AppointmentDiaries", new[] { "SchoolId" });
            DropIndex("dbo.Affectives", new[] { "SchoolId" });
            DropTable("dbo.TagPosts");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TeacherComments");
            DropTable("dbo.StudentQuestions");
            DropTable("dbo.Schools");
            DropTable("dbo.SchoolClasses");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ReportCards");
            DropTable("dbo.Psychomotors");
            DropTable("dbo.PrincipalComments");
            DropTable("dbo.HomePageSetUps");
            DropTable("dbo.Grades");
            DropTable("dbo.FeeCategories");
            DropTable("dbo.Events");
            DropTable("dbo.Tags");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
            DropTable("dbo.Books");
            DropTable("dbo.BookIssues");
            DropTable("dbo.AssignFormTeacherToClasses");
            DropTable("dbo.Staffs");
            DropTable("dbo.InviteTeachers");
            DropTable("dbo.ClassroomMaterials");
            DropTable("dbo.TopicQuestions");
            DropTable("dbo.TopicAssignments");
            DropTable("dbo.ClassRoomTopics");
            DropTable("dbo.ClassRoomCommentReplies");
            DropTable("dbo.ClassRoomComments");
            DropTable("dbo.ClassRoomAnouncements");
            DropTable("dbo.ClassRooms");
            DropTable("dbo.InviteStudents");
            DropTable("dbo.Guardians");
            DropTable("dbo.FeeTypes");
            DropTable("dbo.FeePayments");
            DropTable("dbo.SubjectRegistrations");
            DropTable("dbo.Results");
            DropTable("dbo.ContinuousAssessments");
            DropTable("dbo.AssignSubjectTeachers");
            DropTable("dbo.TimeTables");
            DropTable("dbo.ResultDivisions");
            DropTable("dbo.ExamRules");
            DropTable("dbo.QuestionAnswers");
            DropTable("dbo.CaSetUps");
            DropTable("dbo.Terms");
            DropTable("dbo.Sessions");
            DropTable("dbo.ExamSettings");
            DropTable("dbo.ExamTypes");
            DropTable("dbo.ExamLogs");
            DropTable("dbo.Classes");
            DropTable("dbo.AssignSubjects");
            DropTable("dbo.Subjects");
            DropTable("dbo.CaLists");
            DropTable("dbo.Students");
            DropTable("dbo.AssignedClasses");
            DropTable("dbo.BehaviorSkillCategories");
            DropTable("dbo.BehaviouralSkills");
            DropTable("dbo.AssignBehaviors");
            DropTable("dbo.AppointmentDiaries");
            DropTable("dbo.Affectives");
            DropTable("dbo.Administrators");
        }
    }
}
