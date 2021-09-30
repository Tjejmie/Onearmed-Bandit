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


        public int BorderSlot
        {
            get { return (int)GetValue(BorderSlotProperty); }
            set { SetValue(BorderSlotProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderSlot.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderSlotProperty =
            DependencyProperty.Register("BorderSlot", typeof(int), typeof(Slots), new PropertyMetadata(2));


        public int Number
        {
            get { return (int)GetValue(numberProperty); }
            set { SetValue(numberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for number.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty numberProperty =
            DependencyProperty.Register("Number", typeof(int), typeof(Slots), new PropertyMetadata(1));

        public SolidColorBrush BorderColor
        {
            get { return (SolidColorBrush)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BorderColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BorderColorProperty =
            DependencyProperty.Register("BorderColor", typeof(SolidColorBrush), typeof(Slots), new PropertyMetadata(Brushes.Gray));

        public SolidColorBrush Slotcolor
        {
            get { return (SolidColorBrush)GetValue(SlotcolorProperty); }
            set { SetValue(SlotcolorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Slotcolor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SlotcolorProperty =
            DependencyProperty.Register("Slotcolor", typeof(SolidColorBrush), typeof(Slots), new PropertyMetadata(Brushes.White));

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(Slots), new PropertyMetadata(null));

        public Slots()
        {
            InitializeComponent();
        }
    }
}
