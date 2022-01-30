namespace EaslyController.Layout
{
    using EaslyController.Controller;
    using EaslyController.Focus;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class LayoutControllerView : FocusControllerView, ILayoutInternalControllerView
    {
        /// <summary>
        /// Gets the empty <see cref="LayoutControllerView"/> object.
        /// </summary>
        public static new LayoutControllerView Empty { get; } = new LayoutControllerView();

        /// <summary>
        /// Creates and initializes a new instance of a <see cref="LayoutControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        /// <param name="context">The context used to measure, arrange, and draw or print the view.</param>
        public static LayoutControllerView Create(LayoutController controller, ILayoutTemplateSet templateSet, ILayoutMeasureContext context)
        {
            LayoutControllerView View = new LayoutControllerView(controller, templateSet, context);
            View.Init();
            return View;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutControllerView"/> class.
        /// </summary>
        protected LayoutControllerView()
            : this(LayoutController.Empty, LayoutTemplateSet.Default, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutControllerView"/> class.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        /// <param name="context">The context used to measure, arrange, and draw or print the view.</param>
        private protected LayoutControllerView(LayoutController controller, ILayoutTemplateSet templateSet, ILayoutMeasureContext context)
            : base(controller, templateSet)
        {
            MeasureContext = context;
            DrawContext = context as ILayoutDrawContext;
            PrintContext = context as ILayoutPrintContext;
            InternalViewSize = RegionHelper.InvalidSize;
            IsInvalidated = true;
            ShowUnfocusedComments = true;
        }
    }
}
