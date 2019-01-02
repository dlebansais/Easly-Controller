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

                return true;
            }
        }
        #endregion

        #region Ancestor Interface
        public override string ToString()
        {
            return "Template {" + NodeName + "}";
        }
        #endregion
    }
}
