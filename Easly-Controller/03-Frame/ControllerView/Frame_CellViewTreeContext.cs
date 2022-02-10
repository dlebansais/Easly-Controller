namespace EaslyController.Frame
{
    using System.Diagnostics;

    /// <summary>
    /// Context used when building the cell view tree.
    /// </summary>
    public interface IFrameCellViewTreeContext
    {
        /// <summary>
        /// The view in which cells are created.
        /// </summary>
        FrameControllerView ControllerView { get; }

        /// <summary>
        /// The state view for which to create cells.
        /// </summary>
        IFrameNodeStateView StateView { get; }

        /// <summary>
        /// The block state view for which to create cells. Can be null.
        /// </summary>
        FrameBlockStateView BlockStateView { get; }

        /// <summary>
        /// Changes the state view to that of a child state.
        /// </summary>
        /// <param name="childStateView">The new state view.</param>
        void SetChildStateView(IFrameNodeStateView childStateView);

        /// <summary>
        /// Changes the state view to that of the parent state.
        /// </summary>
        /// <param name="parentStateView">The new state view.</param>
        void RestoreParentStateView(IFrameNodeStateView parentStateView);

        /// <summary>
        /// Changes the block state view.
        /// </summary>
        /// <param name="blockStateView">The new block state view.</param>
        void SetBlockStateView(FrameBlockStateView blockStateView);
    }

    /// <summary>
    /// Context used when building the cell view tree.
    /// </summary>
    internal class FrameCellViewTreeContext : IFrameCellViewTreeContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameCellViewTreeContext"/> class.
        /// </summary>
        /// <param name="controllerView">The view in which cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        public FrameCellViewTreeContext(FrameControllerView controllerView, IFrameNodeStateView stateView)
        {
            ControllerView = controllerView;
            StateView = stateView;
            BlockStateView = null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The view in which cells are created.
        /// </summary>
        public FrameControllerView ControllerView { get; }

        /// <summary>
        /// The state view for which to create cells.
        /// </summary>
        public IFrameNodeStateView StateView { get; private set; }

        /// <summary>
        /// The block state view for which to create cells. Can be null.
        /// </summary>
        public FrameBlockStateView BlockStateView { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Changes the state view to that of a child state.
        /// </summary>
        /// <param name="childStateView">The new state view.</param>
        public void SetChildStateView(IFrameNodeStateView childStateView)
        {
            Debug.Assert(childStateView != StateView);
            Debug.Assert(childStateView.State.ParentState == StateView.State);

            StateView = childStateView;
        }

        /// <summary>
        /// Changes the state view to that of the parent state.
        /// </summary>
        /// <param name="parentStateView">The new state view.</param>
        public void RestoreParentStateView(IFrameNodeStateView parentStateView)
        {
            Debug.Assert(parentStateView != StateView);
            Debug.Assert(parentStateView.State == StateView.State.ParentState);

            StateView = parentStateView;
        }

        /// <summary>
        /// Changes the block state view.
        /// </summary>
        /// <param name="blockStateView">The new block state view.</param>
        public void SetBlockStateView(FrameBlockStateView blockStateView)
        {
            BlockStateView = blockStateView;
        }
        #endregion
    }
}
