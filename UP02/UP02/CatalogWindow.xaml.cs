using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {
        User currentUser;
        Brush backgroundColor;

        List<Tovari> tovari = new List<Tovari>();

        public CatalogWindow(User currentUser)
        {
            InitializeComponent();

            this.currentUser = currentUser;
            backgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#23E1EF"));
        }

        public CatalogWindow()
        {
            InitializeComponent();

            backgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#23E1EF"));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentUser != null)
            {
                switch (currentUser.RoleId)
                {
                    case 1: //Админ
                        LoadFilters();
                        break;
                    case 2: //менеджер
                        Add_btn.Visibility = Visibility.Collapsed;
                        LoadFilters();
                        break;
                    case 3:// клиент
                        FilterPanel.Visibility = Visibility.Collapsed;
                        Add_btn.Visibility = Visibility.Collapsed;
                        Zakazi_btn.Visibility = Visibility.Collapsed;
                        break;
                }

                FIO.Text = currentUser.Famil + " " + currentUser.Name + " " + currentUser.Otchestvo;
            }
            else
            {
                FilterPanel.Visibility = Visibility.Collapsed;
                Add_btn.Visibility = Visibility.Collapsed;
                FIO.Visibility = Visibility.Collapsed;
                Zakazi_btn.Visibility = Visibility.Collapsed;
            }

            using(DBContext db = new DBContext())
            {
                tovari = db.Tovaris.ToList();
            }

            LoadList();
        }

        private void LoadFilters()
        {
            Sort.Items.Add("По умолчанию");
            Sort.Items.Add("По убыванию (Цена)");
            Sort.Items.Add("По возрастанию (Цена)");
            Sort.Items.Add("По убыванию (Остаток)");
            Sort.Items.Add("По возрастанию (Остаток)");

            Filter.Items.Add("Все диапазоны скидки");
            Filter.Items.Add("0-12,99%");
            Filter.Items.Add("13-16,99%");
            Filter.Items.Add("17% и более");

            Sort.SelectedIndex = 0;
            Filter.SelectedIndex = 0;
        }

        private void DeleteFilter_btn_Click(object sender, RoutedEventArgs e)
        {
            Sort.SelectedIndex = 0;
            Filter.SelectedIndex = 0;
            Search.Text = "";

            LoadList();
        }

        private void LoadList()
        {
            using (DBContext db = new DBContext())
            {
                tovari = db.Tovaris.ToList();

                switch (Filter.SelectedIndex)
                {
                    case 1:
                        tovari = tovari.Where(w => w.Skidka >= 0 && w.Skidka < 13).ToList();
                        break;
                    case 2:
                        tovari = tovari.Where(w => w.Skidka >= 13 && w.Skidka < 17).ToList();
                        break;
                    case 3:
                        tovari = tovari.Where(w => w.Skidka >= 17).ToList();
                        break;
                }

                if(Search.Text.Length > 0)
                {
                    tovari = tovari.Where(d => ((d.Name?.Contains(Search.Text) ?? false) || (d.Description?.Contains(Search.Text) ?? false))).ToList();
                }

                switch (Sort.SelectedIndex)
                {
                    case 1:
                        tovari = tovari.OrderByDescending(o => o.Price).ToList();
                        break;
                    case 2:
                        tovari = tovari.OrderBy(o => o.Price).ToList();
                        break;
                    case 3:
                        tovari = tovari.OrderByDescending(o => o.Count).ToList();
                        break;
                    case 4:
                        tovari = tovari.OrderBy(o => o.Count).ToList();
                        break;
                }
            }

            TovarList.Items.Clear();

            foreach (Tovari tovar in tovari)
            {
                Grid mainGrid = GetElement(tovar);
                TovarList.Items.Add(mainGrid);
            }
        }

        private Grid GetElement(Tovari tovar)
        {
            string categoryName = "";
            string proizvoditelName = "";
            string postavshikName = "";
            string edIzName = "";
            string photoName = "picture.png";
            int skidka = (int)tovar.Skidka;

            using (DBContext db = new DBContext())
            {
                categoryName = db.Categories.FirstOrDefault(f => f.Id == (int)tovar.CategoryId).Name;
                proizvoditelName = db.Proizvoditels.FirstOrDefault(f => f.Id == (int)tovar.ProizvoditelId).Name;
                postavshikName = db.Postavshiks.FirstOrDefault(f => f.Id == (int)tovar.PostavshikId).Name;
                edIzName = db.EdIzms.FirstOrDefault(f => f.Id == (int)tovar.EdIzmId).Name;
            }

            if (tovar.Photo.ToString().Length > 0)
                photoName = tovar.Photo;

            Grid mainPanel = new Grid();
            mainPanel.Width = 590;

            Image img = new Image();
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.Source = new BitmapImage(new Uri($"/Images/{photoName}", UriKind.Relative));
            img.Width = 150;

            StackPanel infoPanel = new StackPanel();
            infoPanel.HorizontalAlignment = HorizontalAlignment.Center;
            infoPanel.Width = 250;

            TextBlock catAndName = new TextBlock();
            catAndName.Text = $"{categoryName} | {tovar.Name}";
            catAndName.FontWeight = FontWeights.Bold;
            catAndName.TextWrapping = TextWrapping.Wrap;
            catAndName.Margin = new Thickness(0,0,0,10);

            TextBlock description = new TextBlock();
            description.Text = "Описание товара: " + tovar.Description;
            description.TextWrapping = TextWrapping.Wrap;
            description.Margin = new Thickness(0, 0, 0, 10);

            TextBlock proizvoditel = new TextBlock();
            proizvoditel.Text = "Производитель: " + proizvoditelName;
            proizvoditel.TextWrapping = TextWrapping.Wrap;
            proizvoditel.Margin = new Thickness(0, 0, 0, 10);

            TextBlock postavshik = new TextBlock();
            postavshik.Text = "Поставщик: " + postavshikName;
            postavshik.TextWrapping = TextWrapping.Wrap;
            postavshik.Margin = new Thickness(0, 0, 0, 10);

            WrapPanel pricePanel = new WrapPanel();
            pricePanel.Margin = new Thickness(0, 0, 0, 10);

            TextBlock titlePrice = new TextBlock();
            titlePrice.Text = "Цена: ";

            TextBlock priceOld = new TextBlock();
            priceOld.Text = tovar.Price.ToString("F2");

            TextBlock priceNew = new TextBlock();
            priceNew.Visibility = Visibility.Collapsed;

            pricePanel.Children.Add(titlePrice);
            pricePanel.Children.Add(priceOld);
            pricePanel.Children.Add(priceNew);

            TextBlock edIzm = new TextBlock();
            edIzm.Text = "Единица измерения: " + edIzName;
            edIzm.Margin = new Thickness(0, 0, 0, 10);

            TextBlock count = new TextBlock();
            count.Text = "Количество на складе: " + tovar.Count;
            count.Margin = new Thickness(0, 0, 0, 10);

            infoPanel.Children.Add(catAndName);
            infoPanel.Children.Add(description);
            infoPanel.Children.Add(proizvoditel);
            infoPanel.Children.Add(postavshik);
            infoPanel.Children.Add(pricePanel);
            infoPanel.Children.Add(edIzm);
            infoPanel.Children.Add(count);

            mainPanel.Children.Add(img);
            mainPanel.Children.Add(infoPanel);

            if (skidka > 0)
            {
                Grid skidkaPanel = new Grid();
                skidkaPanel.VerticalAlignment = VerticalAlignment.Stretch;
                skidkaPanel.HorizontalAlignment = HorizontalAlignment.Right;
                skidkaPanel.Width = 150;

                TextBlock skidkaText = new TextBlock();
                skidkaText.Text = skidka + "%";
                skidkaText.FontSize = 18;
                skidkaText.FontWeight = FontWeights.Bold;
                skidkaText.VerticalAlignment = VerticalAlignment.Center;
                skidkaText.HorizontalAlignment = HorizontalAlignment.Center;

                skidkaPanel.Children.Add(skidkaText);

                mainPanel.Children.Add(skidkaPanel);

                if (skidka > 25)
                    skidkaPanel.Background = backgroundColor;

                priceOld.Foreground = Brushes.Red;
                priceOld.TextDecorations = TextDecorations.Strikethrough;

                double raznitsa = Convert.ToDouble(tovar.Price / 100) * Convert.ToDouble(skidka);
                double newPrice = Convert.ToDouble(tovar.Price) - raznitsa;
                priceNew.Text = newPrice.ToString("F2");
                priceNew.Visibility = Visibility.Visible;
            }

            if (tovar.Count == 0)
                mainPanel.Background = Brushes.Gray;

            return mainPanel;
        }

        private void Back_btn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wm = new MainWindow();
            wm.Show();
            Close();
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadList();
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadList();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadList();
        }

        private void TovarList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(currentUser.RoleId == 1 && TovarList.SelectedIndex > -1)
            {
                Tovari currentTovar = tovari[TovarList.SelectedIndex];
                TovarWindow tw = new TovarWindow(currentTovar);
                tw.ShowDialog();

                LoadList();
            }
        }

        private void Add_btn_Click(object sender, RoutedEventArgs e)
        {
            TovarWindow tw = new TovarWindow();
            tw.ShowDialog();

            LoadList();
        }

        private void Zakazi_btn_Click(object sender, RoutedEventArgs e)
        {
            ZakaziWindow zw = new ZakaziWindow(currentUser);
            zw.Show();
            Close();
        }
    }
}
