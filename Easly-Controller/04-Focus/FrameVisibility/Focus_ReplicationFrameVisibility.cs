using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Frame visibility that depends if the current block is replicated.
    /// </summary>
    public interface IFocusReplicationFrameVisibility : IFocusFrameVisibility, IFocusBlockFrameVisibility
    {
    }

    /// <summary>
    /// Frame visibility that depends if the current block is replicated.
    /// </summary>
    public class FocusReplicationFrameVisibility : FocusFrameVisibility, IFocusReplicationFrameVisibility
    {
        #region Properties
        /// <summary>
        /// True if the visibility depends on the show/hidden state of the view with the focus.
        /// </summary>
        public override bool IsVolatile { get { return false; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame visibility is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame visibility can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            return true;
        }

        /// <summary>
        /// Is the associated frame visible.
        /// </summary>
        /// <param name="context">Context used to build the cell view tree.</param>
        /// <param name="frame">The frame with the associated visibility.</param>
        public virtual bool IsBlockVisible(IFocusCellViewTreeContext context, IFocusBlockFrame frame)
        {
            if (!context.ControllerView.IsInReplicatedBlock(context.BlockStateView))
                return false;

            return true;
        }
        #endregion
    }
}
