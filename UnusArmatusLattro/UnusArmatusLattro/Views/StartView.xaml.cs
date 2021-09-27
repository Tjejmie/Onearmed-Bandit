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
        // MediaPlayer mediaPlayer;
       MainWindow mainWindow;
  
        public StartView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           mainWindow = (MainWindow)Window.GetWindow(this);
           mainWindow.TurnOffMusic();

            ControlTemplate ct = soundBtn.Template;
            imgLineItemAdd.Source = new BitmapImage(new Uri("/Resources/Images/soundOff.png", UriKind.RelativeOrAbsolute));
          

        }
    }
}
