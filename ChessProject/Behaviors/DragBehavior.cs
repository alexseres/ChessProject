using ChessProject.Models.Pieces;
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

        private Point _elementStartPosition;
        private Point _mouseStartPosition;
        private Point _m_start;

        public static readonly DependencyProperty IsDragProperty =
            DependencyProperty.RegisterAttached("Drag", typeof(bool), typeof(DragBehavior), new PropertyMetadata(false, OnDragChanged));

        public static bool GetDrag(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragProperty);
        }

        public static void SetDrag(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragProperty, value);
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
            FrameworkElement element = sender as FrameworkElement;
            Grid parent = element.FindAncestor<Grid>();
            //var parent = Application.Current.MainWindow;
            _mouseStartPosition = mouseButtoEventArgs.GetPosition(parent);
            ((UIElement)sender).CaptureMouse();
        }

        private void ElementOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            ((UIElement)sender).ReleaseMouseCapture();
            FrameworkElement element = sender as FrameworkElement;
            Grid parent = element.FindAncestor<Grid>();
            Point pos = mouseButtonEventArgs.GetPosition(parent);
            
            //var pos = mouseButtonEventArgs.GetPosition()
            _elementStartPosition.X = Transform.X;
            _elementStartPosition.Y = Transform.Y;
        }

        private void ElementOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            FrameworkElement element = sender as FrameworkElement;
            Grid parent = element.FindAncestor<Grid>();
            var mousePos = mouseEventArgs.GetPosition(parent);
            var diff = (mousePos - _mouseStartPosition);
            if (!((UIElement)sender).IsMouseCaptured) return;
            //if(mousePos.X < parent.Width && mousePos.Y < parent.Height && mousePos.X >= 0 && mousePos.Y >= 0)
            //{
            
            //}
             
            Transform.X = diff.X;
            Transform.Y = diff.Y;
            //Transform.X = _elementStartPosition.X + diff.X;
            //Transform.Y = _elementStartPosition.Y + diff.Y;
        }
    }
}
