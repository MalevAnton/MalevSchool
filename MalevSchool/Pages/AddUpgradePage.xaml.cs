using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MalevSchool.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddUpgradePage.xaml
    /// </summary>
    public partial class AddUpgradePage : Page
    {
        private string code;

        int n = 0;

        string path;

        bool flagUpdate;

        bool flagUpdatePhoto;

        Service serv;

        ServicePhoto servicephoto;

        public AddUpgradePage(string code)
        {
            InitializeComponent();

            this.code = code;

            addPhotos.Visibility = Visibility.Collapsed;
        }

        public AddUpgradePage(Service serv, string code)
        {
            InitializeComponent();

            this.code = code;

            this.serv = serv;

            flagUpdate = true;

            usl.Text = serv.Title;

            cost.Text = Convert.ToString(serv.Cost);

            time.Text = Convert.ToString(serv.DurationInSeconds / 60);

            sale.Text = Convert.ToString(serv.Discount * 100);

            opis.Text = serv.Description;

            if (serv.MainImagePath != null)
            {
                BitmapImage img = new BitmapImage(new Uri(serv.MainImagePath, UriKind.RelativeOrAbsolute));

                Img.Source = img;
            }

            UpdatePhoto.Visibility = Visibility.Visible;
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (flagUpdate == false)
            {
                if (usl.Text == "" || cost.Text == "" || time.Text == "" || sale.Text == "" || path == null)
                {
                    MessageBox.Show("Обязательные поля не заполнены", "Ошибка", MessageBoxButton.OK);
                }

                else
                {

                    serv = new Service();

                    serv.Title = usl.Text;

                    serv.Cost = Convert.ToInt32(cost.Text);

                    double skidk = Convert.ToDouble(sale.Text) / 100;

                    serv.Discount = skidk;

                    int timeSecond = Convert.ToInt32(time.Text) * 60;

                    serv.DurationInSeconds = timeSecond;

                    serv.MainImagePath = path;

                    if (opis.Text == "")
                    {
                        serv.Description = null;
                    }

                    else
                    {
                        serv.Description = opis.Text;
                    }

                    BaseClass.ME.Service.Add(serv);

                    servicephoto = new ServicePhoto();

                    servicephoto.ServiceID = serv.ID;

                    servicephoto.PhotoPath = path;

                    BaseClass.ME.ServicePhoto.Add(servicephoto);

                    BaseClass.ME.SaveChanges();

                    MessageBox.Show("Информация добавлена");

                    FrameClass.fMain.Navigate(new ShowListServicePage(code));

                }
            }

            else
            {
                if (usl.Text == "" || cost.Text == "" || time.Text == "" || sale.Text == "")
                {
                    MessageBox.Show("Обязательные поля не заполнены", "Ошибка", MessageBoxButton.OK);
                }

                else
                {
                    serv.Title = usl.Text;

                    serv.Cost = Convert.ToInt32(cost.Text);

                    double skidk = Convert.ToDouble(sale.Text) / 100;

                    serv.Discount = skidk;

                    int timeSecond = Convert.ToInt32(time.Text) * 60;

                    serv.DurationInSeconds = timeSecond;

                    if (path == null)
                    {
                        path = serv.MainImagePath;
                    }
                    if ((path != null) && (flagUpdatePhoto == false))
                    {
                        servicephoto = new ServicePhoto();

                        servicephoto.ServiceID = serv.ID;

                        servicephoto.PhotoPath = path;

                        BaseClass.ME.ServicePhoto.Add(servicephoto);
                    }

                    serv.MainImagePath = path;

                    if (opis.Text == "")
                    {
                        serv.Description = null;
                    }

                    else
                    {
                        serv.Description = opis.Text;
                    }

                    BaseClass.ME.SaveChanges();

                    MessageBox.Show("Информация добавлена");

                    FrameClass.fMain.Navigate(new ShowListServicePage(code));

                }
            }
        }

        private void DeletPhoto_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> photos = BaseClass.ME.ServicePhoto.Where(x => x.ServiceID == serv.ID).ToList();

            if (photos[n].PhotoPath != serv.MainImagePath)
            {
                ServicePhoto photo = photos.FirstOrDefault(x => x.PhotoPath == photos[n].PhotoPath);

                BaseClass.ME.ServicePhoto.Remove(photo);

                BaseClass.ME.SaveChanges();

                FrameClass.fMain.Navigate(new AddUpgradePage(serv, code));
            }

            else
            {
                MessageBox.Show("Данную фотографию нельзя удалить, так как она является обязательной", "Ошибка", MessageBoxButton.OK);
            }
        }

        private void UpdatePhoto_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> servicePhoto = BaseClass.ME.ServicePhoto.Where(x => x.ServiceID == serv.ID).ToList();

            if (servicePhoto.Count > 1)
            {
                BitmapImage img = new BitmapImage(new Uri(servicePhoto[n].PhotoPath, UriKind.RelativeOrAbsolute));

                Img.Source = img;

                Nextbtn.Visibility = Visibility.Visible;

                Backbtn.Visibility = Visibility.Visible;

                safePhoto.Visibility = Visibility.Visible;

                addPhoto.Visibility = Visibility.Collapsed;

                UpdatePhoto.Visibility = Visibility.Collapsed;

                back.Visibility = Visibility.Visible;

                addPhotos.Visibility = Visibility.Collapsed;

                DeletPhoto.Visibility = Visibility.Visible;
            }

            else
            {
                MessageBox.Show("Нет дополнительных фотографий", "Ошибка", MessageBoxButton.OK);
            }
        }

        private void addPhotos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();

                OFD.Multiselect = true;

                if (OFD.ShowDialog() == true)
                {
                    foreach (string file in OFD.FileNames)
                    {
                        ServicePhoto u = new ServicePhoto();

                        u.ServiceID = serv.ID;

                        path = file;

                        string[] arrayPath = path.Split('\\');

                        path = "\\" + arrayPath[arrayPath.Length - 2] + "\\" + arrayPath[arrayPath.Length - 1];

                        u.PhotoPath = path;

                        BaseClass.ME.ServicePhoto.Add(u);
                    }

                    BaseClass.ME.SaveChanges();

                    MessageBox.Show("Фото добавлены");
                }
            }

            catch
            {
                MessageBox.Show("Что-то пошло не так");
            }
        }

        private void addPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog OFD = new OpenFileDialog();

                OFD.InitialDirectory = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.Length - 9) + "images\\";

                OFD.ShowDialog();

                path = OFD.FileName;

                string[] arrayPath = path.Split('\\');


                if (arrayPath.Length != 1)
                {
                    path = "\\" + arrayPath[arrayPath.Length - 2] + "\\" + arrayPath[arrayPath.Length - 1];

                    Img.Source = new BitmapImage(new Uri(path, UriKind.Relative));
                }
            }
            catch
            {
                MessageBox.Show("Что то пошло не так", "Ошибка", MessageBoxButton.OK);
            }
        }

        private void safePhoto_Click(object sender, RoutedEventArgs e)
        {
            flagUpdatePhoto = true;

            List<ServicePhoto> u = BaseClass.ME.ServicePhoto.Where(x => x.ServiceID == serv.ID).ToList();

            serv.MainImagePath = u[n].PhotoPath;

            BaseClass.ME.SaveChanges();

            MessageBox.Show("Фотография изменена");

            Nextbtn.Visibility = Visibility.Collapsed;

            Backbtn.Visibility = Visibility.Collapsed;

            safePhoto.Visibility = Visibility.Collapsed;

            addPhoto.Visibility = Visibility.Visible;

            back.Visibility = Visibility.Collapsed;

            UpdatePhoto.Visibility = Visibility.Visible;

            addPhotos.Visibility = Visibility.Visible;

            DeletPhoto.Visibility = Visibility.Collapsed;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void back_Click_1(object sender, RoutedEventArgs e)
        {
            flagUpdatePhoto = false;

            Nextbtn.Visibility = Visibility.Collapsed;

            Backbtn.Visibility = Visibility.Collapsed;

            safePhoto.Visibility = Visibility.Collapsed;

            addPhoto.Visibility = Visibility.Visible;

            back.Visibility = Visibility.Collapsed;

            UpdatePhoto.Visibility = Visibility.Visible;

            addPhotos.Visibility = Visibility.Visible;

            DeletPhoto.Visibility = Visibility.Collapsed;

            if (serv.MainImagePath != null)
            {
                BitmapImage img = new BitmapImage(new Uri(serv.MainImagePath, UriKind.RelativeOrAbsolute));

                Img.Source = img;
            }
        }

        private void Backbtn_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> u = BaseClass.ME.ServicePhoto.Where(x => x.ServiceID == serv.ID).ToList();

            n--;

            if (Nextbtn.IsEnabled == false)
            {
                Nextbtn.IsEnabled = true;
            }

            if (u != null)
            {

                BitmapImage img = new BitmapImage(new Uri(u[n].PhotoPath, UriKind.RelativeOrAbsolute));

                Img.Source = img;
            }

            if (n == 0)
            {
                Backbtn.IsEnabled = false;
            }
        }

        private void Nextbtn_Click(object sender, RoutedEventArgs e)
        {
            List<ServicePhoto> servicePhoto = BaseClass.ME.ServicePhoto.Where(x => x.ServiceID == serv.ID).ToList();

            n++;

            if (Backbtn.IsEnabled == false)
            {
                Backbtn.IsEnabled = true;
            }

            if (servicePhoto != null)
            {

                BitmapImage img = new BitmapImage(new Uri(servicePhoto[n].PhotoPath, UriKind.RelativeOrAbsolute));

                Img.Source = img;
            }

            if (n == servicePhoto.Count - 1)
            {
                Nextbtn.IsEnabled = false;
            }
        }

        private void Back_Click_2(object sender, RoutedEventArgs e)
        {
            FrameClass.fMain.Navigate(new ShowListServicePage(code));
        }
    }
}
