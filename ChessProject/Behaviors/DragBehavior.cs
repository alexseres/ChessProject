using ChessProject.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChessProject.Behaviors
{
    public class DragBehavior
    {
        public readonly TranslateTransform Transform = new TranslateTransform();
        private static DragBehavior _instance = new DragBehavior();
        public static DragBehavior Instance { get { return _instance; } set { _instance = value; } }

        public static readonly DependencyProperty IsDragProperty =
            DependencyProperty.RegisterAttached("Drag", typeof(bool), typeof(DragBehavior), new PropertyMetadata(false, OnDragChanged));
        public static bool GetDrag(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragProperty);
        }

        private static void OnDragChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement)sender;
            var isDrag = (bool)(e.NewValue);

            Instance = new DragBehavior();
            ((UIElement)sender).RenderTransform = Instance.Transform;

            if (isDrag)
            {
                element.MouseLeftButtonDown += Instance.ElementOnMouseLeftButtonDown;
                element.MouseLeftButtonUp += Instance.ElementOnMouseLeftButtonUp;
                element.MouseMove += Instance.ElementOnMouseMove;
            }
            else
            {
                element.MouseLeftButtonDown -= Instance.ElementOnMouseLeftButtonDown;
                element.MouseLeftButtonUp -= Instance.ElementOnMouseLeftButtonUp;
                element.MouseMove -= Instance.ElementOnMouseMove;
            }
        }

        private void ElementOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtoEventArgs)
        {
            ((UIElement)sender).CaptureMouse();
        }

        private void ElementOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            ((UIElement)sender).ReleaseMouseCapture();
        }

        private void ElementOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            FrameworkElement element = sender as FrameworkElement;
            Grid parent = element.FindAncestor<Grid>();
            var mousePos = mouseEventArgs.GetPosition(parent);
            if (!((UIElement)sender).IsMouseCaptured) return;

        }

        
    }
}
