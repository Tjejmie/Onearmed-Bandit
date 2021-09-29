using System;
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
using UnusArmatusLattro.Models;

namespace UnusArmatusLattro.Views
{
    /// <summary>
    /// Interaction logic for HighscoreView.xaml
    /// </summary>
    public partial class HighscoreView : UserControl
    {
        public int Score
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Score.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScoreProperty =
            DependencyProperty.Register("Score", typeof(int), typeof(HighscoreView), new PropertyMetadata(0));

        public new string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(HighscoreView), new PropertyMetadata(""));

        public HighscoreView()
        {
            InitializeComponent();
        }
    }
}
