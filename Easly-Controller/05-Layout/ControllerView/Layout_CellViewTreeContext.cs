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
        new ILayoutControllerView ControllerView { get; }

        /// <summary>
        /// The state view for which to create cells.
        /// </summary>
        new ILayoutNodeStateView StateView { get; }

        /// <summary>
        /// The block state view for which to create cells. Can be null.
        /// </summary>
        new ILayoutBlockStateView BlockStateView { get; }
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
        public LayoutCellViewTreeContext(IFocusControllerView controllerView, IFocusNodeStateView stateView)
            : base(controllerView, stateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The view in which cells are created.
        /// </summary>
        public new ILayoutControllerView ControllerView { get { return (ILayoutControllerView)base.ControllerView; } }

        /// <summary>
        /// The state view for which to create cells.
        /// </summary>
        public new ILayoutNodeStateView StateView { get { return (ILayoutNodeStateView)base.StateView; } }

        /// <summary>
        /// The block state view for which to create cells. Can be null.
        /// </summary>
        public new ILayoutBlockStateView BlockStateView { get { return (ILayoutBlockStateView)base.BlockStateView; } }
        #endregion
    }
}
