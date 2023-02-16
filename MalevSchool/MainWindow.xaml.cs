using MalevSchool.Pages;
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

namespace MalevSchool
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string code;
        public MainWindow()
        {
            InitializeComponent();

            BaseClass.ME = new MalevEntities();

            FrameClass.fMain = fMain;

            FrameClass.fMain.Navigate(new ShowListServicePage(code));
        }

        private void btnCodeAdmin_Click(object sender, RoutedEventArgs e)
        {
            code = tboxCodeAdmin.Text;

            FrameClass.fMain.Navigate(new ShowListServicePage(code));
        }
    }
}
