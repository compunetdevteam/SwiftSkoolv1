﻿using SwiftSkoolv1.Domain.ClassRoom;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwiftSkoolv1.Domain
{
    public class Staff : GeneralSchool
    {
        [Key]
        public string StaffId { get; set; }
        [StringLength(15)]
        public string Salutation { get; set; }
        [StringLength(35)]
        public string FirstName { get; set; }
        [StringLength(35)]
        public string MiddleName { get; set; }
        [StringLength(35)]
        public string LastName { get; set; }
        //[Index(IsUnique = true)]
        //[MaxLength(20)]
        public string PhoneNumber { get; set; }

        //[Index(IsUnique = true)]
        //[MaxLength(20)]
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string StateOfOrigin { get; set; }
        public string Designation { get; set; }
        public byte[] StaffPassport { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Qualifications { get; set; }
        public string Password { get; set; }
        public string Username => $"{this.FirstName} {this.LastName}";
        public string ApplicationUserId { get; set; }
        public virtual ICollection<AssignedClass> AssignedClasses { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<InviteTeacher> InviteTeacher { get; set; }
    }

}