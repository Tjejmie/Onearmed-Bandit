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
using UnusArmatusLattro.ViewModels;

namespace UnusArmatusLattro.Views
{
    /// <summary>
    /// Interaction logic for StartView.xaml
    /// </summary>
    public partial class StartView : UserControl
    {
        MainWindow mainWindow;

        public StartView()
        {
            InitializeComponent();
        }

     

        private void SoundBtn_Checked(object sender, RoutedEventArgs e)
        {
            mainWindow = (MainWindow)Window.GetWindow(this);
            mainWindow.PauseMusic();
            imgLineItemAdd.Source = new BitmapImage(new Uri("/Resources/Images/soundOff.png", UriKind.RelativeOrAbsolute));

        }

        private void SoundBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            mainWindow.ResumeMusic();
            imgLineItemAdd.Source = new BitmapImage(new Uri("/Resources/Images/soundOn.png", UriKind.RelativeOrAbsolute));
        }
    }
}
