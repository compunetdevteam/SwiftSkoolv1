namespace SwiftSkoolv1.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewMIGRATION : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ClassRoomAnouncements", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.ClassRoomCommentReplies", "ClassroomCommentId", "dbo.ClassRoomComments");
            DropForeignKey("dbo.ClassRoomComments", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.ClassRoomTopics", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.ClassRoomComments", "ClassRoomTopicId", "dbo.ClassRoomTopics");
            DropForeignKey("dbo.TopicAssignments", "ClassRoomTopicId", "dbo.ClassRoomTopics");
            DropForeignKey("dbo.TopicQuestions", "ClassRoomTopicId", "dbo.ClassRoomTopics");
            DropForeignKey("dbo.ClassroomMaterials", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.InviteStudents", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.InviteTeachers", "ClassRoomId", "dbo.ClassRooms");
            DropForeignKey("dbo.InviteTeachers", "StaffId", "dbo.Staffs");
            DropForeignKey("dbo.InviteStudents", "StudentId", "dbo.Students");
            DropIndex("dbo.InviteStudents", new[] { "ClassRoomId" });
            DropIndex("dbo.InviteStudents", new[] { "StudentId" });
            DropIndex("dbo.ClassRoomAnouncements", new[] { "ClassRoomId" });
            DropIndex("dbo.ClassRoomComments", new[] { "ClassRoomId" });
            DropIndex("dbo.ClassRoomComments", new[] { "ClassRoomTopicId" });
            DropIndex("dbo.ClassRoomCommentReplies", new[] { "ClassroomCommentId" });
            DropIndex("dbo.ClassRoomTopics", new[] { "ClassRoomId" });
            DropIndex("dbo.TopicAssignments", new[] { "ClassRoomTopicId" });
            DropIndex("dbo.TopicQuestions", new[] { "ClassRoomTopicId" });
            DropIndex("dbo.ClassroomMaterials", new[] { "ClassRoomId" });
            DropIndex("dbo.InviteTeachers", new[] { "ClassRoomId" });
            DropIndex("dbo.InviteTeachers", new[] { "StaffId" });
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        ModuleId = c.Int(nullable: false, identity: true),
                        SubjectId = c.Int(nullable: false),
                        ClassId = c.Int(nullable: false),
                        TermId = c.Int(nullable: false),
                        ModuleName = c.String(),
                        ModuleDescription = c.String(),
                        ExpectedTime = c.Int(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.ModuleId)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.Terms", t => t.TermId, cascadeDelete: true)
                .Index(t => t.SubjectId)
                .Index(t => t.ClassId)
                .Index(t => t.TermId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.Topics",
                c => new
                    {
                        TopicId = c.Int(nullable: false, identity: true),
                        ModuleId = c.Int(nullable: false),
                        TopicName = c.String(),
                        ExpectedTime = c.Int(nullable: false),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.TopicId)
                .ForeignKey("dbo.Modules", t => t.ModuleId, cascadeDelete: true)
                .Index(t => t.ModuleId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.SubmitAssignments",
                c => new
                    {
                        SubmitAssignmentId = c.Int(nullable: false, identity: true),
                        TopicAssignmentId = c.Int(nullable: false),
                        StudentId = c.String(maxLength: 128),
                        AssignmentBody = c.String(),
                        AttachmentLocation = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.SubmitAssignmentId)
                .ForeignKey("dbo.Students", t => t.StudentId)
                .ForeignKey("dbo.TopicAssignments", t => t.TopicAssignmentId, cascadeDelete: true)
                .Index(t => t.TopicAssignmentId)
                .Index(t => t.StudentId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.AssignmentReviews",
                c => new
                    {
                        SubmitAssignmentId = c.Int(nullable: false),
                        ReviewComment = c.String(),
                        Rating = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.SubmitAssignmentId)
                .ForeignKey("dbo.SubmitAssignments", t => t.SubmitAssignmentId)
                .Index(t => t.SubmitAssignmentId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.TopicMaterials",
                c => new
                    {
                        TopicMaterialId = c.Int(nullable: false, identity: true),
                        TopicId = c.Int(nullable: false),
                        FileType = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        NoteBody = c.String(),
                        Author = c.String(nullable: false),
                        Description = c.String(),
                        FileLocation = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.TopicMaterialId)
                .ForeignKey("dbo.Topics", t => t.TopicId, cascadeDelete: true)
                .Index(t => t.TopicId)
                .Index(t => t.SchoolId);
            
            CreateTable(
                "dbo.BookCategories",
                c => new
                    {
                        BookCategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        Description = c.String(),
                        SchoolId = c.String(maxLength: 15, unicode: false),
                    })
                .PrimaryKey(t => t.BookCategoryId)
                .Index(t => t.SchoolId);
            
            AddColumn("dbo.TopicAssignments", "TopicId", c => c.Int(nullable: false));
            AddColumn("dbo.TopicAssignments", "SchoolId", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.Books", "BookCategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.TopicAssignments", "TopicId");
            CreateIndex("dbo.TopicAssignments", "SchoolId");
            CreateIndex("dbo.Books", "BookCategoryId");
            AddForeignKey("dbo.TopicAssignments", "TopicId", "dbo.Topics", "TopicId", cascadeDelete: true);
            AddForeignKey("dbo.Books", "BookCategoryId", "dbo.BookCategories", "BookCategoryId", cascadeDelete: true);
            DropColumn("dbo.TopicAssignments", "ClassRoomTopicId");
            DropTable("dbo.InviteStudents");
            DropTable("dbo.ClassRooms");
            DropTable("dbo.ClassRoomAnouncements");
            DropTable("dbo.ClassRoomComments");
            DropTable("dbo.ClassRoomCommentReplies");
            DropTable("dbo.ClassRoomTopics");
            DropTable("dbo.TopicQuestions");
            DropTable("dbo.ClassroomMaterials");
            DropTable("dbo.InviteTeachers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InviteTeachers",
                c => new
                    {
                        InviteTeacherId = c.Int(nullable: false, identity: true),
                        JoinClassRoomId = c.Int(nullable: false),
                        ClassRoomId = c.String(maxLength: 128),
                        StaffId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.InviteTeacherId);
            
            CreateTable(
                "dbo.ClassroomMaterials",
                c => new
                    {
                        ClassroomMaterialId = c.Int(nullable: false, identity: true),
                        ClassRoomId = c.String(maxLength: 128),
                        MaterialName = c.String(),
                        MaterialLocation = c.String(),
                    })
                .PrimaryKey(t => t.ClassroomMaterialId);
            
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
                .PrimaryKey(t => t.TopicQuestionId);
            
            CreateTable(
                "dbo.ClassRoomTopics",
                c => new
                    {
                        ClassRoomTopicId = c.Int(nullable: false, identity: true),
                        ClassRoomId = c.String(maxLength: 128),
                        TopicName = c.String(),
                        ShortDescription = c.String(),
                    })
                .PrimaryKey(t => t.ClassRoomTopicId);
            
            CreateTable(
                "dbo.ClassRoomCommentReplies",
                c => new
                    {
                        ClassroomCommentReplyId = c.Int(nullable: false, identity: true),
                        ClassroomCommentId = c.Int(nullable: false),
                        ReplyMessage = c.String(),
                    })
                .PrimaryKey(t => t.ClassroomCommentReplyId);
            
            CreateTable(
                "dbo.ClassRoomComments",
                c => new
                    {
                        ClassroomCommentId = c.Int(nullable: false, identity: true),
                        ClassRoomId = c.String(maxLength: 128),
                        ClassRoomTopicId = c.Int(nullable: false),
                        CommentMessage = c.String(),
                    })
                .PrimaryKey(t => t.ClassroomCommentId);
            
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
                .PrimaryKey(t => t.ClassRoomAnouncementId);
            
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
                "dbo.InviteStudents",
                c => new
                    {
                        InviteStudentId = c.Int(nullable: false, identity: true),
                        JoinClassRoomId = c.Int(nullable: false),
                        ClassRoomId = c.String(maxLength: 128),
                        StudentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.InviteStudentId);
            
            AddColumn("dbo.TopicAssignments", "ClassRoomTopicId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Books", "BookCategoryId", "dbo.BookCategories");
            DropForeignKey("dbo.TopicMaterials", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.TopicAssignments", "TopicId", "dbo.Topics");
            DropForeignKey("dbo.SubmitAssignments", "TopicAssignmentId", "dbo.TopicAssignments");
            DropForeignKey("dbo.SubmitAssignments", "StudentId", "dbo.Students");
            DropForeignKey("dbo.AssignmentReviews", "SubmitAssignmentId", "dbo.SubmitAssignments");
            DropForeignKey("dbo.Topics", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Modules", "TermId", "dbo.Terms");
            DropForeignKey("dbo.Modules", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Modules", "ClassId", "dbo.Classes");
            DropIndex("dbo.BookCategories", new[] { "SchoolId" });
            DropIndex("dbo.Books", new[] { "BookCategoryId" });
            DropIndex("dbo.TopicMaterials", new[] { "SchoolId" });
            DropIndex("dbo.TopicMaterials", new[] { "TopicId" });
            DropIndex("dbo.AssignmentReviews", new[] { "SchoolId" });
            DropIndex("dbo.AssignmentReviews", new[] { "SubmitAssignmentId" });
            DropIndex("dbo.SubmitAssignments", new[] { "SchoolId" });
            DropIndex("dbo.SubmitAssignments", new[] { "StudentId" });
            DropIndex("dbo.SubmitAssignments", new[] { "TopicAssignmentId" });
            DropIndex("dbo.TopicAssignments", new[] { "SchoolId" });
            DropIndex("dbo.TopicAssignments", new[] { "TopicId" });
            DropIndex("dbo.Topics", new[] { "SchoolId" });
            DropIndex("dbo.Topics", new[] { "ModuleId" });
            DropIndex("dbo.Modules", new[] { "SchoolId" });
            DropIndex("dbo.Modules", new[] { "TermId" });
            DropIndex("dbo.Modules", new[] { "ClassId" });
            DropIndex("dbo.Modules", new[] { "SubjectId" });
            DropColumn("dbo.Books", "BookCategoryId");
            DropColumn("dbo.TopicAssignments", "SchoolId");
            DropColumn("dbo.TopicAssignments", "TopicId");
            DropTable("dbo.BookCategories");
            DropTable("dbo.TopicMaterials");
            DropTable("dbo.AssignmentReviews");
            DropTable("dbo.SubmitAssignments");
            DropTable("dbo.Topics");
            DropTable("dbo.Modules");
            CreateIndex("dbo.InviteTeachers", "StaffId");
            CreateIndex("dbo.InviteTeachers", "ClassRoomId");
            CreateIndex("dbo.ClassroomMaterials", "ClassRoomId");
            CreateIndex("dbo.TopicQuestions", "ClassRoomTopicId");
            CreateIndex("dbo.TopicAssignments", "ClassRoomTopicId");
            CreateIndex("dbo.ClassRoomTopics", "ClassRoomId");
            CreateIndex("dbo.ClassRoomCommentReplies", "ClassroomCommentId");
            CreateIndex("dbo.ClassRoomComments", "ClassRoomTopicId");
            CreateIndex("dbo.ClassRoomComments", "ClassRoomId");
            CreateIndex("dbo.ClassRoomAnouncements", "ClassRoomId");
            CreateIndex("dbo.InviteStudents", "StudentId");
            CreateIndex("dbo.InviteStudents", "ClassRoomId");
            AddForeignKey("dbo.InviteStudents", "StudentId", "dbo.Students", "StudentId");
            AddForeignKey("dbo.InviteTeachers", "StaffId", "dbo.Staffs", "StaffId");
            AddForeignKey("dbo.InviteTeachers", "ClassRoomId", "dbo.ClassRooms", "ClassRoomId");
            AddForeignKey("dbo.InviteStudents", "ClassRoomId", "dbo.ClassRooms", "ClassRoomId");
            AddForeignKey("dbo.ClassroomMaterials", "ClassRoomId", "dbo.ClassRooms", "ClassRoomId");
            AddForeignKey("dbo.TopicQuestions", "ClassRoomTopicId", "dbo.ClassRoomTopics", "ClassRoomTopicId", cascadeDelete: true);
            AddForeignKey("dbo.TopicAssignments", "ClassRoomTopicId", "dbo.ClassRoomTopics", "ClassRoomTopicId", cascadeDelete: true);
            AddForeignKey("dbo.ClassRoomComments", "ClassRoomTopicId", "dbo.ClassRoomTopics", "ClassRoomTopicId", cascadeDelete: true);
            AddForeignKey("dbo.ClassRoomTopics", "ClassRoomId", "dbo.ClassRooms", "ClassRoomId");
            AddForeignKey("dbo.ClassRoomComments", "ClassRoomId", "dbo.ClassRooms", "ClassRoomId");
            AddForeignKey("dbo.ClassRoomCommentReplies", "ClassroomCommentId", "dbo.ClassRoomComments", "ClassroomCommentId", cascadeDelete: true);
            AddForeignKey("dbo.ClassRoomAnouncements", "ClassRoomId", "dbo.ClassRooms", "ClassRoomId");
        }
    }
}
