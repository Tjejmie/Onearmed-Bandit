using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using UnusArmatusLattro.Data;
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        readonly Storyboard buttonStory = new Storyboard();
        readonly Storyboard leverStory = new Storyboard();
        bool isRunning;

        public GameView()
        {
            InitializeComponent();
            SetColorButton();
            SetColorLever();
        }

        private void SetColorLever()
        {
            ColorAnimation color = new ColorAnimation
            {
                From = Colors.Yellow,
                To = Colors.Red,
                Duration = TimeSpan.FromSeconds(.2),
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true
            };

            leverStory.Children.Add(color);
            Storyboard.SetTarget(color, Lever);
            Storyboard.SetTargetProperty(color, new PropertyPath("(Ellipse.Stroke).(SolidColorBrush.Color)"));
            leverStory.Begin();
        }

        private void SetColorButton()
        {
            ColorAnimation color = new ColorAnimation
            {
                From = Colors.LightPink,
                To = Colors.Red,
                Duration = TimeSpan.FromSeconds(.2),
                RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = true
            };

            buttonStory.Children.Add(color);
            Storyboard.SetTarget(color, Border);
            Storyboard.SetTargetProperty(color, new PropertyPath("(Border.BorderBrush).(SolidColorBrush.Color)"));
        }

        private void Lever_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas.SetTop(Lever, 300);
        }

        private void DoubleAnimation_Completed2(object sender, EventArgs e)
        {
            if (isRunning)
            {
                GameViewModel gameViewModel = (GameViewModel)DataContext;
                if (gameViewModel != null)
                {
                    if (gameViewModel.ScoreToAdd != null && gameViewModel.ScoreToAdd != "")
                    {
                        gameViewModel.StartTimer();

                    }
                    gameViewModel.ScoreToAdd = "";
                }
            }
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {
            GameViewModel gameViewModel = (GameViewModel)DataContext;
            gameViewModel.StartTimer();
            isRunning = true;
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

        private void Lever_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GameViewModel gameViewModel = (GameViewModel)DataContext;
            gameViewModel.Playeffect(Sounds.Lever);
            buttonStory.Begin();
            leverStory.Stop();
        }


        private void DoubleAnimation_Completed_2(object sender, EventArgs e)
        {
            if (isRunning)
            {
                GameViewModel gameViewModel = (GameViewModel)DataContext;
                if (gameViewModel != null)
                {
                    if (gameViewModel.SpinnToAdd != null)
                    {
                        gameViewModel.StartTimer();

                    }
                    gameViewModel.SpinnToAdd = "";
                }
            }
        }
    }
}
