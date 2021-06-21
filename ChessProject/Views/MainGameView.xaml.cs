using ChessProject.ViewModels;
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
using System.Windows.Shapes;

namespace ChessProject.Views
{
    /// <summary>
    /// Interaction logic for MainGameView.xaml
    /// </summary>
    public partial class MainGameView : Window
    {
        public MainGameViewModel _vm;
        public MainGameView()
        {
            InitializeComponent();
            _vm = new MainGameViewModel();
            this.DataContext = _vm;
        }
    }
}
