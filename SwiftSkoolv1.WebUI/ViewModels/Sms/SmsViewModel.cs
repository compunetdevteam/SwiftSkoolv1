﻿using System.ComponentModel.DataAnnotations;

namespace SwiftSkool.ViewModel.Sms
{
    public class SmsViewModel
    {
        public string ClassName { get; set; }

        public string Term { get; set; }

        public string Session { get; set; }

        public string SenderId { get; set; }

        [Display(Name = "Message")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}