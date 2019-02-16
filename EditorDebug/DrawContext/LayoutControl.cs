namespace EditorDebug
{
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Media;
    using EaslyController.Layout;
    using TestDebug;

    public class LayoutControl : FrameworkElement
    {
        public void SetController(ILayoutController controller)
        {
            Controller = controller;

            DrawingVisual = new DrawingVisual();
            DrawingContext = DrawingVisual.RenderOpen();
            DrawContext = new LayoutDrawContext(DrawingContext);

            ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, DrawContext);
            ControllerView.MeasureAndArrange();

            DrawingContext.Close();

            InvalidateMeasure();
            InvalidateVisual();
        }

        private ILayoutController Controller;
        private DrawingVisual DrawingVisual;
        private DrawingContext DrawingContext;
        private LayoutDrawContext DrawContext;

        public ILayoutControllerView ControllerView { get; private set; }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (ControllerView != null)
            {
                Debug.Assert(!ControllerView.IsInvalidated);
                return new Size(ControllerView.ViewSize.Width, ControllerView.ViewSize.Height);
            }
            else
                return base.MeasureOverride(availableSize);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (ControllerView != null)
            {
                DrawContext.UpdateDC(dc);

                //Debug.WriteLine($"OnRender, view size={Size}");

                Brush WriteBrush = Brushes.White;
                Rect Fullrect = new Rect(0, 0, ActualWidth, ActualHeight);
                dc.DrawRectangle(WriteBrush, null, Fullrect);

                ControllerView.Draw();
                ControllerView.ShowCaret(true);
            }
        }
    }
}
