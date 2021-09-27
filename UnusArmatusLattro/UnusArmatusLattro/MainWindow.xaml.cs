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
using UnusArmatusLattro.ViewModels;
using System.Media;
using System.Windows.Media.Animation;

namespace UnusArmatusLattro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MediaPlayer mediaPlayer;
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel mainView = new MainViewModel();
            mainView.CurrentViewModel = new StartViewModel(mainView);
            DataContext = mainView;
            BackgroundMusic();
        }

        public void BackgroundMusic()
        {
            var timeline = new MediaTimeline(new Uri("Resources/MP3/Las Vegas Casino Music Video_ For Night Game of Poker, Blackjack, Roulette Wheel & Slots.mp3", UriKind.Relative));
            timeline.RepeatBehavior = RepeatBehavior.Forever;
            mediaPlayer = new MediaPlayer();
            mediaPlayer.Volume = 0.05f;
            mediaPlayer.Clock = timeline.CreateClock();
            mediaPlayer.Clock.Controller.Begin();
        }

        public void PauseMusic()
        {
            mediaPlayer.Clock.Controller.Pause();
        }

        public void ResumeMusic()
        {
            mediaPlayer.Clock.Controller.Resume();
        }

    }
}
