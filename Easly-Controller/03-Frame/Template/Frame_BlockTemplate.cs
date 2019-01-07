using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFrameBlockTemplate : IFrameTemplate
    {
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view containing <paramref name="blockStateView"/> for which to create cells.</param>
        /// <param name="blockStateView">The block state view for which to create cells.</param>
        IFrameCellView BuildBlockCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameBlockStateView blockStateView);
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public class FrameBlockTemplate : FrameTemplate, IFrameBlockTemplate
    {
        #region Properties
        /// <summary>
        /// Checks that a template and all its frames are valid.
        /// </summary>
        public override bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(NodeName))
                    return false;

                if (Root == null)
                    return false;

                if (Root.ParentFrame == null)
                    return false;

                return true;
            }
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view containing <paramref name="blockStateView"/> for which to create cells.</param>
        /// <param name="blockStateView">The block state view for which to create cells.</param>
        public virtual IFrameCellView BuildBlockCells(IFrameControllerView controllerView, IFrameNodeStateView stateView, IFrameBlockStateView blockStateView)
        {
            IFrameBlockFrame BlockFrame = Root as IFrameBlockFrame;
            Debug.Assert(BlockFrame != null);

            return BlockFrame.BuildBlockCells(controllerView, stateView, blockStateView);
        }
        #endregion

        #region Debugging
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return "Node Template {" + NodeName + "}";
        }
        #endregion
    }
}
