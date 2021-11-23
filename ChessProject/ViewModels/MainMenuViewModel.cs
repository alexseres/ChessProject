using ChessProject.Commands;
using ChessProject.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        public MainGameView MainGameViewProperty { get; set; }

        private RelayCommand<MainGameView> _openMainGameCommand;
        public RelayCommand<MainGameView> OpenMainGameCommand { get { return _openMainGameCommand; } set { SetProperty(ref _openMainGameCommand, value); } }

        public MainMenuViewModel()
        {
            OpenMainGameCommand = new RelayCommand<MainGameView>(OpenMainGame, OpenMainGameCanExecute);
        }

        public bool OpenMainGameCanExecute(object obj)
        {
            return true;
        }

        public void OpenMainGame(object obj)
        {
            
            MainGameViewProperty = new MainGameView();
            MainGameViewProperty.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            MainGameViewProperty.Show();

        }
    }
}
