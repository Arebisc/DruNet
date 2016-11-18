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
using DruNet_WPF.Views;
using MahApps.Metro.Controls;

namespace DruNet_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
//            WindowState = WindowState.Maximized;
            InitializeComponent();

            MainPage.Navigate += RootFrame.NavigationService.Navigate;

            RootFrame.Navigate(new Views.MainPage());
        }
    }
}
