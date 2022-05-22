using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace railway.model
{
    public enum UserType{Manager, Client} 

    public class User
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public UserType UserType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }


        public virtual List<Ticket> Tickets { get; set; }
        
    }
}
