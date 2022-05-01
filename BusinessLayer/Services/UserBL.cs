using BusinessLayer.Interfaces;
using DatabaseLayer;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBL: IUserBL
    {
        IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        //Creating method to add user in database
        public User AddUser(UserPostModel user)
        {
            try
            {
                return this.userRL.AddUser(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
