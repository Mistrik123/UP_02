using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UP02.Models;

namespace UP02
{
    /// <summary>
    /// Логика взаимодействия для ZakaziWindow.xaml
    /// </summary>
    public partial class ZakaziWindow : Window
    {
        User currentUser;

        public ZakaziWindow(User currentUser)
        {
            InitializeComponent();

            this.currentUser = currentUser;
        }
    }
}
