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

namespace UnusArmatusLattro.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public GameView()
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

        private void Lever_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(this, new DataObject(DataFormats.Serializable, this), DragDropEffects.Move);
            }
        }

        private void Lever_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Canvas.SetTop(Lever, 300);
        }
    }
}
