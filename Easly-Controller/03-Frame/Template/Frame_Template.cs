namespace EaslyController.Frame
{
    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFrameTemplate
    {
        /// <summary>
        /// Template name.
        /// (Set in Xaml)
        /// </summary>
        string NodeName { get; set; }

        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        IFrameFrame Root { get; set; }

        /// <summary>
        /// Checks that a template and all its frames are valid.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        IFrameCellView BuildCells(IFrameControllerView controllerView, IFrameNodeStateView stateView);
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public class FrameTemplate : IFrameTemplate
    {
        #region Properties
        /// <summary>
        /// Template name.
        /// (Set in Xaml)
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        public IFrameFrame Root { get; set; }

        /// <summary>
        /// Checks that a template and all its frames are valid.
        /// </summary>
        public virtual bool IsValid
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
        public virtual IFrameCellView BuildCells(IFrameControllerView controllerView, IFrameNodeStateView stateView)
        {
            return Root.BuildCells(controllerView, stateView);
        }
        #endregion

        #region Debugging
        public override string ToString()
        {
            return "Template {" + NodeName + "}";
        }
        #endregion
    }
}
