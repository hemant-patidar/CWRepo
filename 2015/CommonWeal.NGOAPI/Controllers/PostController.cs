﻿using CommonWeal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using CommonWeal.NGOWeb;
using System.Threading.Tasks;
using System.IO;
using System.Web;

namespace CommonWeal.NGOAPI.Controllers
{
    [AllowAnonymous]
    public class PostController : BaseController
    {
        public class Comment
        {

            public string Username { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public string commentContent { get; set; }           
            public string commentUserImage { get; set; }
            public int commentId { get; set; }
            public int PostID{get;set;}
            public int UserID { get; set; }

        }
        public class PostLikeModel
        {
            public string userName { get; set; }
            public string userImageUrl { get; set; }
            public int UserID { get; set; }
            public int LoginID{get;set;}
            public int PostID{get;set;}
            public bool like { get;set;}
        }
        public class Post
        {
            public int userId { get; set; }
            public int postId { get; set; }
            public string userName { get; set; }
            public string userImage { get; set; }
            public DateTime postCreateTime { get; set; }
            public string postImageUrl { get; set; }
            public int likeCount { get; set; }
            public int commentCount { get; set; }
            public string postcontent { get; set; }
            public List<PostLikeModel> postlike { get; set; }
            public List<Comment> PostComments { get; set; }
        }
        CommonWealEntities context = new CommonWealEntities();
        //public class GetLikeComment
        //{
        //    public int postID { get; set; }
        //    public List<string> commentContent { get; set; }
        //}
        //public HttpResponseMessage specificComment(GetLikeComment objget)
        //{
        //    var res = context.NGOPosts.Where(a => a.PostID == objget.postID).FirstOrDefault();
        //    var res1 = context.PostComments.Where(x => x.PostID == res.PostID).FirstOrDefault().CommentText;
        //    objget.commentContent=
        //    var res2=context.RegisteredUsers.Where(x=>x.LoginID==res1)
        //    var res3= context.PostLikes.Where(x=>x.PostID==res.PostID).FirstOrDefault().
        //}
        public class postData
        {
            public string content { get; set; }
            public string file { get; set; }
            public int loginID { get; set; }
            public int[] cat { get; set; }
        }
        
        /*post for mobile/IOS*/
        [HttpPost]
        public bool NGOPost(postData postvalue )
        {
            NGOPost obpost = new NGOPost();
            dbOperations ob = new dbOperations();
            CommonWealEntities db = new CommonWealEntities();
            if (postvalue.file != null)
            {
                byte[] imageBytes = Convert.FromBase64String(postvalue.file);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                var abc = Guid.NewGuid();
                string path = "/Images/Post/" + abc + ".jpg";
                string filepath = HttpContext.Current.Server.MapPath(path);
                image.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
                obpost.PostUrl = path;                             
                obpost.LoginID = postvalue.loginID;
                obpost.PostType = "Image";
                obpost.PostDateTime = System.DateTime.Now;
                obpost.ModifiedOn = System.DateTime.Now;
                obpost.CreatedOn = System.DateTime.Now;
                obpost.PostCommentCount = 0;
                obpost.PostLikeCount = 0;
                obpost.PostContent = postvalue.content;
              
               
                db.NGOPosts.Add(obpost);
                db.SaveChanges();
                ob.SubmitPostCategory(obpost.PostID, postvalue.cat);
                return true;
            }
            /*if only content is posted without any image*/
            else if (postvalue.content != null)
            {
                /*login user login id is a property in base controller
                which contain current user information from user table*/
                obpost.LoginID = postvalue.loginID;
                //this.User.Identity.Name;
                obpost.PostType = "Text";
                obpost.PostDateTime = DateTime.Now;
                obpost.ModifiedOn = DateTime.Now;
                obpost.CreatedOn = DateTime.Now;
                obpost.PostCommentCount = 0;
                obpost.PostLikeCount = 0;
                obpost.PostContent = postvalue.content;
                db.NGOPosts.Add(obpost);
                db.SaveChanges();
                ob.SubmitPostCategory(obpost.PostID,postvalue.cat);
                return true;
            }
            return false;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> AddFile()
        {

            if (!Request.Content.IsMimeMultipartContent())
            {
                this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            if (Request.Content.IsMimeMultipartContent())
            {

                string StoragePath = HttpContext.Current.Server.MapPath("~/Images");
                var streamProvider = new MultipartFormDataStreamProvider(Path.Combine(StoragePath, "Post"));
                await Request.Content.ReadAsMultipartAsync(streamProvider);
                foreach (MultipartFileData fileData in streamProvider.FileData)
                {
                    if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                    {
                        return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
                    }
                    string fileName = fileData.Headers.ContentDisposition.FileName;
                    if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                    {
                        fileName = fileName.Trim('"');
                    }
                    if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                    {
                        fileName = Path.GetFileName(fileName);
                    }
                    File.Move(fileData.LocalFileName, Path.Combine(StoragePath, fileName));
                }
                return Request.CreateResponse(HttpStatusCode.OK, StoragePath);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
            }
        }
    

        [HttpGet]
        public HttpResponseMessage AreaOfInterest()
        {
            CommonWealEntities context = new CommonWealEntities();
            context.Configuration.LazyLoadingEnabled = false;
            var res = context.AreaOfInterests.ToList();
            var response = Request.CreateResponse(HttpStatusCode.OK, res);
            return response;
        }
        [HttpGet]
        public HttpResponseMessage GetOnLoad()
        {
            dbOperations db = new dbOperations();
            var res = db.GetPostOnLoad();
            var response = Request.CreateResponse(HttpStatusCode.OK, res);
            return response;

        }
        public class showCat
        {
            public int[] cat { get; set; }
        }
        [HttpPost]
        public HttpResponseMessage ShowCategory(showCat objcat)
        {
            dbOperations db = new dbOperations();
            var res = db.GetPostByCategory1(objcat.cat);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created,res);
            return response;
        }
        public class postOnSeeMore
        {
            public int[] category { get; set; }
            public int pageNum { get; set; }
            public int NgoID { get; set; }
        }
        [HttpPost]
        public HttpResponseMessage GetPostOnSeeMore(postOnSeeMore objsee)
        {
            dbOperations db = new dbOperations();
            var res = db.GetPostOnSeeMore(objsee.category,objsee.pageNum,objsee.NgoID);
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, res);
            return response;       
        }

