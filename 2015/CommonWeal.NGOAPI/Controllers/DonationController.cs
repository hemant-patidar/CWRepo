﻿using CommonWeal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

public class DonationData
{
    public string Image { get; set; }
    public string Description { get; set; }
    public int RequestNGOID { get; set; }
    //public string ItemName { get; set; }
    //public int  ItemCount { get; set; }
    public List<DonationDetail> donationdetaildata{get; set;}
}

namespace CommonWeal.NGOAPI.Controllers
{
    [AllowAnonymous]
    public class DonationController : BaseController
    {

        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "ASJ", "AJ" };
        //}

        [HttpPost]
        public bool NGODonationRequest(DonationData donationdata)
        {
            CommonWealEntities context = new CommonWealEntities();
            DonationRequest donationrequest = new DonationRequest();
            donationrequest.ImgeUrl = donationdata.Image;
            donationrequest.Description = donationdata.Description;
            donationrequest.RequestNGOId = donationdata.RequestNGOID;
            donationrequest.createdOn = DateTime.Now;
            donationrequest.Status = false;
            donationrequest.ItemCost = 0;

            context.DonationRequests.Add(donationrequest);
            context.SaveChanges();
            var ItemTypes = donationdata.donationdetaildata.Count();
            DonationDetail donationdetail = new DonationDetail();
            for (int i = 0; i < ItemTypes; i++)
            {
                donationdetail = donationdata.donationdetaildata[i];
                donationdetail.RequestID = donationrequest.RequestID;
                donationdetail.DonatedCount = 0;
                donationdetail.ItemRequire = donationdetail.ItemCount;
                //donationdetail.ItemName = donationdata.donationdetaildata.it
                //donationdetail.ItemCount = donationdata.ItemCount;
                donationdetail.createdOn = DateTime.Now;
                context.DonationDetails.Add(donationdetail);

            }
            NGOPost ngopost = new NGOPost();
            ngopost.CreatedOn = DateTime.Now;
            ngopost.PostDateTime = DateTime.Now;
            ngopost.Isdelete = false;
            ngopost.IsRequest = true;
            ngopost.PostContent = donationdata.Description;
            ngopost.LoginID = donationdata.RequestNGOID;
            ngopost.PostCommentCount = 0;
            ngopost.PostLikeCount = 0;
            ngopost.RequestID = donationrequest.RequestID;
            context.NGOPosts.Add(ngopost);
            context.SaveChanges();
            var res = "yes";
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, res);
            return true;
        }



        public class UserDonationData
        {
            public int donatecount { get; set; }
            public int ItemID { get; set; }
            public int UserLoginID { get; set; }
            public int RequestId { get; set; }
        }
        public class DonarDetailData
        {
            public List<UserDonationData> donardetailvalue { get; set; }
        }

        [HttpPost]
        public bool UserDonation(DonarDetailData donardetaildata)
        {
            CommonWealEntities context = new CommonWealEntities();
            DonarDetail donardetail = new DonarDetail();
            var result = false;
           var count = donardetaildata.donardetailvalue.Count;
            for (int i = 0; i < count; i++)
            {
                donardetail.ItemID = donardetaildata.donardetailvalue[i].ItemID;
                donardetail.DonarLoginID = donardetaildata.donardetailvalue[i].UserLoginID;
                donardetail.createdOn = DateTime.Now;
              
                // donardetail.RequestId = donardetaildata.donardetailvalue[i].RequestId;
               
                donardetail.Donatecount = donardetaildata.donardetailvalue[i].donatecount;
                var donationdetail = context.DonationDetails.Where(w => w.ItemID == donardetail.ItemID).FirstOrDefault();
                donardetail.RequestId = donationdetail.RequestID;
                donationdetail.DonatedCount = donationdetail.DonatedCount + donardetaildata.donardetailvalue[i].donatecount;
                donationdetail.ItemRequire = donationdetail.ItemCount - donationdetail.DonatedCount;
                context.DonarDetails.Add(donardetail);
                context.SaveChanges();
                result = true;
              
            }


            // var res = "hello";
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            return result;
        }

        public class GetDonationData 
        {
            public List<DonationRequest> donationrequest { get; set; }
            public List<DonationDetail> dontiondetail { get; set; }
            public List<DonarDetail> donardetail { get; set; }
        }
        [HttpGet]
        public HttpResponseMessage GetDonationDetail()
        {
            CommonWealEntities context = new CommonWealEntities();
            var requestvalue = context.DonationRequests.ToList();
            GetDonationData getdonationdata = new GetDonationData();
            getdonationdata.donationrequest = requestvalue;
            var requestdetail = context.DonationDetails.ToList();
            getdonationdata.dontiondetail = requestdetail;
            var donar = context.DonarDetails.ToList();
            getdonationdata.donardetail = donar;
           
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, getdonationdata);
            return response;
        }
    }
}
