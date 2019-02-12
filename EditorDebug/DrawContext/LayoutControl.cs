namespace EditorDebug
{
    using System.Diagnostics;
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
            ControllerView = null;

            InvalidateVisual();
        }

        private ILayoutController Controller;
        private ILayoutControllerView ControllerView;

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (Controller != null && ControllerView == null)
            {
                LayoutDrawContext DrawContext = new LayoutDrawContext(dc);
                ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, DrawContext);
            }
            else if (ControllerView != null)
            {
                LayoutDrawContext DrawContext = ControllerView.DrawContext as LayoutDrawContext;
                DrawContext.UpdateDC(dc);
            }

            System.Windows.Size Size;

            if (ControllerView != null && MeasureHelper.IsValid(ControllerView.ViewSize) && ControllerView.ViewSize.IsVisible)
                Size = new System.Windows.Size(ControllerView.ViewSize.Width, ControllerView.ViewSize.Height);
            else
                Size = System.Windows.Size.Empty;

            //Debug.WriteLine($"OnRender, view size={Size}");

            Brush WriteBrush = Brushes.White;
            Rect Fullrect = new Rect(0, 0, ActualWidth, ActualHeight);
            dc.DrawRectangle(WriteBrush, null, Fullrect);

            if (Size != System.Windows.Size.Empty)
                UpdateLayoutView();
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
