using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ShowListServicePage.xaml
    /// </summary>
    public partial class ShowListServicePage : Page
    {
        private string code;

        public ShowListServicePage(string code)
        {
            InitializeComponent();

            this.code = code;

            LSH.ItemsSource = BaseClass.ME.Service.ToList();

            ch.Text = BaseClass.ME.Service.ToList().Count + "/" + BaseClass.ME.Service.ToList().Count;

            if (code == "0000")
            {
                btnAdd.Visibility = Visibility.Visible;

                btnShow.Visibility = Visibility.Visible;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.fMain.Navigate(new AddUpgradePage(code));
        }

        private void searchName_TextChanged(object sender, TextChangedEventArgs e)
        {
            filter();
        }

        private void searchOpis_TextChanged(object sender, TextChangedEventArgs e)
        {
            filter();
        }

        private void sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter();
        }

        private void filt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filter();
        }

        private void btnShow_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.fMain.Navigate(new BIZapPage(code));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            int id = Convert.ToInt32(btn.Uid);

            Service serv = BaseClass.ME.Service.FirstOrDefault(x => x.ID == id);

            List<ClientService> clientservices = BaseClass.ME.ClientService.Where(x => x.ServiceID == serv.ID).ToList();

            if (clientservices.Count > 0)
            {
                MessageBox.Show("Удаление невозможно, cуществует запись на данную услугу");
            }

            else
            {
                BaseClass.ME.Service.Remove(serv);

                BaseClass.ME.SaveChanges();

                FrameClass.fMain.Navigate(new ShowListServicePage(code));
            }
        }

        private void btnDelete_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (code == "0000")
            {
                btn.Visibility = Visibility.Visible;
            }
        }

        private void btnUpgrade_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            int id = Convert.ToInt32(btn.Uid);

            Service serv = BaseClass.ME.Service.FirstOrDefault(x => x.ID == id);

            FrameClass.fMain.Navigate(new AddUpgradePage(serv, code));
        }

        private void btnUpgrade_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (code == "0000")
            {
                btn.Visibility = Visibility.Visible;
            }
        }

        private void btnAddUslug_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            int id = Convert.ToInt32(btn.Uid);

            Service serv = BaseClass.ME.Service.FirstOrDefault(x => x.ID == id);

            FrameClass.fMain.Navigate(new AddZapPage(serv, code));
        }

        private void btnAddUslug_Loaded(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            if (code == "0000")
            {
                btn.Visibility = Visibility.Visible;
            }
        }

        void filter()
        {
            List<Service> services = new List<Service>();

            services = BaseClass.ME.Service.ToList();

            if (!string.IsNullOrWhiteSpace(searchName.Text))
            {
                services = services.Where(x => x.Title.ToLower().Contains(searchName.Text.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(searchOpis.Text))
            {

                List<Service> trl = services.Where(x => x.Description != null).ToList();

                if (trl.Count > 0)
                {

                    services = trl.Where(x => x.Description.ToLower().Contains(searchOpis.Text.ToLower())).ToList();

                }

                else
                {
                    MessageBox.Show("нет описания");

                    searchOpis.Text = "";
                }

            }

            switch (filt.SelectedIndex)
            {
                case 0:

                    services = services.ToList();

                break;

                case 1:

                    services = services.Where(x => x.Discount >= 0 && x.Discount < 0.05).ToList();

                break;

                case 2:

                    services = services.Where(x => x.Discount >= 0.05 && x.Discount < 0.15).ToList();

                break;

                case 3:

                    services = services.Where(x => x.Discount >= 0.15 && x.Discount < 0.3).ToList();

                break;

                case 4:

                    services = services.Where(x => x.Discount >= 0.3 && x.Discount < 0.7).ToList();

                break;

                case 5:

                    services = services.Where(x => x.Discount >= 0.7 && x.Discount < 1).ToList();

                break;
            }

            switch (sort.SelectedIndex)
            {
                case 0:
                    {
                        services.Sort((x, y) => x.Cost.CompareTo(y.Cost));
                    }
                break;

                case 1:
                    {
                        services.Sort((x, y) => x.Cost.CompareTo(y.Cost));

                        services.Reverse();
                    }
                break;
            }

            LSH.ItemsSource = services;

            if (services.Count == 0)
            {
                MessageBox.Show("нет записей");

                ch.Text = BaseClass.ME.Service.ToList().Count + "/" + BaseClass.ME.Service.ToList().Count;

                searchName.Text = "";

                searchOpis.Text = "";

                sort.SelectedIndex = 0;

                filt.SelectedIndex = 0;
            }

            ch.Text = services.Count + "/" + BaseClass.ME.Service.ToList().Count;

        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
