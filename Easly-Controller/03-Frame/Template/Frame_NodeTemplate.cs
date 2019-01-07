using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFrameNodeTemplate : IFrameTemplate
    {
        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView);
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public class FrameNodeTemplate : FrameTemplate, IFrameNodeTemplate
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
        /// <param name="stateView">The state view for which to create cells.</param>
        public virtual IFrameCellView BuildNodeCells(IFrameControllerView controllerView, IFrameNodeStateView stateView)
        {
            IFrameNodeFrame NodeFrame = Root as IFrameNodeFrame;
            Debug.Assert(NodeFrame != null);

            return NodeFrame.BuildNodeCells(controllerView, stateView, null);
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
