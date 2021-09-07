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
    /// Interaction logic for Slots.xaml
    /// </summary>
    public partial class Slots : UserControl
    {


        public string number
        {
            get { return (string)GetValue(numberProperty); }
            set { SetValue(numberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for number.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty numberProperty =
            DependencyProperty.Register("number", typeof(string), typeof(Slots), new PropertyMetadata("1"));



        public SolidColorBrush Slotcolor
        {
            get { return (SolidColorBrush)GetValue(SlotcolorProperty); }
            set { SetValue(SlotcolorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Slotcolor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SlotcolorProperty =
            DependencyProperty.Register("Slotcolor", typeof(SolidColorBrush), typeof(Slots), new PropertyMetadata(Brushes.White));


        public Slots()
        {
            InitializeComponent();
        }
    }
}
