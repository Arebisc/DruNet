using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DruNet_WPF.Core;

namespace DruNet_WPF.Views
{
    /// <summary>
    /// Interaction logic for ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        public ClientPage()
        {
            InitializeComponent();

            //ApplicationLogicInitializer.ClientRun();
            Client.Instance.PrintOutputOnTextBlock += PrintOutput;
            //Client.Instance.Run();
        }

        public void PrintOutput(string output)
        {
            Dispatcher.Invoke(() =>
            {
                OutputTb.Text += (output + "\n");
            });
        }

        /*
        private void CommandLine_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                switch (CommandLineTb.Text)
                {
                    case "/login":
                        Login(CommandLineTb.Text);
                        break;
                    case "/ls":
                        Client.Instance.ViewTree();
                        break;
                    case "/logout":
                        Client.Instance.locker = 1;
                        Client.Instance.Send(0, " ");
                        break;
                    case "/cd":
                        Client.Instance.CreateDirectory(CommandLineTb.Text);
                        break;
                    case "/rf":
                        Client.Instance.ReadFile(CommandLineTb.Text);
                        break;
                    case "/cf":
                        Client.Instance.CreateFile(CommandLineTb.Text);
                        break;
                    default:
                        break;
                }

                CommandLineTb.Text = String.Empty;
            }
        }
        */

        private void ShowLoginPanel(object sender, RoutedEventArgs e)
        {
            if (LoginPanel.Visibility != Visibility.Visible && Client.Instance.locker == 0)
            {
                LoginPanel.Visibility = Visibility.Visible;
            }
        }

        private void LoginAction(object sender, RoutedEventArgs e)
        {
            Client.Instance.Send(1, LoginTb.Text);
            var serverResponse = Client.Instance.ReceiveFlag();

            if (serverResponse == 1)
            {
                Client.Instance.Send(1, PasswordTb.Password);
                serverResponse = Client.Instance.ReceiveFlag();
                if (serverResponse == 1)
                {
                    PrintOutput("Logged succesful!");
                    Client.Instance.locker = 0;
                }
                else
                {
                    PrintOutput("Wrong Password!");
                }
            }
            else
            {
                PrintOutput("Wrong Login");
            }
        }
    }
}
