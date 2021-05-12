using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Karrent
{
    class ErrorBox
    {
        public static void Show(string message)
        {
            MessageBox.Show(message, "KarRent", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
