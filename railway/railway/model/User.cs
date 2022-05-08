using System;
using System.Collections.Generic;
using System.Text;

namespace railway.model
{
    public enum UserType{Manager, Client   }  // treci korisnik

    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public UserType UserType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Ticket> Tickets { get; set; }
        
    }
}
