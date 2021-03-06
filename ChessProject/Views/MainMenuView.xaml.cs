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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessProject.Views
{
    /// <summary>
    /// Interaction logic for MainMenuView.xaml
    /// </summary>
    public partial class MainMenuView
    {
        public MainMenuViewModel _vm; 
        public MainMenuView()
        {
            InitializeComponent();
            _vm = new MainMenuViewModel();
            DataContext = _vm;
            Application.Current.MainWindow = this;
        }
    }
}
