using ChessProject.Models;
using ChessProject.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChessProject.ViewModels
{
    public class MainGameViewModel
    {
        public Side BlackSide { get; set; }
        public Side WhiteSide { get; set; }

        public MainGameViewModel()
        {
            BlackSide = SideInitializer.InitializeSide("Black");
            WhiteSide = SideInitializer.InitializeSide("White");
        }


    }
}
