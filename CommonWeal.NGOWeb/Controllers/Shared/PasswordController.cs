﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonWeal.Data;

namespace CommonWeal.NGOWeb.Controllers.Shared
{
    public class PasswordController : Controller
    {
        //
        // GET: /Password/


        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        public ActionResult ConfirmOTP(string UserEmail)
        {
            var EnteredEmail = UserEmail;
            CommonWealEntities context = new CommonWealEntities();
            dbOperations obj = new dbOperations();
            var request = obj.RegisteredUserIsAccepted();
            var request2 = obj.UserIsAccepted();

            var request1 = obj.RegisteredNGOIsAccepted();
            var ob = request.Where(w => w.UserEmail == EnteredEmail).FirstOrDefault();
            // var ob = context.RegisteredUsers.Where(w => w.UserEmail == EnteredEmail).FirstOrDefault();
            //var ob1 = context.NGOUsers.Where(w => w.NGOEmailID == EnteredEmail).FirstOrDefault();
            var ob1 = request1.Where(w => w.NGOEmailID == EnteredEmail).FirstOrDefault();
            //var Email = ob.UserEmail;
            if (ob != null || ob1 != null)
            {
                foreach (var EmailId in request2)
                {
                    if
                        (EnteredEmail == EmailId.LoginEmailID.ToString()) //
                    {
                        //return( RedirectToAction("ForgotPasswordConfirm",EmailId));
                        //ViewBag.Email = UserEmail;
                        var Randomcode = obj.GenerateRandomPassword(7);
                        obj.SendActivationEmail(UserEmail, Randomcode);
                        ViewBag.code = Randomcode;
                        ForgotPassword entry = new ForgotPassword();
                        entry.EmailId = UserEmail;
                        entry.OTP = Randomcode;
                        entry.CreatedOn = System.DateTime.Now;
                        entry.ModifiedOn = System.DateTime.Now;
                        entry.CreatedBy = UserEmail;
                        // context.Users.Add(obj);
                        context.ForgotPasswords.Add(entry);
                        context.SaveChanges();
                        Session["FinalEmail"] = UserEmail;
                        Session["OTP"] = Randomcode;
                        return View();//"ConfirmPassword","Password"
                    }
                }
            }
            else
            {
                //TempData["EmailID"] = "<script>alert('EmailId doesnot exist please register');</script>";
                TempData["EmailID"] = "Email-id does not exist. Please, enter registered email-id.";
                return RedirectToAction("ForgotPassword", "Password");

            }
            return View();
        }

        //[HttpPost]
        public ActionResult ConfirmPassword(string FinalEmail, string OTP, string FinalOTP)
        {
            CommonWealEntities context = new CommonWealEntities();
            dbOperations obj = new dbOperations();

            //if (OTP == FinalOTP)
            //{
            //    return View();
            //}
            var checkOTP = obj.UserDetail(FinalOTP);
            if (checkOTP != null)
            {
                var matchEmail = checkOTP.EmailId;
                if (matchEmail == FinalEmail)
                {
                    return View();
                }
            }
            else
            {
                TempData["wrongOTP"] = "<script>alert('Invalid OTP');</script>";
                return RedirectToAction("ForgotPassword", "Password");
            }
            return View();
        }

        public ActionResult UpdatePassword(string NewPassword, string ConfirmPassword, string FinalEmail)//,string ConfirmOTP, string OTP
        {
            //if (ConfirmOTP == OTP)
            //{
            var EnteredEmail = FinalEmail;
            CommonWealEntities context = new CommonWealEntities();
            dbOperations obj = new dbOperations();

            if (NewPassword == ConfirmPassword)
            {
                var request = obj.RegisteredUserIsAccepted();
                var ob = context.RegisteredUsers.Where(w => w.UserEmail == EnteredEmail).FirstOrDefault();
                if (ob != null) //.UserEmail
                {
                    try
                    {
                        ob.UserPassword = NewPassword;
                        ob.ConfirmPassword = ConfirmPassword;
                        var LoginId = ob.LoginID;
                        var NewUserPassword = context.Users.Where(w => w.LoginID == LoginId).FirstOrDefault();
                        NewUserPassword.LoginPassword = NewPassword;
                        context.SaveChanges();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                    {
                        Exception raise = dbEx;
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                string message = string.Format("{0}:{1}",
                                    validationErrors.Entry.Entity.ToString(),
                                    validationError.ErrorMessage);
                                // raise a new exception nesting  
                                // the current instance as InnerException  
                                raise = new InvalidOperationException(message, raise);
                            }
                        }
                        throw raise;
                    }

                }

                var ob1 = context.NGOUsers.Where(w => w.NGOEmailID == EnteredEmail).FirstOrDefault();
                if (ob1 != null)
                {
                    ob1.NGOPassword = NewPassword;
                    var LoginId = ob1.LoginID;
                    ob1.ConfirmPassword = ConfirmPassword;
                    //var LoginId = ob.LoginID;
                    var ChangePassword = context.Users.Where(w => w.LoginID == LoginId).FirstOrDefault();
                    ChangePassword.LoginPassword = NewPassword;
                    context.SaveChanges();
                }
                //Session["FinalEmail"] = null;
                Session.Remove("FinalEmail");
                //return Content("Password Changed Successfully please Login");
                TempData["PasswordChange"] = "<script>alert('Password changed successfully. Please, login');</script>";
                return RedirectToAction("Index", "Login");
                
            }
            //}


            else
            {
                TempData["Wrong"] = "<script>alert('Enter valid OTP');</script>";
                return RedirectToAction("ForgotPassword", "Password", new { iid = FinalEmail });
            }
            return View();

        }


        public ActionResult ChangPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangPassword(int x)
        {
            return View();
        }

    }
}
