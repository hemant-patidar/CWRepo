﻿using CommonWeal.NGOWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using CommonWeal.NGOWeb.ViewModel;
using CommonWeal.Data;
using CommonWeal.NGOWeb.Controllers;

namespace CommonWeal.NGOWeb
{
    public class dbOperations
    {
        CommonWealEntities context = new CommonWealEntities();
        public List<NGOUser> GetAllUserNotAccepted()
        {
            List<NGOUser> userList = new List<NGOUser>();
            userList = context.NGOUsers.Where(w => w.IsActive == false && w.IsBlock == false).ToList();
            return userList;
        }
        public List<NGOUser> GetAllUserAccepted()
        {
            List<NGOUser> userList = new List<NGOUser>();
            userList = context.NGOUsers.Where(w => w.IsActive == true).ToList();
            //userList = context.NGOUsers.Include(x => x.User).Where(w => w.User.IsActive== false).ToList();
            return userList;
        }
        public List<NGOUser> GetAllUserBlocked()
        {
            List<NGOUser> userList = new List<NGOUser>();
            userList = context.NGOUsers.Where(w => w.IsBlock == true).ToList();
            return userList;
        }
        public NGOUser GetNGODetails(int id)
        {
            NGOUser ob = new NGOUser();
            ob = context.NGOUsers.Where(w => w.LoginID == id).FirstOrDefault();
            return ob;
        }
        public ForgotPassword UserDetail(string FinalOTP)
        {
            ForgotPassword ob = new ForgotPassword();
            ob = context.ForgotPasswords.Where(w => w.OTP == FinalOTP).FirstOrDefault();
            return ob;
        }
        public List<RegisteredUser> RegisteredUserIsAccepted()
        {
            List<RegisteredUser> userList = new List<RegisteredUser>();
            // userList = context.RegisteredUsers.ToList();
            userList = context.RegisteredUsers.Include(x => x.User).Where(w => w.User.IsActive == true).ToList();
            return userList;
        }
        public List<NGOUser> RegisteredNGOIsAccepted()
        {
            List<NGOUser> userList = new List<NGOUser>();
            // userList = context.RegisteredUsers.ToList();
            userList = context.NGOUsers.Include(x => x.User).Where(w => w.User.IsActive == true).ToList();
            return userList;
        }
        public List<User> UserIsAccepted()
        {
            List<User> userList = new List<User>();
            // userList = context.RegisteredUsers.ToList();
            userList = context.Users.Where(w => w.IsActive == true).ToList();
            return userList;
        }
        public List<NGOUser> WarnedNGOs()
        {
            List<NGOUser> NGOList = new List<NGOUser>();
            // userList = context.RegisteredUsers.ToList();
            NGOList = context.NGOUsers.Where(w => w.IsWarn == true).ToList();
            return NGOList;
        }
        //User Table
        public List<RegisteredUser> AllUsers()
        {
            List<RegisteredUser> userList = new List<RegisteredUser>();
            // userList = context.RegisteredUsers.ToList();
            userList = context.RegisteredUsers.Include(x => x.User).Where(w => w.User.IsActive == true && w.User.LoginUserType == 3).ToList();
            return userList;
        }
        public List<RegisteredUser> BlockedNormalUsers()
        {
            List<RegisteredUser> userList = new List<RegisteredUser>();
            userList = context.RegisteredUsers.Include(x => x.User).Where(w => w.User.IsBlock == true && w.User.LoginUserType == 3).ToList();
            return userList;
        }


