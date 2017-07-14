﻿using SwiftSkool.Calender;
using SwiftSkool.Models;
using System;
using System.Linq;

namespace SwiftSkool.Services
{
    public class Utils
    {

        public static bool InitialiseDiary()
        {
            // init connection to database
            using (ApplicationDbContext ent = new ApplicationDbContext())
            {


                try
                {
                    for (int i = 0; i < 30; i++)
                    {
                        AppointmentDiary item = new AppointmentDiary
                        {
                            Title = "Appt: " + i.ToString(),
                            SomeImportantKey = i,
                            StatusENUM = GetRandomValue(0, 3)
                        };
                        // record ID is auto generated
                        // random is exclusive - we have three status enums
                        if (i <= 5) // create a few appointments for todays date
                        {
                            item.DateTimeScheduled = GetRandomAppointmentTime(false, true);
                        }
                        else
                        {
                            // rest of appointments on previous and future dates
                            if (i % 2 == 0)
                                item.DateTimeScheduled = GetRandomAppointmentTime(true, false);
                            // flip/flop between date ahead of today and behind today
                            else item.DateTimeScheduled = GetRandomAppointmentTime(false, false);
                        }
                        item.AppointmentLength = GetRandomValue(1, 5) * 15;
                        // appoiment length in blocks of fifteen minutes in this demo
                        ent.AppointmentDiary.Add(item);
                        ent.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    return false;
                }

                return ent.AppointmentDiary.Any();
            }
        }

        public static int GetRandomValue(int LowerBound, int UpperBound)
        {
            Random rnd = new Random();
            return rnd.Next(LowerBound, UpperBound);
        }

        /// <summary>
        /// sends back a date/time +/- 15 days from todays date
        /// </summary>
        public static DateTime GetRandomAppointmentTime(bool GoBackwards, bool Today)
        {
            Random rnd = new Random(Environment.TickCount); // set a new random seed each call
            var baseDate = DateTime.Today;
            if (Today)
                return new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, rnd.Next(9, 18), rnd.Next(1, 6) * 5, 0);
            else
            {
                int rndDays = rnd.Next(1, 16);
                if (GoBackwards)
                    rndDays = rndDays * -1; // make into negative number
                return new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, rnd.Next(9, 18), rnd.Next(1, 6) * 5, 0).AddDays(rndDays);
            }
        }

    }
}