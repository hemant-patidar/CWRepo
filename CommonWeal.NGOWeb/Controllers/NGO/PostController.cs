﻿using CommonWeal.Data;
using CommonWeal.NGOWeb.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;



namespace CommonWeal.NGOWeb.Controllers.NGO
{
    [Authorize]
    public class PostController : BaseController
    {
        /*action for commenting on a post through post*/
        [HttpPost]
        public JsonResult SumitComment(string strComment, int postId)
        {
            string userName = "";
            List<string> userinfo = new List<string>();
            if (strComment != null)
            {
                CommonWealEntities db = new CommonWealEntities();
                PostComment postcmnt = new PostComment();
                postcmnt.CommentText = strComment;
                postcmnt.CreatedOn = DateTime.Now;
                postcmnt.ModifiedOn = DateTime.Now;
                postcmnt.PostID = postId;
                postcmnt.IsDelete = false;
                /*login user property defined in base controller*/
                postcmnt.LoginID = LoginUser.LoginID;
                //Convert.ToInt32(User.Identity.Name);

                db.PostComments.Add(postcmnt);
                db.SaveChanges();
                /*update like count in post table  */
                var post = db.NGOPosts.Where(ngpost => ngpost.PostID == postId).FirstOrDefault();
                post.PostLikeCount = db.PostComments.Where(x => x.PostID == postId).Count();
                db.SaveChanges();
                /*getting user type for getting name from user or ngo table dynamically by switch case */
                int userType = db.Users.Where(user => user.LoginID == LoginUser.LoginID).FirstOrDefault().LoginUserType;

                switch (userType)
                {
                    case 1: string NGOUser = db.NGOUsers.Where(ngusr => ngusr.LoginID == LoginUser.LoginID).FirstOrDefault().NGOName.ToString();
                        userName = NGOUser;
                        break;
                    case 3: var RegUser = db.RegisteredUsers.Where(lgnuser => lgnuser.LoginID == LoginUser.LoginID).FirstOrDefault();
                        userName = RegUser.FirstName + " " + RegUser.LastName;
                        break;

                }

                userinfo.Add(userName);
                userinfo.Add(postcmnt.CreatedOn.Value.ToString("MM/dd/yyyy HH:mm"));
                userinfo.Add(postcmnt.CommentID.ToString());
                userinfo.Add(LoginUser.LoginID.ToString());
            }
            return Json(userinfo, JsonRequestBehavior.AllowGet);
        }



        /*action for submit like through ajax*/
        // [HttpPost]

        public PartialViewResult SubmitLike(bool like, string controllerNAME = "default", int PostID = -1)
        {
            CommonWealEntities db = new CommonWealEntities();
            if (PostID != -1)
            {


                /*login user property defined in base controller*/
                /*checking is current login user already liked the image or not */
                var currentLikeUser = db.PostLikes.Where(pstlike => pstlike.PostID == PostID & pstlike.LoginID == LoginUser.LoginID).FirstOrDefault();
                db.Configuration.ValidateOnSaveEnabled = false;
                if (currentLikeUser == null)
                {
                    /*if not like than add row in post  */
                    PostLike pl = new PostLike();
                    pl.CreatedOn = DateTime.Now;
                    pl.ModifiedOn = DateTime.Now;
                    pl.IsLike = true;
                    /*login user property defined in base controller*/
                    pl.LoginID = LoginUser.LoginID;
                    pl.PostID = PostID;
                    db.PostLikes.Add(pl);
                    /*update like count */
                    var post = db.NGOPosts.Where(ngpost => ngpost.PostID == PostID).FirstOrDefault();
                    post.PostLikeCount++;
                    db.SaveChanges();
                }
                /*if already liked by user than remove like row of user for unlike */
                else
                {
                    //  var removeLike = db.PostLikes.Where(pstlike => pstlike.PostID == PostID & pstlike.LoginID == LoginUser.LoginID).FirstOrDefault();
                    db.PostLikes.Remove(currentLikeUser);
                    var post = db.NGOPosts.Where(ngpost => ngpost.PostID == PostID).FirstOrDefault();
                    post.PostLikeCount--;

                    db.SaveChanges();
                }
            }
            var postlikelist = new Post();
            postlikelist.postlike = getLikeList(PostID);
            var postUser = db.NGOPosts.Where(x => x.PostID == PostID).FirstOrDefault().LoginID;
            if (postUser != null)
            {
                postlikelist.userId = postUser.Value;
            }
            postlikelist.postId = PostID;

            postlikelist.controllername = controllerNAME;
            return PartialView("../UserHome/_LikePartial", postlikelist);
        }

