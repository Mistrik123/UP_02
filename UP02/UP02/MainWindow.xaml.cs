using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UP02.Models;

namespace UP02
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        User currentUser;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LogBtn_Click(object sender, RoutedEventArgs e)
        {
            if (LoginLogTxt.Text.Length > 0 && PasswordLogTxt.Text.Length > 0)
            {
                using (DBContext db = new DBContext())
                {
                    currentUser = db.Users.FirstOrDefault(f => f.Login == LoginLogTxt.Text && f.Password == PasswordLogTxt.Text);
                }

                if (currentUser != null)
                {
                    CatalogWindow cw = new CatalogWindow(currentUser);
                    cw.Show();
                    Close();
                }
                else
                    MessageBox.Show("Неверный логин и/или пароль", "Вход", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
                MessageBox.Show("Введите логин и пароль", "Вход", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void RegBtn_Click(object sender, RoutedEventArgs e)
        {
            CatalogWindow cw = new CatalogWindow();
            cw.Show();
            Close();
        }
    }
}