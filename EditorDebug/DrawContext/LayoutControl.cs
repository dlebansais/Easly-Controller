namespace EditorDebug
{
    using System.Windows;
    using System.Windows.Media;
    using EaslyController.Controller;
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

            InvalidateMeasure();
            InvalidateVisual();
        }

        private ILayoutController Controller;
        private DrawingVisual DrawingVisual;
        private DrawingContext DrawingContext;
        private LayoutDrawContext DrawContext;

        public ILayoutControllerView ControllerView { get; private set; }

        protected override System.Windows.Size MeasureOverride(System.Windows.Size availableSize)
        {
            if (ControllerView != null)
                return new System.Windows.Size(ControllerView.ViewSize.Width, ControllerView.ViewSize.Height);
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

                UpdateLayoutView();
            }
        }

        private void UpdateLayoutView()
        {
            ILayoutVisibleCellViewList CellList = new LayoutVisibleCellViewList();
            ControllerView.EnumerateVisibleCellViews(CellList);

            foreach (ILayoutVisibleCellView CellView in CellList)
                CellView.Draw();
        }
    }
}
