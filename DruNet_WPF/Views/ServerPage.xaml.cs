using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using DruNet_WPF.Core;

namespace DruNet_WPF.Views
{
    /// <summary>
    /// Interaction logic for ServerPage.xaml
    /// </summary>
    public partial class ServerPage : Page
    {
        public ServerPage()
        {
            InitializeComponent();
            //ApplicationLogicInitializer.ServerRun();
            Server.Instance.PrintOutputOnTextBlock += PrintOutput;
            ServerRun();
        }

        public void AddOutput(string output)
        {
            OutputTb.Text += output;
        }

        private void ServerRun()
        {
            ThreadStart serverThread = new ThreadStart(Server.Instance.Start);
            Thread serverListiner = new Thread(serverThread);
            serverListiner.Start();
        }

        public void PrintOutput(string output)
        {
            Dispatcher.Invoke(() =>
            {
                OutputTb.Text += (output + "\n");
            });            
        }
    }
}
