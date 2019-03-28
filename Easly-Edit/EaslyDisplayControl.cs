namespace EaslyEdit
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using EaslyController.Constants;
    using EaslyController.Layout;
    using EaslyDraw;

    /// <summary>
    /// A control to display, but not edit, Easly source code.
    /// </summary>
    public class EaslyDisplayControl : Control
    {
        #region Custom properties and events
        #region Controller
        /// <summary>
        /// Identifies the <see cref="Controller"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ControllerProperty = DependencyProperty.Register("Controller", typeof(ILayoutController), typeof(EaslyDisplayControl), new PropertyMetadata(ControllerPropertyChangedCallback));

        /// <summary>
        /// Gets or sets the source code controller.
        /// </summary>
        public ILayoutController Controller
        {
            get { return (ILayoutController)GetValue(ControllerProperty); }
            set { SetValue(ControllerProperty, value); }
        }

        /// <summary></summary>
        protected private static void ControllerPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyDisplayControl ctrl = (EaslyDisplayControl)d;
            if (ctrl.Controller != e.OldValue)
                ctrl.OnControllerPropertyChanged(e);
        }

        /// <summary></summary>
        protected virtual void OnControllerPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            Cleanup();
            Initialize();

            InvalidateMeasure();
            InvalidateVisual();
        }
        #endregion
        #region TemplateSet
        /// <summary>
        /// Identifies the <see cref="TemplateSet"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TemplateSetProperty = DependencyProperty.Register("TemplateSet", typeof(ILayoutTemplateSet), typeof(EaslyDisplayControl), new PropertyMetadata(CustomLayoutTemplateSet.LayoutTemplateSet, TemplateSetPropertyChangedCallback));

        /// <summary>
        /// Gets or sets display templates.
        /// </summary>
        public ILayoutTemplateSet TemplateSet
        {
            get { return (ILayoutTemplateSet)GetValue(TemplateSetProperty); }
            set { SetValue(TemplateSetProperty, value); }
        }

        protected private static void TemplateSetPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyDisplayControl ctrl = (EaslyDisplayControl)d;
            if (ctrl.TemplateSet != e.OldValue)
                ctrl.OnTemplateSetPropertyChanged(e);
        }

        protected private virtual void OnTemplateSetPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            Cleanup();
            Initialize();

            InvalidateMeasure();
            InvalidateVisual();
        }
        #endregion
        #region CommentDisplayMode
        /// <summary>
        /// Identifies the <see cref="CommentDisplayMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CommentDisplayModeProperty = DependencyProperty.Register("CommentDisplayMode", typeof(CommentDisplayModes), typeof(EaslyDisplayControl), new PropertyMetadata(CommentDisplayModes.Tooltip, CommentDisplayModePropertyChangedCallback));

        /// <summary>
        /// Gets or sets the comment display mode.
        /// </summary>
        public CommentDisplayModes CommentDisplayMode
        {
            get { return (CommentDisplayModes)GetValue(CommentDisplayModeProperty); }
            set { SetValue(CommentDisplayModeProperty, value); }
        }

        protected private static void CommentDisplayModePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyDisplayControl ctrl = (EaslyDisplayControl)d;
            if (ctrl.CommentDisplayMode != (CommentDisplayModes)e.OldValue)
                ctrl.OnCommentDisplayModePropertyChanged(e);
        }

        protected private virtual void OnCommentDisplayModePropertyChanged(DependencyPropertyChangedEventArgs e)
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
        /// <summary>
        /// Identifies the <see cref="ShowUnfocusedComments"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowUnfocusedCommentsProperty = DependencyProperty.Register("ShowUnfocusedComments", typeof(bool), typeof(EaslyDisplayControl), new PropertyMetadata(true, ShowUnfocusedCommentsPropertyChangedCallback));

        /// <summary>
        /// Gets or sets displaying unfocused comments.
        /// </summary>
        public bool ShowUnfocusedComments
        {
            get { return (bool)GetValue(ShowUnfocusedCommentsProperty); }
            set { SetValue(ShowUnfocusedCommentsProperty, value); }
        }

        protected private static void ShowUnfocusedCommentsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyDisplayControl ctrl = (EaslyDisplayControl)d;
            if (ctrl.ShowUnfocusedComments != (bool)e.OldValue)
                ctrl.OnShowUnfocusedCommentsPropertyChanged(e);
        }

        protected private virtual void OnShowUnfocusedCommentsPropertyChanged(DependencyPropertyChangedEventArgs e)
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
        /// <summary>
        /// Identifies the <see cref="ShowBlockGeometry"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowBlockGeometryProperty = DependencyProperty.Register("ShowBlockGeometry", typeof(bool), typeof(EaslyDisplayControl), new PropertyMetadata(false, ShowBlockGeometryPropertyChangedCallback));

        /// <summary>
        /// Gets or sets displaying a geometry around blocks.
        /// </summary>
        public bool ShowBlockGeometry
        {
            get { return (bool)GetValue(ShowBlockGeometryProperty); }
            set { SetValue(ShowBlockGeometryProperty, value); }
        }

        protected private static void ShowBlockGeometryPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyDisplayControl ctrl = (EaslyDisplayControl)d;
            if (ctrl.ShowBlockGeometry != (bool)e.OldValue)
                ctrl.OnShowBlockGeometryPropertyChanged(e);
        }

        protected private virtual void OnShowBlockGeometryPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ControllerView != null)
            {
                ControllerView.SetShowBlockGeometry(ShowBlockGeometry);

                InvalidateMeasure();
                InvalidateVisual();
            }
        }
        #endregion
        #region ShowLineNumber
        /// <summary>
        /// Identifies the <see cref="ShowLineNumber"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowLineNumberProperty = DependencyProperty.Register("ShowLineNumber", typeof(bool), typeof(EaslyDisplayControl), new PropertyMetadata(false, ShowLineNumberPropertyChangedCallback));

        /// <summary>
        /// Gets or sets displaying line number.
        /// </summary>
        public bool ShowLineNumber
        {
            get { return (bool)GetValue(ShowLineNumberProperty); }
            set { SetValue(ShowLineNumberProperty, value); }
        }

        protected private static void ShowLineNumberPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EaslyDisplayControl ctrl = (EaslyDisplayControl)d;
            if (ctrl.ShowLineNumber != (bool)e.OldValue)
                ctrl.OnShowLineNumberPropertyChanged(e);
        }

        protected private virtual void OnShowLineNumberPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ControllerView != null)
            {
                ControllerView.SetShowLineNumber(ShowLineNumber);

                InvalidateMeasure();
                InvalidateVisual();
            }
        }
        #endregion
        #endregion

        #region Implementation
        protected private virtual void Initialize()
        {
            if (IsReady)
            {
                DrawContext = DrawContext.CreateDrawContext(CreateTypeface(), FontSize, hasCommentIcon: false, displayFocus: false);
                ControllerView = LayoutControllerView.Create(Controller, TemplateSet, DrawContext);
                InitializeProperties();
            }
        }

        protected private virtual bool IsReady
        {
            get { return Controller != null && TemplateSet != null && FontFamily != null && FontSize > 0; }
        }

        protected private virtual Typeface CreateTypeface()
        {
            return new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
        }

        protected private virtual void InitializeProperties()
        {
            ControllerView.SetCommentDisplayMode(CommentDisplayMode);
            ControllerView.SetShowUnfocusedComments(ShowUnfocusedComments);
            ControllerView.SetShowBlockGeometry(ShowBlockGeometry);
            ControllerView.SetShowLineNumber(ShowLineNumber);
        }

        protected private virtual void Cleanup()
        {
            DrawContext = null;
            ControllerView = null;
        }


        protected private DrawContext DrawContext;
        protected private ILayoutControllerView ControllerView;
        #endregion

        #region Overrides
        /// <summary>
        /// Called to remeasure a control.
        /// </summary>
        /// <param name="availableSize">The maximum size that the method can return.</param>
        /// <returns>The size of the control, up to the maximum specified by constraint.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            if (ControllerView != null)
            {
                EaslyController.Controller.Padding PagePadding = ControllerView.DrawContext.PagePadding;

                double Width = ControllerView.ViewSize.Width.Draw + (SystemParameters.ThinVerticalBorderWidth * 2) + PagePadding.Left.Draw + PagePadding.Right.Draw;
                double Height = ControllerView.ViewSize.Height.Draw + (SystemParameters.ThinHorizontalBorderHeight * 2) + PagePadding.Top.Draw + PagePadding.Bottom.Draw;

                if (Width > availableSize.Width)
                    Width = availableSize.Width;

                if (Height > availableSize.Height)
                    Height = availableSize.Height;

                return new Size(Width, Height);
            }
            else
                return base.MeasureOverride(availableSize);
        }

        /// <summary>
        /// When overridden in a derived class, participates in rendering operations that are directed by the layout system. The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing.
        /// </summary>
        /// <param name="dc">The drawing instructions for a specific element. This context is provided to the layout system.</param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (ControllerView != null)
            {
                DrawContext.SetWpfDrawingContext(dc);

                Brush WriteBrush = Brushes.White;
                Rect Fullrect = new Rect(0, 0, ActualWidth, ActualHeight);
                dc.DrawRectangle(Background, null, Fullrect);

                ControllerView.Draw(ControllerView.RootStateView);
            }
        }
        #endregion
    }
}
