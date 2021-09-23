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
    /// Interaction logic for BettingGameView.xaml
    /// </summary>
    public partial class BettingGameView : UserControl
    {
        public BettingGameView()
        {
            InitializeComponent();
            
        }
        private void DoubleAnimation_Completed2(object sender, EventArgs e)
        {
            BettingGameViewModel gameViewModel = (BettingGameViewModel)DataContext;
            //if (gameViewModel.ScoreToAdd != null);
               //gameViewModel.StartTimer();
        }

        private void DoubleAnimation_Completed(object sender, EventArgs e)
        {
            BettingGameViewModel gameViewModel = (BettingGameViewModel)DataContext;
            if(gameViewModel.ConfirmBet(BettingBox.Text, Wallet.Text))
            gameViewModel.StartTimer();
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            BettingBox.Focus();
        }
    }
}
