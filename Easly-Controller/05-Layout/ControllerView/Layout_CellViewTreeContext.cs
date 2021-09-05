namespace EaslyController.Layout
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using EaslyController.Focus;

    /// <summary>
    /// Context used when building the cell view tree.
    /// </summary>
    public interface ILayoutCellViewTreeContext : IFocusCellViewTreeContext
    {
        /// <summary>
        /// The view in which cells are created.
        /// </summary>
        new LayoutControllerView ControllerView { get; }

        /// <summary>
        /// The state view for which to create cells.
        /// </summary>
        new ILayoutNodeStateView StateView { get; }

        /// <summary>
        /// The block state view for which to create cells. Can be null.
        /// </summary>
        new LayoutBlockStateView BlockStateView { get; }
    }

    /// <summary>
    /// Context used when building the cell view tree.
    /// </summary>
    internal class LayoutCellViewTreeContext : FocusCellViewTreeContext, ILayoutCellViewTreeContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutCellViewTreeContext"/> class.
        /// </summary>
        /// <param name="controllerView">The view in which cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        /// <param name="forcedCommentStateView">The state view for which the comment must be visible, even if empty.</param>
        public LayoutCellViewTreeContext(LayoutControllerView controllerView, ILayoutNodeStateView stateView, ILayoutNodeStateView forcedCommentStateView)
            : base(controllerView, stateView, forcedCommentStateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The view in which cells are created.
        /// </summary>
        public new LayoutControllerView ControllerView { get { return (LayoutControllerView)base.ControllerView; } }

        /// <summary>
        /// The state view for which to create cells.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }

        /// <summary>
        /// The block state view for which to create cells. Can be null.
        /// </summary>
        public new LayoutBlockStateView BlockStateView { get { return (LayoutBlockStateView)base.BlockStateView; } }
        #endregion
    }
}
