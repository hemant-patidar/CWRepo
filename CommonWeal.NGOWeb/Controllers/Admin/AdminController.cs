﻿using System.Linq;
using System.Web.Mvc;
using CommonWeal.NGOWeb;


namespace CommonWeal.NGOWeb.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    //[Authorize]
    public class AdminController : BaseController
    {
        dbOperations obj = new dbOperations();
        public ActionResult Index()
        {
            CommonWealEntities1 obj = new CommonWealEntities1();
            var users = obj.NGOUsers;
            var CountOfRequests = users.Where(w => w.IsActive == false && w.IsBlock == false).Count();
            ViewBag.COR = CountOfRequests;
            var CountOfActiveUsers = users.Where(w => w.IsActive == true && w.IsBlock == false).Count();
            ViewBag.COAU = CountOfActiveUsers;
            var CountOfBlockedUsers = users.Where(w => w.IsBlock == true).Count();
            ViewBag.COBU = CountOfBlockedUsers;
            return View();

        }

        public ActionResult Active_Users()
        {
            CommonWealEntities1 context = new CommonWealEntities1();
            dbOperations obj = new dbOperations();
            var request = obj.GetAllUserAccepted();
            return View(request);
        }

        public ActionResult Blocked_Users()
        {
            CommonWealEntities1 context = new CommonWealEntities1();
            dbOperations obj = new dbOperations();
            var request = obj.GetAllUserBlocked();
            return View(request);
        }

        public ActionResult Requests()
        {
            CommonWealEntities1 context = new CommonWealEntities1();
            dbOperations obj = new dbOperations();
            var request = obj.GetAllUserNotAccepted();
            return View(request);
        }

        public ActionResult Settings()
        {
            return View();
        }
        public ActionResult Accept(int id)
        {
            CommonWealEntities1 context = new CommonWealEntities1();
            //User UL = new User ();
            var ob = context.Users.Where(w => w.LoginID == id).FirstOrDefault();
            ob.IsActive = true;
            context.SaveChanges();
            var ob1 = context.NGOUsers.Where(w => w.LoginID == id).FirstOrDefault();
            ob1.IsActive = true;
            context.SaveChanges();
            return RedirectToAction("Requests", "Admin");

        }
        public ActionResult Block(int id)
        {


            CommonWealEntities1 context = new CommonWealEntities1();
            User UL = new User();
            var ob = context.Users.Where(w => w.LoginID == id).FirstOrDefault();
            ob.IsActive = false;
            ob.IsBlock = true;
            context.SaveChanges();
            var ob1 = context.NGOUsers.Where(w => w.LoginID == id).FirstOrDefault();
            ob1.IsActive = false;
            ob1.IsBlock = true;
            context.SaveChanges();
            return RedirectToAction("Active_Users", "Admin");
        }
        public ActionResult Unblock(int id)
        {


            CommonWealEntities1 context = new CommonWealEntities1();
            User UL = new User();
            var ob = context.Users.Where(w => w.LoginID == id).FirstOrDefault();
            ob.IsActive = true;
            ob.IsBlock = false;
            var ob1 = context.NGOUsers.Where(w => w.LoginID == id).FirstOrDefault();
            ob1.IsActive = true;
            ob1.IsBlock = false;
            context.SaveChanges();
            return RedirectToAction("Blocked_Users", "Admin");
        }


    }
}