        /*action for getting like list of particular post*/
        [AllowAnonymous]
        [HttpPost]
        public List<PostLikeModel> getLikeList(int postid)
        {
            CommonWealEntities db = new CommonWealEntities();

            List<PostLikeModel> postlikelist = new List<PostLikeModel>();

            var likeuserlist = db.PostLikes.Where(pstlike => pstlike.PostID == postid).ToList();
            foreach (var item in likeuserlist)
            {
                /*getting post like list and filling in post like model list */
                PostLikeModel plm = new PostLikeModel();
                int userType = db.Users.Where(user => user.LoginID == item.LoginID).FirstOrDefault().LoginUserType;

                switch (userType)
                {
                    case 1: string NGOUser = db.NGOUsers.Where(ngusr => ngusr.LoginID == item.LoginID).FirstOrDefault().NGOName.ToString();
                        plm.userName = NGOUser;
                        break;
                    case 3: var RegUser = db.RegisteredUsers.Where(lgnuser => lgnuser.LoginID == item.LoginID).FirstOrDefault();
                        plm.userName = RegUser.FirstName + " " + RegUser.LastName;
                        break;

                }
                plm.postId = postid;
                plm.userImageUrl = "";
                plm.UserID = item.LoginID;
                postlikelist.Add(plm);

            }


            return postlikelist;
        }

        /*like list throgh ajax*/
        public JsonResult getLikeListAjax(int postid = -1)
        {
            return Json(getLikeList(postid), JsonRequestBehavior.AllowGet);
        }


