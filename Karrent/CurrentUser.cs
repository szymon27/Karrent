using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karrent.Enums;
using Karrent.Objects;

namespace Karrent
{
    class CurrentUser
    {
        public int Id { get; set; } = 0;
        public UserTypes UserType { get; set; } = UserTypes.Guest;
        public string Username { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Surname { get; set; } = String.Empty;
        public DateTime? BirthDate { get; set; } = null;
        public bool IsActive { get; set; } = true;
        public DateTime? CreationDate { get; set; } = null;

        protected static CurrentUser _instance;

        protected CurrentUser() { }

        public static CurrentUser GetInstance()
        {
            if (_instance == null)
                _instance = new CurrentUser();
            return _instance;
        }
        public void SetCredentials(User user)
        {
            this.Id = user.Id;
            this.UserType = user.UserType;
            this.Username = user.Username;
            this.Password = user.Password;
            this.Name = user.Name;
            this.Surname = user.Surname;
            this.BirthDate = user.BirthDate;
            this.IsActive = user.IsActive;
            this.CreationDate = user.CreationDate;
        }

        public void SetUserAsGuest()
        {
            this.Id = 0;
            this.UserType = UserTypes.Guest;
            this.Username = String.Empty;
            this.Password = String.Empty;
            this.Name = String.Empty;
            this.Surname = String.Empty;
            this.BirthDate = null;
            this.IsActive = true;
            this.CreationDate = null;
        }
    }
}
