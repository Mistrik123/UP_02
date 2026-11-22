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
        List<Zakazi> zakazi;


        public ZakaziWindow(User currentUser)
        {
            InitializeComponent();

            this.currentUser = currentUser;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(currentUser.RoleId != 1)
            {
                AddZakaz_btn.Visibility = Visibility.Collapsed;
            }

            LoadZakazi();
        }

        private void LoadZakazi()
        {
            ZakaziList.Items.Clear();

            using (DBContext db = new DBContext())
            {
                zakazi = db.Zakazis.OrderBy(o => o.IdZakaz).ToList();
            }

            foreach(Zakazi zakaz in zakazi)
            {
                Grid mainGrid = GetElement(zakaz);
                ZakaziList.Items.Add(mainGrid);
            }
        }

        private Grid GetElement(Zakazi zakaz)
        {
            string statusName = "";
            string houseNumber = "";
            string streetName = "";
            string cityName = "";

            using (DBContext db = new DBContext())
            {
                statusName = db.Statuses.FirstOrDefault(f => f.Id == zakaz.StatusId).Name;
                
                Adress currentAdress = db.Adresses.FirstOrDefault(f => f.Id == zakaz.AdressVidahi);
                houseNumber = currentAdress.HouseNumber.ToString();

                Street currentStreet = db.Streets.FirstOrDefault(f => f.Id == currentAdress.StreetId);
                streetName = currentStreet.Name;

                cityName = db.Cities.FirstOrDefault(f => f.Id == currentStreet.CityId).Name;
            }

            Grid mainGrid = new Grid();
            mainGrid.Width = 300;

            StackPanel infoPanel = new StackPanel();
            infoPanel.HorizontalAlignment = HorizontalAlignment.Left;
            infoPanel.Width = 200;

            TextBlock number = new TextBlock();
            number.Text = "Заказ №" + zakaz.IdZakaz;
            number.FontWeight = FontWeights.Bold;

            TextBlock status = new TextBlock();
            status.Text = "Статус: " + statusName;

            TextBlock adress = new TextBlock();
            adress.Text = $"Адрес пункта выдачи: {cityName}, {streetName}, д. {houseNumber}";
            adress.TextWrapping = TextWrapping.Wrap;

            TextBlock date = new TextBlock();
            date.Text = "Дата заказа: " + zakaz.DateZakaza.ToString();

            infoPanel.Children.Add(number);
            infoPanel.Children.Add(status);
            infoPanel.Children.Add(adress);
            infoPanel.Children.Add(date);

            TextBlock dateDostavka = new TextBlock();
            dateDostavka.HorizontalAlignment = HorizontalAlignment.Right;
            dateDostavka.VerticalAlignment = VerticalAlignment.Center;
            dateDostavka.TextAlignment = TextAlignment.Center;
            dateDostavka.Text = "Доставка\n" + zakaz.DateDostavka.ToString();

            mainGrid.Children.Add(infoPanel);
            mainGrid.Children.Add(dateDostavka);

            return mainGrid;
        }

        private void AddZakaz_btn_Click(object sender, RoutedEventArgs e)
        {
            ZakazWindow zw = new ZakazWindow();
            zw.ShowDialog();

            LoadZakazi();
        }

        private void ZakaziList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ZakaziList.SelectedIndex > -1)
            {
                ZakazWindow zw = new ZakazWindow(zakazi[ZakaziList.SelectedIndex]);
                zw.ShowDialog();

                LoadZakazi();
            }
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            CatalogWindow cw = new CatalogWindow();
            cw.Show();
            Close();
        }
    }
}
