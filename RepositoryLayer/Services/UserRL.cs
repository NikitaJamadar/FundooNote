using DatabaseLayer;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooNoteContext;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL:IUserRL
    {
        FunduContext fundo;
        public IConfiguration Configuration { get; }

        //Creating constructor for initialization
        public UserRL(FunduContext fundo, IConfiguration configuration)
        {
            this.fundo = fundo;
            this.Configuration = configuration;
        }

        //Creating method to add user in database
        public User AddUser(UserPostModel user)
        {
            try
            {
                User user1 = new User();
                user1.userID = new User().userID;
                user1.firstName = user.firstName;
                user1.lastName = user.lastName;
                user1.email = user.email;
                user1.password = EncryptPassword(user.password);
                user1.registerdDate = DateTime.Now;
                user1.address = user.address;
                fundo.Users.Add(user1);
                fundo.SaveChanges();
                return user1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Creating method to encrypt the password which is entered by user
        public static string EncryptPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    return null;
                }
                else
                {
                    byte[] b = Encoding.ASCII.GetBytes(password);
                    string encrypted = Convert.ToBase64String(b);
                    return encrypted;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
