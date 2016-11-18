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
            
        }

        public void PrintOutput(string output)
        {
            OutputTb.Text += output;
        }

        private void CommandLine_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Client.Instance.Send(1, CommandLineTb.Text);
                CommandLineTb.Text = String.Empty;
            }
        }
    }
}
