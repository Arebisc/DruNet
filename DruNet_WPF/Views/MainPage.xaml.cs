using System;
using System.Windows;
using System.Windows.Controls;

public delegate bool NavigationService(object Page);

namespace DruNet_WPF.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public static NavigationService Navigate;

        public MainPage()
        {
            InitializeComponent();
        }

        private bool CheckIfIpAddressIsset()
        {
            return IpAddress.Text != String.Empty;
        }

        private void ClientStart_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckIfIpAddressIsset())
                Navigate?.Invoke(new Views.ClientPage());
        }

        private void ServerStart_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckIfIpAddressIsset())
                Navigate?.Invoke(new Views.ServerPage());
        }
    }
}
