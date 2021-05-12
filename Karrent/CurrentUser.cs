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
        public User User { get; set; }

        protected static CurrentUser _instance;

        protected CurrentUser() 
        {
            User = new User(0, UserTypes.Guest, String.Empty, String.Empty, String.Empty, String.Empty, null, true, null);        
        }

        public static CurrentUser GetInstance()
        {
            if (_instance == null)
                _instance = new CurrentUser();
            return _instance;
        }
        public void SetCredentials(User user)
        {
            this.User = user;
        }

        public void SetUserAsGuest()
        {
            User = new User(0, UserTypes.Guest, String.Empty, String.Empty, String.Empty, String.Empty, null, true, null);
        }
    }
}