        public class GetLikeComment
        {
            public int postID { get; set; }
            public List<string> commentContent { get; set; }
        }

        /*method for getting like on particular post*/
        [HttpPost]
        public HttpResponseMessage LikePost(PostLikeModel objLike)
        {
            CommonWealEntities context = new CommonWealEntities();
            var RegUserlist = context.RegisteredUsers.ToList();            
            var NGOUserlist = context.NGOUsers.ToList();
            var LoginUserlist = context.Users.ToList();
            var res = context.PostLikes.Where(a => a.PostID == objLike.PostID).ToList();
            List<PostLikeModel> imageLikeList = new List<PostLikeModel>();
          //  var PostLikeList = PostLikeListmain.Where(pstlike => pstlike.PostID == item.PostID).ToList();

            int likecount = 0;
            foreach (var like in res)
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
                pl.PostID = like.PostID.Value;               
                pl.UserID = like.LoginID;
                imageLikeList.Add(pl);
                likecount++;
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, imageLikeList);
            return response;
        
    }
        /*method for getting comment on particular post*/
        [HttpPost]
         public HttpResponseMessage CommentPost(Comment objcomment)
        {
            var RegUserlist = context.RegisteredUsers.ToList();            
            var NGOUserlist = context.NGOUsers.ToList();
            var LoginUserlist = context.Users.ToList();
            var res = context.PostComments.Where(a => a.PostID == objcomment.PostID).ToList();            
                List<Comment> commentlist = new List<Comment>();
                    //pm.commentCount = postcomment.Count();
                    int commentcount = 0;
                    foreach (var a in res)
                    {
                        Comment cmnt = new Comment();
                        cmnt.commentId = a.CommentID;
                        cmnt.commentContent = a.CommentText;                      
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
                        cmnt.PostID=a.PostID.Value;
                        cmnt.UserID=a.LoginID.Value;
                        commentlist.Add(cmnt);
                        commentcount++;
                    }
                    var response = Request.CreateResponse(HttpStatusCode.OK, commentlist);
                    return response;               
            }        
        
        [HttpPost]
        public HttpResponseMessage CommentGET(int postID)
        {
            CommonWealEntities context = new CommonWealEntities();
            var res = context.PostComments.Where(a => a.PostID == postID).ToList();
            var response = Request.CreateResponse(HttpStatusCode.OK, res);
            return response;
        }
        [HttpPost]
        public HttpResponseMessage CommentsPOST(PostComment objPostComment)
        {
            CommonWealEntities context = new CommonWealEntities();
            if (objPostComment.LoginID > 0)
            {           
            context.PostComments.Add(objPostComment);
            context.SaveChanges();
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, context.PostComments);
            return response;
                
        }
        [HttpPost]
        public HttpResponseMessage SubmitLike(PostLikeModel objpostlike)
        {
            if (objpostlike.LoginID > 0)
            {
                int PostID = -1;
                PostID = objpostlike.PostID;

                if (PostID != -1)
                {
                    CommonWealEntities db = new CommonWealEntities();

                    /*login user property defined in base controller*/
                    /*checking is current login user already liked the image or not */
                    var currentLikeUser = db.PostLikes.Where(pstlike => pstlike.PostID == PostID & pstlike.LoginID == objpostlike.LoginID).FirstOrDefault();

                    if (currentLikeUser == null)
                    {
                        /*if not like than add row in post  */
                        PostLike pl = new PostLike();
                        pl.CreatedOn = DateTime.Now;
                        pl.ModifiedOn = DateTime.Now;
                        pl.IsLike = true;
                        /*login user property defined in base controller*/
                        pl.LoginID = objpostlike.LoginID;
                        pl.PostID = PostID;
                        db.PostLikes.Add(pl);
                        db.SaveChanges();
                        /*update like count */
                        var post = db.NGOPosts.Where(ngpost => ngpost.PostID == PostID).FirstOrDefault();
                        post.PostLikeCount++;
                        db.SaveChanges();
                    }
                    else
                    {/*if already liked by user than remove like row of user for unlike */
                        var removeLike = db.PostLikes.Where(pstlike => pstlike.PostID == PostID & pstlike.LoginID == objpostlike.LoginID).FirstOrDefault();
                        db.PostLikes.Remove(removeLike);
                        var post = db.NGOPosts.Where(ngpost => ngpost.PostID == PostID).FirstOrDefault();
                        post.PostLikeCount--;
                        db.SaveChanges();
                    }
                }
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, context.PostLikes);
            return response;
        }
    }
}