        /*method for getting next slot of posts on click of load more button*/
        //[HttpPost]
        [AllowAnonymous]
        public PartialViewResult onLoadPost(int[] category, string controller = "", int count = 0, int NgoID = 0)
        {
            try
            {
                var load = new List<Post>();
                CommonWealEntities db = new CommonWealEntities();
                dbOperations ob = new dbOperations();
                if (NgoID == 1)
                {
                    NgoID = LoginUser.LoginID;
                    load = ob.GetPostOnSeeMore(category, count, NgoID);
                }
                else
                {
                    load = ob.GetPostOnSeeMore(category, count, NgoID);
                }
                if (load.Count() > 0)
                {
                    for (int i = 0; i < load.Count(); i++)
                        load[i].controllername = controller;
                }
                /*returing list to  partial view and than partial view is retuned to ajax call  */
                return PartialView("~/views/userHome/_Posts.cshtml", load);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /*action for getting  post category wise*/
        [AllowAnonymous]
        public PartialViewResult GetPostByCategory2(int[] category)
        {
            var load = new List<Post>();
            load = null;
            if (category != null && category.Count() > 0)
            {
                dbOperations ob = new dbOperations();
                CommonWealEntities db = new CommonWealEntities();
                //var list = db.WorkingAreas.Where(x => x.CategoryID == category).ToList();
                // var post = db.NGOPosts.Include

                //var query = objEntities.Employee.Join(objEntities.Department, r => r.EmpId, p => p.EmpId, (r,p) => new{r.FirstName, r.LastName, p.DepartmentName});
                //var query = db.NGOPosts.Join(db.PostCategories, ngoPost => ngoPost.PostID, postCategories => postCategories.PostID, (r, p) => new { r }).ToList();
                // var query1 = db.NGOPosts.Include(x => x.PostCategories).Where(w => w.PostCategories.Where(m => m.CategoryID == category).Any()).ToList();
                load = ob.GetPostByCategory1(category);


            }

            return PartialView("~/views/userHome/_Posts.cshtml", load);
        }

        [AllowAnonymous]
        public JsonResult getpostCount()
        {
            var result1 = BaseController.pageleft;
            return Json(result1, JsonRequestBehavior.AllowGet);
        }



        public JsonResult deletePost(int ID)
        {
            bool result = false;
            try
            {
                CommonWealEntities context = new CommonWealEntities();
                context.Configuration.ValidateOnSaveEnabled = false;
                //var res1 = context.PostLikes.Where(a => a.PostID == ID).ToList();
                //if (res1 != null)
                //{
                //    foreach (var item in res1)
                //    {
                //        context.PostLikes.Remove(item);
                //    }

                //}
                //var res2 = context.PostComments.Where(a => a.PostID == ID).ToList();
                //if (res2 != null)
                //{
                //    foreach (var item in res2)
                //    {
                //        context.PostComments.Remove(item);
                //    }
                //}
                //var res3 = context.PostCategories.Where(a => a.PostID == ID).ToList();
                //if (res3 != null)
                //{
                //    foreach (var item in res3)
                //    {
                //        context.PostCategories.Remove(item);
                //    }

                //}
                var res = context.NGOPosts.Where(a => a.PostID == ID).FirstOrDefault();
                if (res != null)
                {
                    res.Isdelete = true;
                }

                context.SaveChanges();
                result = true;
            }
            catch (Exception)
            {

                throw;
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        /*action for delete comment through ajax*/
        public JsonResult DeleteCommentOnPost(int id = 0)
        {
            bool result = false;
            CommonWealEntities context = new CommonWealEntities();
            context.Configuration.ValidateOnSaveEnabled = false;
            var res2 = context.PostComments.Where(a => a.CommentID == id).FirstOrDefault();

            if (res2 != null)
            {

                //  context.PostComments.Remove(res2);

                res2.IsDelete =true;
                var ob = context.SpamUsers.Where(x=>x.CommentID==id).FirstOrDefault();
                if (ob != null)
                {
                    context.SpamUsers.Remove(ob);
                }
                context.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /*method for submit abused users on comment */
        //[HttpPost]
        [AllowAnonymous]
        public JsonResult AbuseUser(int CommentId = 0)
        {
            try
            {

                bool result = false;
                dbOperations ob = new dbOperations();
                if (CommentId > 0)
                {
                    result = ob.abuseUser(CommentId);

                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [AllowAnonymous]
        /*action for search ngo  through ajax*/
        public JsonResult SearchNGO(string name = "", int country = 0, int state = 0, int city = 0)
        {
            var result = new List<NGoSearchModal>();
            CommonWealEntities context = new CommonWealEntities();
            if (country > 0 && state > 0 && city > 0 && name == "")
            {
                result = context.NGOUsers.Where(x => x.NGOName.ToLower().Contains(name.ToLower()) && x.country == country && x.State == state && x.city == city).Select(x => new NGoSearchModal { loginID = x.LoginID.Value, name = x.NGOName }).ToList();
            }
            else if (country > 0 && state > 0 && city > 0)
            {
                result = context.NGOUsers.Where(x => x.NGOName.ToLower().Contains(name.ToLower()) && x.country == country && x.State == state && x.city == city).Select(x => new NGoSearchModal { loginID = x.LoginID.Value, name = x.NGOName }).ToList();

            }
            else if (country > 0 && state > 0 && city == 0)
            {
                result = context.NGOUsers.Where(x => x.NGOName.ToLower().Contains(name.ToLower()) && x.country == country && x.State == state).Select(x => new NGoSearchModal { loginID = x.LoginID.Value, name = x.NGOName }).ToList();
            }
            else if (country > 0 && state == 0 && city == 0)
            {
                result = context.NGOUsers.Where(x => x.NGOName.ToLower().Contains(name.ToLower()) && x.country == country).Select(x => new NGoSearchModal { loginID = x.LoginID.Value, name = x.NGOName }).ToList();
            }
            else
            {


                result = context.NGOUsers.Where(x => x.NGOName.ToLower().Contains(name.ToLower())).Select(x => new NGoSearchModal { loginID = x.LoginID.Value, name = x.NGOName }).ToList();
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        /*modal for ngo search key value*/
        public class NGoSearchModal
        {

            public int loginID { get; set; }
            public string name { get; set; }

        }
        public PartialViewResult RenderDonateDetails(int id)
        {
            dbOperations db = new dbOperations();
            var postDetails= db.GetPostDetailwithDonateById(id);
            return PartialView("_RenderDonateDetails", postDetails);
        }
    }
}
