﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UnusArmatusLattro.Views
{
    /// <summary>
    /// Interaction logic for GameOverView.xaml
    /// </summary>
    public partial class GameOverView : UserControl
    {
        public GameOverView()
        {
            InitializeComponent();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            txtUserName.Focus();
        }

    }
}
