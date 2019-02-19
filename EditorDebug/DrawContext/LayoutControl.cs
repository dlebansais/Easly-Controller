namespace EditorDebug
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Media;
    using EaslyController.Layout;
    using EaslyDraw;
    using TestDebug;

    public class LayoutControl : FrameworkElement
    {
        public void SetController(ILayoutController controller)
        {
            Controller = controller;

            DrawingVisual = new DrawingVisual();
            DrawingContext = DrawingVisual.RenderOpen();
            DrawContext = new DrawContext();

            ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, DrawContext);
            ControllerView.SetCommentDisplayMode(EaslyController.Constants.CommentDisplayModes.All);
            ControllerView.SetShowUnfocusedComments(show: true);
            ControllerView.MeasureAndArrange();
            ControllerView.ShowCaret(true, draw: false);

            DrawingContext.Close();

            InvalidateMeasure();
            InvalidateVisual();
        }

        public void OnActivated()
        {
            if (ControllerView != null)
            {
                ControllerView.ShowCaret(true, draw: false);
                InvalidateVisual();
            }
        }

        public void OnDeactivated()
        {
            if (ControllerView != null)
            {
                ControllerView.ShowCaret(false, draw: true);
            }
        }

        private ILayoutController Controller;
        private DrawingVisual DrawingVisual;
        private DrawingContext DrawingContext;
        private DrawContext DrawContext;

        public ILayoutControllerView ControllerView { get; private set; }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (ControllerView != null)
                return new Size(ControllerView.ViewSize.Width, ControllerView.ViewSize.Height);
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

                ControllerView.Draw();
            }
        }
    }
}
