using railway.database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using railway.dto.login_dto;
using railway.model;

namespace railway.services
{
    public class LoginService
    {
        public static bool isExistUser(string username)
        {
            using(var db=new RailwayContext())
            {
                var user = (from u in db.users
                           where u.Username == username
                           select u).FirstOrDefault();
                if(user == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static User logIn(LoginDTO dto)
        {
            using (var db = new RailwayContext())
            {
               var user = (from u in db.users
                           where u.Username == dto.username & u.Password == dto.password
                           select u).FirstOrDefault();

                return (User)user;
            }
            
        }
    }
}
