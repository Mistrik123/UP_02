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
    /// Логика взаимодействия для TovarWindow.xaml
    /// </summary>
    public partial class TovarWindow : Window
    {
        Tovari currentTovar;

        List<Category> categories = new List<Category>();
        List<Postavshik> postavshiks = new List<Postavshik>();
        List<Proizvoditel> proizvoditels = new List<Proizvoditel>();
        List<EdIzm> edIzms = new List<EdIzm>();

        public TovarWindow(Tovari currentTovar)
        {
            InitializeComponent();
            this.currentTovar = currentTovar;
        }

        public TovarWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Delete_btn.Visibility = Visibility.Collapsed;

            string photoName = "picture.png";

            using(DBContext db = new DBContext())
            {
                categories = db.Categories.ToList();
                postavshiks = db.Postavshiks.ToList();
                proizvoditels = db.Proizvoditels.ToList();
                edIzms = db.EdIzms.ToList();
            }

            Category.ItemsSource = categories.Select(s => s.Name);
            Category.SelectedIndex = 0;

            Postavshik.ItemsSource = postavshiks.Select(s => s.Name);
            Postavshik.SelectedIndex = 0;

            Proizvoditel.ItemsSource = proizvoditels.Select(s => s.Name);
            Proizvoditel.SelectedIndex = 0;

            EdIzm.ItemsSource = edIzms.Select(s => s.Name);
            EdIzm.SelectedIndex = 0;

            if (currentTovar != null)
            {
                Delete_btn.Visibility = Visibility.Visible;
                Article.Visibility = Visibility.Collapsed;

                using (DBContext db = new DBContext())
                {
                    Category.SelectedItem = db.Categories.FirstOrDefault(f => f.Id == currentTovar.CategoryId);
                    Postavshik.SelectedItem = db.Proizvoditels.FirstOrDefault(f => f.Id == currentTovar.ProizvoditelId);
                    Proizvoditel.SelectedItem = db.Postavshiks.FirstOrDefault(f => f.Id == currentTovar.PostavshikId);
                    EdIzm.SelectedItem = db.EdIzms.FirstOrDefault(f => f.Id == currentTovar.EdIzmId);
                }
                TovarName.Text = currentTovar.Name;
                Price.Text = Convert.ToDouble(currentTovar.Price).ToString("F2");
                Description.Text = currentTovar.Description;

                Next_btn.Content = "Отредактировать";

                if (currentTovar.Photo.ToString().Length > 0)
                    photoName = currentTovar.Photo;
            }

            Img.Source = new BitmapImage(new Uri($"/Images/{photoName}", UriKind.Relative));
        }

        private void Next_btn_Click(object sender, RoutedEventArgs e)
        {
            Category categ = categories[Category.SelectedIndex];
            Postavshik post = postavshiks[Postavshik.SelectedIndex];
            Proizvoditel proiz = proizvoditels[Proizvoditel.SelectedIndex];
            EdIzm edizm = edIzms[EdIzm.SelectedIndex];
            

            if (TovarName.Text.Length > 0 && Price.Text.Length > 0 && Description.Text.Length > 0)
            {
                if (currentTovar != null)
                {
                    using (DBContext db = new DBContext())
                    {
                        Tovari edit = db.Tovaris.FirstOrDefault(f => f.Article == currentTovar.Article);

                        
                        edit.Name = TovarName.Text;
                        edit.Description = Description.Text;
                        edit.CategoryId = categ.Id;
                        edit.PostavshikId = post.Id;
                        edit.ProizvoditelId = proiz.Id;
                        edit.EdIzmId = edizm.Id;

                        try
                        {
                            edit.Price = Convert.ToDecimal(Price.Text);
                        }
                        catch
                        {
                            MessageBox.Show("Введите корректное значение цены", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        db.SaveChanges();
                        MessageBox.Show("Успешное редактирование", "Редактирование", MessageBoxButton.OK, MessageBoxImage.Information);

                        Close();
                    }
                }
                else
                {
                    using (DBContext db = new DBContext())
                    {

                        Tovari article = db.Tovaris.FirstOrDefault(f => f.Article == Article.Text);

                        if (article != null)
                        {
                            MessageBox.Show("Артикул не уникальный", "Изменение данных", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        Tovari newTovar = new Tovari();

                        newTovar.Name = TovarName.Text;
                        newTovar.Description = Description.Text;
                        newTovar.Article = Article.Text;
                        newTovar.CategoryId = categ.Id;
                        newTovar.PostavshikId = post.Id;
                        newTovar.ProizvoditelId = proiz.Id;
                        newTovar.EdIzmId = edizm.Id;
                        newTovar.Photo = "";
                        newTovar.Skidka = 0;

                        try
                        {
                            newTovar.Price = Convert.ToDecimal(Price.Text);
                        }
                        catch
                        {
                            MessageBox.Show("Введите корректное значение цены", "Добавление", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        db.Tovaris.Add(newTovar);
                        db.SaveChanges();

                        MessageBox.Show("Успешное добавление", "Добавление", MessageBoxButton.OK, MessageBoxImage.Information);

                        Close();
                    }
                }
            }
            else
                MessageBox.Show("Заполните все поля", "Изменение данных", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Delete_btn_Click(object sender, RoutedEventArgs e)
        {
            using(DBContext db = new DBContext())
            {
                db.Tovaris.Remove(db.Tovaris.FirstOrDefault(f => f.Article == currentTovar.Article));
                db.SaveChanges();
                MessageBox.Show("Удаление успешно", "Удаление товара", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
        }
    }
}
