using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karrent.Enums;
namespace Karrent.Objects
{
    class User
    {
        public int Id { get; set; }
        public UserTypes UserType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreationDate { get; set; }

        public User(int id, UserTypes userType, string username, string password, string name,
            string surname, DateTime? birthDate, bool isActive, DateTime? creationDate)
        {
            this.Id = id;
            this.UserType = userType;
            this.Username = username;
            this.Password = password;
            this.Name = name;
            this.Surname = surname;
            this.BirthDate = birthDate;
            this.IsActive = isActive;
            this.CreationDate = creationDate;
        }

        public override string ToString()
        {
            return $"Id:{this.Id} " +
                $"User type:{this.UserType} " +
                $"Username:{this.Username} " +
                $"Password:{this.Password} " +
                $"Name:{this.Name} " +
                $"Surname:{this.Surname} " +
                $"Birth date:{this.BirthDate:dd-MM-yyyy} " +
                $"Is active:{this.IsActive} " +
                $"Creation date:{this.CreationDate:dd-MM-yyyy}";
        }
    }
}
