using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Karrent.Enums;
using Karrent.Objects;
using Karrent.Views;

namespace Karrent
{
    class CurrentUser
    {
        public User User { get; set; }
        public MainWindow MainWindow { get; set; }

        protected static CurrentUser _instance;

        protected CurrentUser() 
        {
            SetUserAsGuest(); 
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
            this.MainWindow.setButtonsAfterLogin();
        }

        public void SetUserAsGuest()
        {
            User = new User(0, UserTypes.Guest, String.Empty, String.Empty, String.Empty, String.Empty, null, true, null);
        }
    }
}
