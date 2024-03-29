﻿namespace EaslyController.Focus
{
    using EaslyController.Frame;

    /// <summary>
    /// View of a IxxxController.
    /// </summary>
    public partial class FocusControllerView : FrameControllerView, IFocusInternalControllerView
    {
        /// <summary>
        /// Gets the empty <see cref="FocusControllerView"/> object.
        /// </summary>
        public static new FocusControllerView Empty { get; } = new FocusControllerView();

        /// <summary>
        /// Creates and initializes a new instance of a <see cref="FocusControllerView"/> object.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        public static FocusControllerView Create(FocusController controller, IFocusTemplateSet templateSet)
        {
            FocusControllerView View = new FocusControllerView(controller, templateSet);
            View.Init();
            return View;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusControllerView"/> class.
        /// </summary>
        protected FocusControllerView()
            : this(FocusController.Empty, FocusTemplateSet.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FocusControllerView"/> class.
        /// </summary>
        /// <param name="controller">The controller on which the view is attached.</param>
        /// <param name="templateSet">The template set used to describe the view.</param>
        private protected FocusControllerView(FocusController controller, IFocusTemplateSet templateSet)
            : base(controller, templateSet)
        {
        }

        /// <summary>
        /// Initializes the view by attaching it to the controller.
        /// </summary>
        private protected override void Init()
        {
            base.Init();

            ForcedCommentStateView = null;
            EmptySelection = CreateEmptySelection();
            Selection = EmptySelection;
        }
    }
}
