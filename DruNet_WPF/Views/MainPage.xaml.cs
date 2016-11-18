using System;
using System.Windows;
using System.Windows.Controls;
using DruNet_WPF.Core;

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
            IpAddress.Text = "127.0.0.1";
            Port.Text = "1995";
        }

        private bool CheckIfIpAddressIsset()
        {
            return IpAddress.Text != String.Empty;
        }

        private int GetPort()
        {
            return Int32.Parse(Port.Text);
        }

        private string GetIp()
        {
            return IpAddress.Text;
        }

        private void ClientStart_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckIfIpAddressIsset())
            {
                ApplicationLogicInitializer.ClientInit(GetIp(), GetPort());
                Navigate?.Invoke(new Views.ClientPage());
            }
            else MessageBox.Show("Wpisz adres IP!", "Uwaga");
        }

        private void ServerStart_OnClick(object sender, RoutedEventArgs e)
        {
            if (CheckIfIpAddressIsset())
            {
                ApplicationLogicInitializer.ServerInit(GetIp(), GetPort());
                Navigate?.Invoke(new Views.ServerPage());
            }
            else MessageBox.Show("Wpisz adres IP!", "Uwaga");
        }
    }
}
