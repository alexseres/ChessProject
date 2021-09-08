using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace ChessProject.Utils
{
    public static class AncestorFinder
    {
        public static T FindAncestor<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            DependencyObject tree = VisualTreeHelper.GetParent(dependencyObject);
            while(tree != null && !(tree is T))
            {
                tree = VisualTreeHelper.GetParent(tree);
            }
            return tree as T;
        }

        
    }
}
