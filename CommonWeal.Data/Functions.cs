﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;

namespace CommonWeal.Data
{
    public class Functions
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
    }
}