﻿using System;
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
        }

        public void AddOutput(string output)
        {
            OutputTb.Text += output;
        }

        private void CommandLineTb_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}