using ChessProject.Models.Pieces;
using ChessProject.Utils;
using ChessProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using static ChessProject.ViewModels.MainGameViewModel;

namespace ChessProject.Behaviors
{
    public class DragBehavior :Behavior<Grid>
    {
        public readonly TranslateTransform Transform = new TranslateTransform();
        private static DragBehavior _instance = new DragBehavior();

        public static DragBehavior Instance { get { return _instance; } set { _instance = value; } }

        private Point _elementStartPosition;
        private Point _mouseStartPosition;


        //public DragDelegate DragOver
        //{
        //    get { return (DragDelegate)GetValue(DragOverCommandProperty); }
        //    set { SetValue(DragOverCommandProperty, value); }
        //}

        //public DragDelegate GetDragDelegate(Grid grid)
        //{
        //    return (DragDelegate)grid.GetValue(DragOverCommandProperty);
        //}


        //public void SetDragDelegate(Grid grid, DragDelegate value)
        //{
        //    grid.SetValue(DragOverCommandProperty, value);
        //}

        //public static DragDelegate GetCommandProperty(DependencyObject obj)
        //{
        //    return (DragDelegate)obj.GetValue(DragOverCommandProperty);
        //}

        public ICommand DragOverCommand { get { return (ICommand)GetValue(DragOverCommandProperty); } set { SetValue(DragOverCommandProperty, value); } }
        public static readonly DependencyProperty DragOverCommandProperty = DependencyProperty.Register("DragOverCommand", typeof(ICommand), typeof(DragBehavior), new PropertyMetadata(DragChanged));

        //public ICommand DropCommand { get { return (ICommand)GetValue(DropCommandProperty); } set { SetValue(DropCommandProperty, value); } }
        //public static readonly DependencyProperty DropCommandProperty = DependencyProperty.Register("DropCommand", typeof(ICommand), typeof(DragBehavior), new PropertyMetadata(null));

        //protected override void OnAttached()
        //{
        //    base.OnAttached();
        //    this.AssociatedObject.AllowDrop = true;
        //    this.AssociatedObject.PreviewMouseLeftButtonDown += MouseLeftButtonDown;
        //    this.AssociatedObject.PreviewMouseLeftButtonUp += MouseLeftButtonUp;
        //    this.AssociatedObject.PreviewMouseMove += MouseMove;
        //}

        private static void DragChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Console.WriteLine("ok");
            var element = (UIElement)d;
            var isDrag = (bool)(e.NewValue);

            Instance = new DragBehavior();
            ((UIElement)d).RenderTransform = Instance.Transform;

            if (isDrag)
            {
                element.MouseLeftButtonDown += Instance.MouseLeftButtonDown;
                element.MouseLeftButtonUp += Instance.MouseLeftButtonUp;
                element.MouseMove += Instance.MouseMove;
            }
            else
            {
                element.MouseLeftButtonDown -= Instance.MouseLeftButtonDown;
                element.MouseLeftButtonUp -= Instance.MouseLeftButtonUp;
                element.MouseMove -= Instance.MouseMove;
            }
        }

        //private static void OnDragChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var element = (UIElement)sender;
        //    var isDrag = (bool)(e.NewValue);

        //    Instance = new DragBehavior();
        //    ((UIElement)sender).RenderTransform = Instance.Transform;

        //    if (isDrag)
        //    {
        //        element.MouseLeftButtonDown += Instance.MouseLeftButtonDown;
        //        element.MouseLeftButtonUp += Instance.MouseLeftButtonUp;
        //        element.MouseMove += Instance.MouseMove;
        //    }
        //    else
        //    {
        //        element.MouseLeftButtonDown -= Instance.MouseLeftButtonDown;
        //        element.MouseLeftButtonUp -= Instance.MouseLeftButtonUp;
        //        element.MouseMove -= Instance.MouseMove;
        //    }
        //}

        //protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    base.OnPropertyChanged(e);
        //}

        //protected override void OnDetaching()
        //{
        //    base.OnDetaching();
        //    this.AssociatedObject.PreviewMouseLeftButtonDown -= MouseLeftButtonDown;
        //    this.AssociatedObject.PreviewMouseLeftButtonUp -= MouseLeftButtonUp;
        //    this.AssociatedObject.PreviewMouseMove -= MouseMove;
        //}


        public void MouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            //ItemsControl itemsControl = (ItemsControl)sender;
            //Point p = args.GetPosition(itemsControl);

            //FrameworkElement element = sender as FrameworkElement;
            //Grid parent = element.FindAncestor<Grid>();
            //Point point = args.GetPosition(parent);
            //_mouseStartPosition = args.GetPosition(parent);
            //((UIElement)sender).CaptureMouse();
           
