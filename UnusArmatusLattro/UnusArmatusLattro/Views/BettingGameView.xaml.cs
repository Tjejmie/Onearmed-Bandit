using System;
using System.Collections.Generic;
using System.Media;
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
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Views
{
    /// <summary>
    /// Interaction logic for BettingGameView.xaml
    /// </summary>
    public partial class BettingGameView : UserControl
    {
        bool isRunning;
        readonly Storyboard story = new Storyboard();
        readonly Storyboard LeverStory = new Storyboard();
        public BettingGameView()
        {
            InitializeComponent();
            GenerateStoryboards();
        }

        private void GenerateStoryboards()
        {
            ColorAnimation color = new ColorAnimation
            {
                From = Colors.LightPink,
                To = Colors.Red,
                Duration = TimeSpan.FromSeconds(.2),
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true
            };

            story.Children.Add(color);
            Storyboard.SetTarget(color, StopBorder);
            Storyboard.SetTargetProperty(color, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));

            ColorAnimation leverColor = new ColorAnimation
            {
                From = Colors.LightPink,
                To = Colors.Red,
                Duration = TimeSpan.FromSeconds(.2),
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true
            };

            LeverStory.Children.Add(leverColor);
            Storyboard.SetTarget(leverColor, Lever);
            Storyboard.SetTargetProperty(leverColor, new PropertyPath("(Ellipse.Stroke).(SolidColorBrush.Color)"));
        }

        private void DoubleAnimation_Completed2(object sender, EventArgs e)
        {
            BettingBox.Focus();
            if (isRunning)
            {
                BettingGameViewModel gameViewModel = (BettingGameViewModel)DataContext;
                if (gameViewModel != null)
                    gameViewModel.ScoreToAdd = "";
                isRunning = false;
            }
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {
            BettingGameViewModel gameViewModel = (BettingGameViewModel)DataContext;
            if (gameViewModel.ConfirmBet(BettingBox.Text, Wallet.Text))
            {
                gameViewModel.StartTimer();
                LeverStory.Stop();
            }
            else
            {
                BettingPopup.IsOpen = true;
                BettingBox.Text = "";
            }
        }

        private void LeverCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetLeft(Lever, LeverCanvas.ActualWidth / 2 - Lever.Width / 2);
            Canvas.SetTop(Lever, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TogglePopupButton.IsChecked = false;
        }

        private void BettingPopupButton(object sender, RoutedEventArgs e)
        {
            BettingPopup.IsOpen = false;
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            BettingBox.Focus();
        }

        private void Lever_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BettingGameViewModel gameViewModel = (BettingGameViewModel)DataContext;
            gameViewModel.PlayEffect(Data.Sounds.Lever);
            isRunning = true;
        }

        private void HighScorePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void BettingBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = e.Key == Key.Space;
        }

        private void Button_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (StopButton.IsEnabled)
                story.Begin();
            else
                story.Stop();
        }

        private void BettingBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BettingBox.Text == "")
                LeverStory.Stop();
            else
            {
                if (int.Parse(BettingBox.Text) <= int.Parse(Wallet.Text))
                {
                    LeverStory.Begin();
                }
                else
                {
                    LeverStory.Stop();
                }
            }
        }
    }
}
