﻿using BusinessLayer.Interfaces;
using DatabaseLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.FundooNoteContext;
using System;
using System.Linq;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        FunduContext fundo;
        IUserBL userBL;
        public UserController(IUserBL userBL, FunduContext fundo)
        {
            this.userBL = userBL;
            this.fundo = fundo;
        }

        //HTTP method to handle registration user request
        [HttpPost("register")]
        public ActionResult RegisterUser(UserPostModel user)
        {
            try
            {
                var getUserData = fundo.Users.FirstOrDefault(u => u.email == user.email);
                if (getUserData != null)
                {
                    return this.Ok(new { success = false, message = $"{user.email} is Already Exists" });
                }
                this.userBL.AddUser(user);
                return this.Ok(new { success = true, message = $"Registration Successfull { user.email}" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //HTTP method to handle login user request
        [HttpPost("Login/{email}/{password}")]
        public ActionResult LoginUser(string email, string password)
        {
            try
            {
                var Id = fundo.Users.Where(x => x.email == email && x.password == password).FirstOrDefault();
                if (Id == null)
                {
                    return this.BadRequest(new { success = false, message = $"Invalid EmailId or Password" });
                }
                var result = this.userBL.LoginUser(email, password);
                if (result != null)
                {
                    return this.Ok(new { success = true, message = $"Login Successful {result}" });
                }
                return this.BadRequest(new { success = false, message = $"Login failed {result}" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //HTTP method to handle forgrt password request
        [HttpPost("ForgetPassword/{email}")]
        public ActionResult ForgetPassword(string email)
        {
            try
            {
                var result = this.userBL.ForgetPassword(email);
                if (result != false)
                {
                    return this.Ok(new { success = true, message = $"Mail sent successfully : {result}" });
                }
                return this.BadRequest(new { success = false, message = $"Failed to send mail : {result}" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //HTTP method to handle change password request
        [Authorize]
        [HttpPost("ChangePassword")]
        public ActionResult ChangePassword(PasswordValidation valid)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                int userId = Int32.Parse(userid.Value);
                var result = fundo.Users.Where(u => u.userID == userId).FirstOrDefault();
                string email = result.email.ToString();
                bool res = userBL.ChangePassword(email, valid);

                if (res == false)
                {
                    return this.BadRequest(new { success = false, message = "Enter valid password" });

                }
                else
                {
                    return this.Ok(new { success = true, message = "Password changed successfully" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