            if(this.DragOverCommand != null)
            {
                var grid = sender as Grid;
            }
           



        }

        public void MouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            ((UIElement)sender).ReleaseMouseCapture();
            FrameworkElement element = sender as FrameworkElement;
            if (element is null) return;
            Grid parent = element.FindAncestor<Grid>();
            Point pos = mouseButtonEventArgs.GetPosition(parent);
            //(int col, int row) = RowAndColumnCalculator.GetRowColumn(parent, pos);


            //Object target = element.GetValue(DragTargetProperty);
            //IDragBehavior dragTarget = target as IDragBehavior;
            //if (!(dragTarget is null))
            //{
            //    dragTarget.OnDrag(1, 2);
            //}


            _elementStartPosition.X = Transform.X;
            _elementStartPosition.Y = Transform.Y;
        }

        public void MouseMove(object sender, MouseEventArgs mouseEventArgs)
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


        //public readonly TranslateTransform Transform = new TranslateTransform();
        //private static DragBehavior _instance = new DragBehavior();

        //public static DragBehavior Instance { get { return _instance; } set { _instance = value; } }

        //private Point _elementStartPosition;
        //private Point _mouseStartPosition;

        //public static readonly DependencyProperty IsDragEnabledProperty =
        //    DependencyProperty.RegisterAttached("IsDragEnabled", typeof(bool), typeof(DragBehavior), new PropertyMetadata(false,OnDragChanged));

        //public static readonly DependencyProperty DragTargetProperty =
        //    DependencyProperty.RegisterAttached("DragTarget", typeof(object), typeof(DragBehavior), null);

        //public static bool GetDragTarget(DependencyObject obj)
        //{
        //    return (bool)obj.GetValue(DragTargetProperty);
        //}

        //public static void SetDragTarget(DependencyObject obj, bool value)
        //{
        //    obj.SetValue(DragTargetProperty, value);
        //}

        //public static bool GetIsDragEnable(DependencyObject obj)
        //{
        //    return (bool)obj.GetValue(IsDragEnabledProperty);
        //}

        //public static void SetIsDragEnabled(DependencyObject obj, bool value)
        //{
        //    obj.SetValue(IsDragEnabledProperty, value);
        //}

        //private static void OnDragChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var element = (UIElement)sender;
        //    var isDrag = (bool)(e.NewValue);

        //    Instance = new DragBehavior();
        //    ((UIElement)sender).RenderTransform = Instance.Transform;

        //    if (isDrag)
        //    {
        //        element.MouseLeftButtonDown += Instance.ElementOnMouseLeftButtonDown;
        //        element.MouseLeftButtonUp += Instance.ElementOnMouseLeftButtonUp;
        //        element.MouseMove += Instance.ElementOnMouseMove;
        //    }
        //    else
        //    {
        //        element.MouseLeftButtonDown -= Instance.ElementOnMouseLeftButtonDown;
        //        element.MouseLeftButtonUp -= Instance.ElementOnMouseLeftButtonUp;
        //        element.MouseMove -= Instance.ElementOnMouseMove;
        //    }
        //}

        //private void ElementOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtoEventArgs)
        //{
        //    FrameworkElement element = sender as FrameworkElement;
        //    Grid parent = element.FindAncestor<Grid>();
        //    //var parent = Application.Current.MainWindow;
        //    _mouseStartPosition = mouseButtoEventArgs.GetPosition(parent);
        //    ((UIElement)sender).CaptureMouse();
        //}

        //private void ElementOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        //{
        //    ((UIElement)sender).ReleaseMouseCapture();
        //    FrameworkElement element = sender as FrameworkElement;
        //    if (element is null) return;
        //    Grid parent = element.FindAncestor<Grid>();
        //    Point pos = mouseButtonEventArgs.GetPosition(parent);
        //    (int col, int row) = RowAndColumnCalculator.GetRowColumn(parent, pos);


        //    Object target = element.GetValue(DragTargetProperty);
        //    IDragBehavior dragTarget = target as IDragBehavior;
        //    if (!(dragTarget is null))
        //    {
        //        dragTarget.OnDrag(1, 2);
        //    }


        //    _elementStartPosition.X = Transform.X;
        //    _elementStartPosition.Y = Transform.Y;
        //}

        //private void ElementOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        //{
        //    FrameworkElement element = sender as FrameworkElement;
        //    Grid parent = element.FindAncestor<Grid>();
        //    var mousePos = mouseEventArgs.GetPosition(parent);
        //    var diff = (mousePos - _mouseStartPosition);
        //    if (!((UIElement)sender).IsMouseCaptured) return;
        //    //if(mousePos.X < parent.Width && mousePos.Y < parent.Height && mousePos.X >= 0 && mousePos.Y >= 0)
        //    //{

        //    //}


        //    Transform.X = diff.X;
        //    Transform.Y = diff.Y;
        //    //Transform.X = _elementStartPosition.X + diff.X;
        //    //Transform.Y = _elementStartPosition.Y + diff.Y;
        //}



    }
}
