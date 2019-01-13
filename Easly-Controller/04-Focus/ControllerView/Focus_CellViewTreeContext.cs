using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Context used when building the cell view tree.
    /// </summary>
    public interface IFocusCellViewTreeContext : IFrameCellViewTreeContext
    {
        /// <summary>
        /// The view in which cells are created.
        /// </summary>
        new IFocusControllerView ControllerView { get; }

        /// <summary>
        /// The state view for which to create cells.
        /// </summary>
        new IFocusNodeStateView StateView { get; }

        /// <summary>
        /// The block state view for which to create cells. Can be null.
        /// </summary>
        new IFocusBlockStateView BlockStateView { get; }
    }

    /// <summary>
    /// Context used when building the cell view tree.
    /// </summary>
    public class FocusCellViewTreeContext : FrameCellViewTreeContext, IFocusCellViewTreeContext
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FocusCellViewTreeContext"/> object.
        /// </summary>
        /// <param name="controllerView">The view in which cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        public FocusCellViewTreeContext(IFrameControllerView controllerView, IFrameNodeStateView stateView)
            : base(controllerView, stateView)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The view in which cells are created.
        /// </summary>
        public new IFocusControllerView ControllerView { get { return (IFocusControllerView)base.ControllerView; } }

        /// <summary>
        /// The state view for which to create cells.
        /// </summary>
        public new IFocusNodeStateView StateView { get { return (IFocusNodeStateView)base.StateView; } }

        /// <summary>
        /// The block state view for which to create cells. Can be null.
        /// </summary>
        public new IFocusBlockStateView BlockStateView { get { return (IFocusBlockStateView)base.BlockStateView; } }
        #endregion
    }
}
