using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        readonly Storyboard story = new Storyboard();
        public GameOverView()
        {
            InitializeComponent();
            ColorAnimation color = new ColorAnimation
            {
                From = Colors.LightPink,
                To = Colors.Red,
                Duration = TimeSpan.FromSeconds(.2),
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true
            };
            story.Children.Add(color);
            Storyboard.SetTarget(color, Border);
            Storyboard.SetTargetProperty(color, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            txtUserName.Focus();
        }

        private void TextboxGotFocus(object sender, RoutedEventArgs e)
        {
            story.Begin();
        }

        private void TextboxLostFocus(object sender, RoutedEventArgs e)
        {
            story.Stop();
        }

        private void HighScorePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z ]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
