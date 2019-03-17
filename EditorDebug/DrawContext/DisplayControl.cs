using System.Windows;
using System.Windows.Media;
using BaseNode;
using EaslyController.Layout;
using EaslyDraw;
using TestDebug;

namespace EditorDebug
{
    public class DisplayControl : FrameworkElement
    {
        public static readonly DependencyProperty NodeProperty = DependencyProperty.Register("Node", typeof(INode), typeof(DisplayControl), new PropertyMetadata(null));

        public INode Node
        {
            get { return (INode)GetValue(NodeProperty); }
            set { SetValue(NodeProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Controller == null)
            {
                LayoutRootNodeIndex RootIndex = new LayoutRootNodeIndex(Node);
                Controller = LayoutController.Create(RootIndex);
                DrawContext = DrawContext.CreateDrawContext(hasCommentIcon: false);
                ControllerView = LayoutControllerView.Create(Controller, CustomLayoutTemplateSet.LayoutTemplateSet, DrawContext);
            }

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

        public ILayoutController Controller { get; private set; }
        public DrawContext DrawContext { get; private set; }
        public ILayoutControllerView ControllerView { get; private set; }
    }
}
