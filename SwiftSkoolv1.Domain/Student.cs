﻿using SwiftSkoolv1.Domain.JambPractice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SwiftSkoolv1.Domain.CEPractice;

namespace SwiftSkoolv1.Domain
{
    public class Student : GeneralSchool
    {

        [Key]
        public string StudentId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string UserName
        {
            get { return $"{LastName} {FirstName}"; }
            set { }
        }

        public string FullName
        {
            get { return $"{LastName} {FirstName} {MiddleName}"; }
            set { }
        }

        public int Age
        {
            get
            {
                var t = DateTime.Now - DateOfBirth;
                return Age = (int)t.Days / 365;
            }
            set { }
        }
        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PlaceOfBirth { get; set; }

        public string StateOfOrigin { get; set; }

        public DateTime AdmissionDate { get; set; }

        public string Religion { get; set; }

        public string Gender { get; set; }

        public string Tribe { get; set; }
        public string CurrentClass { get; set; }

        public byte[] StudentPassport { get; set; }

        public bool Active { get; set; }
        public bool IsGraduated { get; set; }
        public string StudentStatus { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual ICollection<Guardian> Guardian { get; set; }
        public virtual ICollection<FeePayment> FeePayments { get; set; }
        public virtual ICollection<AssignedClass> AssignedClasses { get; set; }
        public virtual ICollection<SubjectRegistration> SubjectRegistrations { get; set; }
        public virtual ICollection<CaList> CaList { get; set; }
        public ICollection<JambExamLog> JambExamLogs { get; set; }
        public ICollection<JambExamRule> JambExamRules { get; set; }
        public ICollection<JambQuestionAnswer> JambQuestionAnswers { get; set; }
        public ICollection<JambStudentQuestion> JambStudentQuestions { get; set; }
        public ICollection<CEExamLog> CEExamLogs { get; set; }
        public ICollection<CEExamRule> CEExamRules { get; set; }
        public ICollection<CEQuestionAnswer> CEQuestionAnswers { get; set; }
        public ICollection<CEStudentQuestion> CEStudentQuestions { get; set; }
    }
}