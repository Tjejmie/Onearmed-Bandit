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
        public BettingGameView()
        {
            InitializeComponent();
            
        }
        private void LeverCanvas_DragOver(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);
            //if (data is LeverButton btn)
            //{

            //    Point dropPosition = e.GetPosition(LeverCanvas);

            //    Canvas.SetTop(btn, e.GetPosition(LeverCanvas).Y);


            //}
            Canvas.SetTop(Lever, e.GetPosition(LeverCanvas).Y);
        }
        private void DoubleAnimation_Completed2(object sender, EventArgs e)
        {
            BettingGameViewModel gameViewModel = (BettingGameViewModel)DataContext;
            BettingBox.Focus();
            //if (gameViewModel.ScoreToAdd != null);
               //gameViewModel.StartTimer();
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {
            BettingGameViewModel gameViewModel = (BettingGameViewModel)DataContext;
            if(gameViewModel.ConfirmBet(BettingBox.Text, Wallet.Text))
            gameViewModel.StartTimer();
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
        
        private void Load(object sender, RoutedEventArgs e)
        {
            BettingBox.Focus();
        }
    }
}
