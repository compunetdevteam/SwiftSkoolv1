using SwiftSkool.Models;
using System;
using System.Collections.Generic;

namespace SwiftSkoolv1.Domain.ClassRoom
{
    public class ClassRoom
    {
        public string ClassRoomId { get; set; }
        public string ClassRoomName { get; set; }
        public string SubjectName { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public ICollection<JoinClassRoom> JoinClassRooms { get; set; }
        public ICollection<ClassRoomTopic> ClassRoomTopics { get; set; }
        public ICollection<ClassroomMaterial> ClassroomMaterials { get; set; }
        public ICollection<ClassRoomAnouncement> ClassRoomAnouncements { get; set; }
        public ICollection<ClassRoomComment> ClassroomComments { get; set; }


    }

    public class JoinClassRoom
    {
        public int JoinClassRoomId { get; set; }
        public string ClassRoomId { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public ClassRoom ClassRoom { get; set; }

    }

    public class ClassRoomTopic
    {
        public int ClassRoomTopicId { get; set; }
        public string ClassRoomId { get; set; }
        public string TopicName { get; set; }
        public string ShortDescription { get; set; }
        public ClassRoom ClassRoom { get; set; }
        public ICollection<TopicAssignment> TopicAssignments { get; set; }
        public ICollection<TopicQuestion> TopicQuestions { get; set; }
        public ICollection<ClassRoomComment> ClassroomComments { get; set; }

    }
    public class TopicAssignment
    {
        public int TopicAssignmentId { get; set; }
        public int ClassRoomTopicId { get; set; }
        public string AssignmentTitle { get; set; }
        public string AssignmentDescription { get; set; }
        public DateTime? AssignmentDueDate { get; set; }
        public ClassRoomTopic ClassRoomTopic { get; set; }

    }

    public class TopicQuestion
    {
        public int TopicQuestionId { get; set; }
        public int ClassRoomTopicId { get; set; }
        public string AssignmentTitle { get; set; }
        public string AssignmentDescription { get; set; }

        public DateTime? AssignmentDueDate { get; set; }
        public ClassRoomTopic ClassRoomTopic { get; set; }

    }

    public class ClassRoomAnouncement
    {
        public int ClassRoomAnouncementId { get; set; }
        public string ClassRoomId { get; set; }
        public string AnnouncementTitle { get; set; }
        public string AnnouncementBody { get; set; }
        public DateTime? AnnouncementDate { get; set; }
        public ClassRoom ClassRooms { get; set; }

    }

    public class ClassroomMaterial
    {
        public int ClassroomMaterialId { get; set; }
        public string ClassRoomId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialLocation { get; set; }
        public ClassRoom ClassRooms { get; set; }
    }

    public class ClassRoomComment
    {
        public int ClassroomCommentId { get; set; }
        public string ClassRoomId { get; set; }
        public int ClassRoomTopicId { get; set; }
        public string CommentMessage { get; set; }
        public ClassRoom ClassRooms { get; set; }

        public ClassRoomTopic ClassRoomTopic { get; set; }
        public ICollection<ClassRoomCommentReply> ClassRoomCommentReplies { get; set; }
    }
    public class ClassRoomCommentReply
    {
        public int ClassroomCommentReplyId { get; set; }
        public int ClassroomCommentId { get; set; }
        public string ReplyMessage { get; set; }
        public ClassRoomComment ClassRoomComment { get; set; }
    }
}