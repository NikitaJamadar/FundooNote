using DatabaseLayer;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooNoteContext;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        //Creating method to login user
        public string LoginUser(string email, string password)
        {
            try
            {
               
                var result = fundo.Users.Where(u => u.email == email && u.password == password).FirstOrDefault();
                if (result == null)
                {
                    return null;
                }
                return GetJWTToken(email, result.userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Generate JwT token
        public static string GetJWTToken(string email, int UserID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email", email),
                    new Claim("userId",UserID.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
    }
}
