﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BaseNode;
using EaslyController.Layout;
using EaslyDraw;
using TestDebug;

namespace EditorDebug
{
    public class EaslyDisplayControl : Control
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(INode), typeof(EaslyDisplayControl), new PropertyMetadata(ContentPropertyChangedCallback));

        public INode Content
        {
            get { return (INode)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        private static void ContentPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((EaslyDisplayControl)d).OnContentPropertyChanged(e);
        }

        protected virtual void OnContentPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Content != null)
            {
                LayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(Content);
                Controller = LayoutController.Create(RootIndex);
                DrawContext = DrawContext.CreateDrawContext(hasCommentIcon: false, displayFocus: false);
                ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, DrawContext);
            }
            else
            {
                Controller = null;
                DrawContext = null;
                ControllerView = null;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (ControllerView != null)
            {
                EaslyController.Controller.Padding PagePadding = ControllerView.DrawContext.PagePadding;
                return new Size(ControllerView.ViewSize.Width.Draw + (SystemParameters.ThinVerticalBorderWidth * 2) + PagePadding.Left.Draw + PagePadding.Right.Draw, ControllerView.ViewSize.Height.Draw + (SystemParameters.ThinHorizontalBorderHeight * 2) + PagePadding.Top.Draw + PagePadding.Bottom.Draw);
            }
            else
                return base.MeasureOverride(availableSize);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (ControllerView != null)
            {
                DrawContext.SetWpfDrawingContext(dc);

                Brush WriteBrush = Brushes.White;
                Rect Fullrect = new Rect(0, 0, ActualWidth, ActualHeight);
                dc.DrawRectangle(WriteBrush, null, Fullrect);

                ControllerView.Draw(ControllerView.RootStateView);
            }
        }

        public ILayoutController Controller { get; protected set; }
        public DrawContext DrawContext { get; protected set; }
        public ILayoutControllerView ControllerView { get; protected set; }
    }
}
