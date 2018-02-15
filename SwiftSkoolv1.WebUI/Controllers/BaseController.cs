﻿using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using SwiftSkoolv1.WebUI.BusinessLogic;
using SwiftSkoolv1.WebUI.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using SwiftSkoolv1.WebUI.ViewModels;

namespace SwiftSkoolv1.WebUI.Controllers
{
    public class BaseController : Controller
    {
        public SwiftSkoolDbContext Db;
        public QueryManager _query;

        public string userSchool;

        public BaseController()
        {
            Db = new SwiftSkoolDbContext();
            _query = new QueryManager();
            userSchool = _query.GetId();
        }


        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var user = User.Identity.GetUserId();
            userSchool = Db.Users.AsNoTracking().Where(x => x.Id.Equals(user))
                .Select(s => s.SchoolId).FirstOrDefault();

            var school = Db.Schools.Find(userSchool);

            // var model = filterContext.Controller.ViewData.Model as BaseViewModel;
            var model = new BaseViewModel();

            if (school != null)
            {
                model.Alias = school.Alias;
                model.SchoolName = school.Name;
                model.SchoolId = school.SchoolId;
                model.Color = school.Color;
                ViewBag.ImageId = school.SchoolId;
            }
            else
            {
                model.Alias = "SwiftSkool";
                model.SchoolName = "SwiftSkool";
                model.SchoolId = "SwiftSkool";
                model.Color = "";
            }
            ViewBag.LayoutViewModel = model;



        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Db != null)
                {
                    Db.Dispose();
                    Db = null;
                }
            }

            base.Dispose(disposing);
        }

        public static List<string> YearCategory()
        {
            var yearCategory = new List<string>();
            yearCategory.Add("1999");
            yearCategory.Add("2000");
            yearCategory.Add("2001");
            yearCategory.Add("2002");
            yearCategory.Add("2003");
            yearCategory.Add("2004");
            yearCategory.Add("2005");
            yearCategory.Add("2006");
            yearCategory.Add("2007");
            yearCategory.Add("2008");
            yearCategory.Add("2009");
            yearCategory.Add("2010");
            yearCategory.Add("2011");
            yearCategory.Add("2013");
            yearCategory.Add("2014");
            yearCategory.Add("2015");
            yearCategory.Add("2016");
            yearCategory.Add("2017");
            return yearCategory;
        }

    }
}