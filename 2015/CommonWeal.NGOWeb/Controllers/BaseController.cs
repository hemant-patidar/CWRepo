﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CommonWeal.NGOWeb;
using CommonWeal.NGOWeb.ViewModel;
using CommonWeal.NGOWeb.Utility;
using CommonWeal.Data;

namespace CommonWeal.NGOWeb.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public List<NGOUser> GetTopNgo { get; set; }
        public static int pageleft { get; set; }
        public List<AreaOfInterest> CategoryList { get; set; }
        public LoggedInUser LoginUser { get; set; }

        /// <summary>
        /// Fetch logged in user detail
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (User.Identity.IsAuthenticated)
            {
                using (CommonWealEntities CWContext = new CommonWealEntities())
                {
                    var userId = Convert.ToInt32(HttpContext.User.Identity.Name);
                    var usr = CWContext.Users.Where(user => user.LoginID == userId && user.IsActive && !user.IsBlock).FirstOrDefault();


                    this.LoginUser = new LoggedInUser()
                    {
                        LoginID = usr.LoginID,
                        LoginEmailID = usr.LoginEmailID,
                        LoginUserType = usr.LoginUserType,
                    };


                    switch ((EnumHelper.UserType)usr.LoginUserType)
                    {
                        case EnumHelper.UserType.Admin:
                            this.LoginUser.UserName = "Admin";
                            break;
                        case EnumHelper.UserType.NGOAdmin:

                            //this.LoginUser.UserName = CWContext.NGOUsers.Where(user => user.LoginID == this.LoginUser.LoginID).FirstOrDefault().NGOName; ;
                            this.LoginUser.UserName = CWContext.NGOUsers.Where(user => user.LoginID == this.LoginUser.LoginID).FirstOrDefault().NGOName;
                            this.LoginUser.LoginUserType = 1; // Added on 07/11/2016 by Rishiraj

                            break;
                        case CommonWeal.Data.EnumHelper.UserType.User:
                            var reguser = CWContext.RegisteredUsers.Where(user => user.LoginID == this.LoginUser.LoginID).FirstOrDefault(); ;
                            this.LoginUser.UserName = reguser.FirstName + " " + reguser.LastName;
                            this.LoginUser.LoginUserType = 3; // Added on 07/11/2016 by Rishiraj
                            break;
                    }


                    ViewBag.LoginUser = LoginUser;
                    ViewBag.UserType = LoginUser.LoginUserType; // Added on 07/11/2016 by Rishiraj


                }

            }
            else
            {
                this.LoginUser = new LoggedInUser()
                {
                    LoginID = 0,
                    LoginEmailID = "default",
                    LoginUserType = 0,
                };
            }
            using (CommonWealEntities CWContext = new CommonWealEntities())
            {
                CategoryList = CWContext.AreaOfInterests.ToList();
                ViewBag.category = new SelectList(CategoryList, "categoryId", "categoryName");
                //GetTopNgo = (System.Collections.Generic.List<int>)CWContext.NGOUsers.OrderByDescending(x => x.PostCount)
                //    .Select(x => Convert.ToInt32(x.LoginID)).Take(5);
                var TopNGo = CWContext.NGOUsers.OrderByDescending(x => x.PostCount).Take(5);
                GetTopNgo = TopNGo.Take(5).ToList();

            }

        }
        /// <summary>
        /// Create Context and set role for authenticated user
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);


            HttpCookie authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket =
                                            FormsAuthentication.Decrypt(authCookie.Value);
                string[] roles = authTicket.UserData.Split(new Char[] { ',' });
                //set roles and recreate context user
                GenericPrincipal userPrincipal = new GenericPrincipal(new GenericIdentity(authTicket.Name), roles);

                HttpContext.User = userPrincipal;

            }

        }


    }
}