        public string GenerateRandomPassword(int length)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-*&#+";
            char[] chars = new char[length];
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }
            return new string(chars);
        }
        public string SendActivationEmail(string UserEmail, string Randomcode)//int userId it should be used in the brackets
        {
            //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            string activationCode = Guid.NewGuid().ToString();

            var fromAddress = new MailAddress("commonweal9@gmail.com");
            var fromPassword = ".netgroup";
            var toAddress = new MailAddress(UserEmail);
            try
            {
                string subject = "Fassword Change";
                string body = Randomcode;

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = Randomcode
                })
                    smtp.Send(message);
            }
            catch (Exception ex)
            {

                // Response.Write("Exception in sendEmail:" + ex.Message);
            }
            return activationCode;
        }

        /*method for getting post by category selection*/
        public List<Post> GetPostByCategory1(int[] category)
        {
            var selectedlist = new List<PostWithCategory>();

            if (category != null && category.Count() > 0 && !category.Contains(1))
            {
                //var NGOPostList =context.NGOPosts.Include(x => x.PostCategories).Where(w => w.PostCategories.Where(m => m.CategoryID == category).Any()).OrderByDescending(x => x.CreatedOn).Take(5).ToList();
                var list = getPostwithcategoryList();
                //foreach (var res in list )
                {
                    selectedlist = list.Where(x => x.CategoryIdList.Where(w => category.Contains(w)).Any()).ToList();
                    
                }
            }
            else
            {
                selectedlist = getPostwithcategoryList().OrderByDescending(x => x.CreatedOn).ToList();
            }
            BaseController.pageleft = selectedlist.Count();
            selectedlist = selectedlist.OrderByDescending(x => x.CreatedOn).Take(5).ToList();
            var result = GetAllPost(selectedlist);
            return result.OrderByDescending(x => x.postCreateTime).ToList();
        }
        /*getting post by id*/
        public List<Post> GetPostById(int id)
        {
            //var NGOPostlist = context.NGOPosts.Where(x => x.LoginID == id).OrderByDescending(x => x.CreatedOn).Take(5).ToList();
            var list = getPostwithcategoryList().Where(x=>x.LoginID==id).ToList();
             BaseController.pageleft = list.Count();
             list = list.OrderByDescending(x => x.CreatedOn).Take(5).ToList();
            var result = GetAllPost(list);
            return result;
        }
       
        /*getting post on see more click*/
        public List<Post> GetPostOnSeeMore(int pageNum = 0, int category = 0,int NgoID=0)
        {
            var selectlist = new List<PostWithCategory>();
            //var NGOPostlist = context.NGOPosts.Include(x => x.PostCategories).Where(w => w.PostCategories.Where(m => m.CategoryID == category).Any()).OrderByDescending(x => x.CreatedOn).Skip(pageNum * 5).Take(5).ToList();
            var list = getPostwithcategoryList();
            if (NgoID == 0)
            {
                if (category > 1)
                {
                    selectlist = list.Where(x => x.CategoryID == category).ToList();
                    BaseController.pageleft = selectlist.Count();
                    selectlist = selectlist.OrderByDescending(x => x.CreatedOn).Skip(pageNum * 5).Take(5).ToList();

                }
                else
                {
                  
                    BaseController.pageleft =list.Count();
                    selectlist = list.OrderByDescending(x => x.CreatedOn).Skip(pageNum * 5).Take(5).ToList();
                }
            }
            else {
                selectlist = list.Where(x => x.LoginID == NgoID).ToList();
                BaseController.pageleft = selectlist.Count();
                selectlist = selectlist.OrderByDescending(x => x.CreatedOn).Skip(pageNum * 5).Take(5).ToList();
            }
            var result = GetAllPost(selectlist);
            return result;
        }


        /*getting post category*/
        public List<Post> GetPostCategory(int category)
        {


            //   var List = from g in context.PostCategories
            //join u in context.NGOPosts on g.PostID equals u.PostID
            //select new { g, u, });
            var list = getPostwithcategoryList();
            //context.PostCategories.Where(x => x.CategoryID == item).Select(x => x.PostID).ToList();
            // arrayList.Add(List);
            // return list;
            var selectedlist = list.Where(x => x.CategoryID == category).ToList();
            var result = GetAllPost(selectedlist);
            //var cat = list[0].CategoryID;
            // var pos = list[0].u.PostID;
            return result;
        }
        /*getting post on 1st time page loading*/
        public List<Post> GetPostOnLoad()
        {
            var NGOPostlist = context.NGOPosts.OrderByDescending(x => x.CreatedOn).Take(5).ToList();
            var list = getPostwithcategoryList();
            BaseController.pageleft = list.Count();
            var selectList = list.OrderByDescending(x => x.CreatedOn).Take(5).ToList();
            var result = GetAllPost(selectList);
            return result;
        }
        /*method for join ngopost and postcategories table and select some column*/
        public List<PostWithCategory> getPostwithcategoryList()
        {

            var postList = context.NGOPosts.ToList();
            List<PostWithCategory> finalPostList = new List<PostWithCategory>();
            foreach (var item in postList)
            {
                PostWithCategory post = new PostWithCategory();
                post.CreatedOn = item.CreatedOn;
                post.PostCommentCount = item.PostCommentCount;
                post.LoginID = item.LoginID;
                post.PostLikeCount = item.PostLikeCount.Value;
                post.PostUrl = item.PostUrl;
                post.PostContent = item.PostContent;
                post.PostID = item.PostID;

                List<string> categoryNames = new List<string>();
                post.CategoryIdList = new List<int>();
                foreach (var category in item.PostCategories)
                {
                    post.CategoryIdList.Add(category.AreaOfInterest.CategoryID);
                    categoryNames.Add(category.AreaOfInterest.CategoryName);
                }
                post.CategoryList = categoryNames;
                finalPostList.Add(post);

            }
            //var list = (from g in context.PostCategories
            //            from u in context.NGOPosts.Where(a => a.PostID == g.PostID).DefaultIfEmpty()
            //            from pc in context.PostCategories.Where(w => w.PostID == g.PostID).Select(s => s.CategoryID)
            //            //from cat in context.AreaOfInterests.Where(w=>w.CategoryID==1)).FirstOrDefault()
            //            select new PostWithCategory()
            //            {
            //                PostUrl = u.PostUrl,
            //                PostType = u.PostType,
            //                PostContent = u.PostContent,
            //                PostCommentCount = u.PostCommentCount != null ? u.PostCommentCount.Value : 0,
            //                PostLikeCount = u.PostLikeCount.Value,
            //                CreatedOn = u.CreatedOn.Value,
            //                ModifiedOn = u.ModifiedOn.Value,
            //                PostDateTime = u.PostDateTime.Value,
            //                PostID = u.PostID,
            //                LoginID = u.LoginID.Value,
            //                CategoryID = g.CategoryID,
            //            }).ToList();
            return finalPostList;


        }
        /*Getting set of Post in  List form */
        public List<Post> GetAllPost(List<PostWithCategory> list)
        {
            /*Make List of custom post model type */
            List<Post> ob = new List<Post>();
            /*Getting list from all table */
            //List<NGOPost> NGOPostlist=new List<NGOPost>();
            //if (category > 1)
            //{
            //    NGOPostlist = context.NGOPosts.Include(x => x.PostCategories).Where(w => w.PostCategories.Where(m => m.CategoryID == category).Any()).OrderByDescending(x=>x.CreatedOn).Skip(pageNum * 5).Take(5).ToList();
            //}
            //else
            //{
            //    NGOPostlist = context.NGOPosts.OrderByDescending(x => x.CreatedOn).Skip(pageNum * 5).Take(5).ToList();
            //}

            var RegUserlist = context.RegisteredUsers.ToList();
            var Commentlist = context.PostComments.ToList();
            var NGOUserlist = context.NGOUsers.ToList();
            var LoginUserlist = context.Users.ToList();
            var PostLikeListmain = context.PostLikes.ToList();
            if (list != null)
            {


                foreach (var item in list)
                {

                    // var RegUser =RegUserlist.Where(regusr=>regusr.UserEmail==item.EmailID);
                    var Comment = Commentlist.Where(comment => comment.PostID == item.PostID);


                    Post pm = new Post();
                    pm.userId = item.LoginID.Value;

                    int UserRole = LoginUserlist.Where(user => user.LoginID == item.LoginID).Select(x => x.LoginUserType).FirstOrDefault();

                    switch (UserRole)
                    {
                        case 1: string NGOUser = NGOUserlist.Where(ngusr => ngusr.LoginID == item.LoginID).FirstOrDefault().NGOName.ToString();
                            pm.userName = NGOUser;
                            break;
                        case 3: var RegUser = RegUserlist.Where(lgnuser => lgnuser.LoginID == item.LoginID).FirstOrDefault();
                            pm.userName = RegUser.FirstName + " " + RegUser.LastName;
                            break;

                    }

                    pm.categoryName = "";
                    foreach (var cat in item.CategoryList)
                    {
                        pm.categoryName = pm.categoryName + "" + cat;
                    }
                    // pm.categoryName = context.AreaOfInterests.Where(a => a.CategoryID == item.CategoryID).FirstOrDefault().CategoryName;
                    pm.postcontent = item.PostContent;
                    pm.postImageUrl = item.PostUrl;
                    pm.postCreateTime = item.CreatedOn.Value;
                    pm.likeCount = item.PostLikeCount;
                    pm.commentCount = item.PostCommentCount.Value;
                    pm.postId = item.PostID;
                    /*getting list of like user of curent post*/
                    var postcomment = Commentlist.Where(Cmntlst => Cmntlst.PostID == item.PostID).ToList();

                    //start like list
                    List<PostLikeModel> imageLikeList = new List<PostLikeModel>();
                    var PostLikeList = PostLikeListmain.Where(pstlike => pstlike.PostID == item.PostID).ToList();

                    int likecount = 0;
                    foreach (var like in PostLikeList)
                    {

                        PostLikeModel pl = new PostLikeModel();
                        int userType = LoginUserlist.Where(user => user.LoginID == like.LoginID).FirstOrDefault().LoginUserType;

                        switch (userType)
                        {
                            case 1: string NGOUser = NGOUserlist.Where(ngusr => ngusr.LoginID == like.LoginID).FirstOrDefault().NGOName.ToString();
                                pl.userName = NGOUser;
                                break;
                            case 3: var RegUser = RegUserlist.Where(lgnuser => lgnuser.LoginID == like.LoginID).FirstOrDefault();
                                pl.userName = RegUser.FirstName + " " + RegUser.LastName;
                                break;

                        }
                        pl.userImageUrl = "";
                        pl.UserID = like.LoginID;

                        imageLikeList.Add(pl);
                        likecount++;

                    }
                    pm.likeCount = item.PostLikeCount;
                    pm.postlike = imageLikeList;
                    //end like list

                    // start all comment of  particular post
                    List<Comment> imagecommentlist = new List<Comment>();
                    pm.commentCount = postcomment.Count();
                    int commentcount = 0;
                    foreach (var a in postcomment)
                    {
                        Comment cmnt = new Comment();
                        cmnt.commentId = a.CommentID;
                        cmnt.commentContent = a.CommentText;
                        cmnt.commentLike = 0;
                        cmnt.commentUserImage = "";
                        cmnt.CreatedDateTime = a.CreatedOn.Value;
                        int userType = LoginUserlist.Where(user => user.LoginID == a.LoginID).FirstOrDefault().LoginUserType;

                        switch (userType)
                        {
                            case 1: string NGOUser = NGOUserlist.Where(ngusr => ngusr.LoginID == a.LoginID).FirstOrDefault().NGOName.ToString();
                                cmnt.Username = NGOUser;
                                break;
                            case 3: var RegUser = RegUserlist.Where(lgnuser => lgnuser.LoginID == a.LoginID).FirstOrDefault();
                                cmnt.Username = RegUser.FirstName + " " + RegUser.LastName;
                                break;

                        }

                        imagecommentlist.Add(cmnt);
                        commentcount++;
                    }
                    pm.commentCount = item.PostCommentCount.Value;
                    //end all comment of  particular post
                    pm.PostComments = imagecommentlist;

                    pm.postCategoryNameList = item.CategoryList;

                    ob.Add(pm);
                }
            }
            return (ob);
        }


        /*getting all categoryname from areaofintrest table*/
        public List<AreaOfInterest> GetAllCategory()
        {
            CommonWealEntities db = new CommonWealEntities();
            var categoryList = db.AreaOfInterests.ToList();
            return categoryList;

        }
        /*method for submit area of intrest by ngouser from  edit profile*/
        public bool SubmitCategory(List<WorkingArea> WAlist)
        {
            CommonWealEntities db = new CommonWealEntities();
            foreach (var item in WAlist)
            {
                db.WorkingAreas.Add(item);
            }

            db.SaveChanges();
            return true;

        }
        /*method for submit category for which post is uploded and will search by*/
        public void SubmitPostCategory(int postid, int[] category)
        {
            try
            {
                CommonWealEntities db = new CommonWealEntities();
                if (category.Length > 0)
                {
                    foreach (var item in category)
                    {

                        PostCategory pc = new PostCategory();
                        pc.PostID = postid;
                        pc.CategoryID = item;
                        db.PostCategories.Add(pc);



                    }
                    db.SaveChanges();
                }
                else
                {
                    PostCategory pc = new PostCategory();
                    pc.PostID = postid;
                    pc.CategoryID = 1;
                    db.PostCategories.Add(pc);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }
    }
}

