using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BaseNode;
using EaslyController.Constants;
using EaslyController.Layout;
using EaslyDraw;
using TestDebug;

namespace EditorDebug
{
    public class EaslyDisplayControl : Control
    {
        #region Custom properties and events
        #region Content
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(INode), typeof(EaslyDisplayControl), new PropertyMetadata(ContentPropertyChangedCallback));

        public INode Content
        {
            get { return (INode)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        protected static void ContentPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyDisplayControl ctrl = (EaslyDisplayControl)d;
            if (ctrl.Content != e.OldValue)
                ctrl.OnContentPropertyChanged(e);
        }

        protected virtual void OnContentPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            Cleanup();
            Initialize();

            InvalidateMeasure();
            InvalidateVisual();
        }
        #endregion
        #region TemplateSet
        public static readonly DependencyProperty TemplateSetProperty = DependencyProperty.Register("TemplateSet", typeof(ILayoutTemplateSet), typeof(EaslyDisplayControl), new PropertyMetadata(CustomLayoutTemplateSet.LayoutTemplateSet, TemplateSetPropertyChangedCallback));

        public ILayoutTemplateSet TemplateSet
        {
            get { return (ILayoutTemplateSet)GetValue(TemplateSetProperty); }
            set { SetValue(TemplateSetProperty, value); }
        }

        protected static void TemplateSetPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyDisplayControl ctrl = (EaslyDisplayControl)d;
            if (ctrl.TemplateSet != e.OldValue)
                ctrl.OnTemplateSetPropertyChanged(e);
        }

        protected virtual void OnTemplateSetPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            Cleanup();
            Initialize();

            InvalidateMeasure();
            InvalidateVisual();
        }
        #endregion
        #region CommentDisplayMode
        public static readonly DependencyProperty CommentDisplayModeProperty = DependencyProperty.Register("CommentDisplayMode", typeof(CommentDisplayModes), typeof(EaslyDisplayControl), new PropertyMetadata(CommentDisplayModes.Tooltip, CommentDisplayModePropertyChangedCallback));

        public CommentDisplayModes CommentDisplayMode
        {
            get { return (CommentDisplayModes)GetValue(CommentDisplayModeProperty); }
            set { SetValue(CommentDisplayModeProperty, value); }
        }

        protected static void CommentDisplayModePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyDisplayControl ctrl = (EaslyDisplayControl)d;
            if (ctrl.CommentDisplayMode != (CommentDisplayModes)e.OldValue)
                ctrl.OnCommentDisplayModePropertyChanged(e);
        }

        protected virtual void OnCommentDisplayModePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ControllerView != null)
            {
                ControllerView.SetCommentDisplayMode(CommentDisplayMode);

                InvalidateMeasure();
                InvalidateVisual();
            }
        }
        #endregion
        #region ShowUnfocusedComments
        public static readonly DependencyProperty ShowUnfocusedCommentsProperty = DependencyProperty.Register("ShowUnfocusedComments", typeof(bool), typeof(EaslyDisplayControl), new PropertyMetadata(true, ShowUnfocusedCommentsPropertyChangedCallback));

        public bool ShowUnfocusedComments
        {
            get { return (bool)GetValue(ShowUnfocusedCommentsProperty); }
            set { SetValue(ShowUnfocusedCommentsProperty, value); }
        }

        protected static void ShowUnfocusedCommentsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyDisplayControl ctrl = (EaslyDisplayControl)d;
            if (ctrl.ShowUnfocusedComments != (bool)e.OldValue)
                ctrl.OnShowUnfocusedCommentsPropertyChanged(e);
        }

        protected virtual void OnShowUnfocusedCommentsPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ControllerView != null)
            {
                ControllerView.SetShowUnfocusedComments(ShowUnfocusedComments);

                InvalidateMeasure();
                InvalidateVisual();
            }
        }
        #endregion
        #region ShowBlockGeometry
        public static readonly DependencyProperty ShowBlockGeometryProperty = DependencyProperty.Register("ShowBlockGeometry", typeof(bool), typeof(EaslyDisplayControl), new PropertyMetadata(false, ShowBlockGeometryPropertyChangedCallback));

        public bool ShowBlockGeometry
        {
            get { return (bool)GetValue(ShowBlockGeometryProperty); }
            set { SetValue(ShowBlockGeometryProperty, value); }
        }

        protected static void ShowBlockGeometryPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyDisplayControl ctrl = (EaslyDisplayControl)d;
            if (ctrl.ShowBlockGeometry != (bool)e.OldValue)
                ctrl.OnShowBlockGeometryPropertyChanged(e);
        }

        protected virtual void OnShowBlockGeometryPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ControllerView != null)
            {
                ControllerView.SetShowBlockGeometry(ShowBlockGeometry);

                InvalidateMeasure();
                InvalidateVisual();
            }
        }
        #endregion
        #endregion

        protected virtual void Initialize()
        {
            if (IsReady)
            {
                LayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(Content);
                Controller = LayoutController.Create(RootIndex);
                DrawContext = DrawContext.CreateDrawContext(CreateTypeface(), FontSize, hasCommentIcon: false, displayFocus: false);
                ControllerView = LayoutControllerView.Create(Controller, TemplateSet, DrawContext);

                ControllerView.SetCommentDisplayMode(CommentDisplayMode);
            }
        }

        protected virtual bool IsReady
        {
            get { return Content != null && TemplateSet != null && FontFamily != null && FontSize > 0; }
        }

        protected virtual Typeface CreateTypeface()
        {
            return new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
        }

        protected virtual void Cleanup()
        {
            Controller = null;
            DrawContext = null;
            ControllerView = null;
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
