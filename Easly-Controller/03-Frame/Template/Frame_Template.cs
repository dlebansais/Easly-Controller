namespace EaslyController.Frame
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFrameTemplate
    {
        /// <summary>
        /// Type of the node associated to this template (an interface type).
        /// (Set in Xaml)
        /// </summary>
        Type NodeType { get; }

        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        IFrameFrame Root { get; }

        /// <summary>
        /// Checks that a template and all its frames are valid.
        /// </summary>
        bool IsValid { get; }
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public abstract class FrameTemplate : IFrameTemplate
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FrameTemplate"/> object.
        /// </summary>
        public static FrameTemplate Empty { get; } = new FrameNodeTemplate();
        #endregion

        #region Properties
        /// <summary>
        /// Type of the node associated to this template (an interface type).
        /// (Set in Xaml)
        /// </summary>
        public Type NodeType { get; set; }

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
                bool IsValid = true;

                IsValid &= NodeType != null;
                IsValid &= Root != null;
                IsValid &= IsRootValid;

                Debug.Assert(IsValid);
                return IsValid;
            }
        }

        private protected virtual bool IsRootValid { get { return Root.ParentFrame == FrameFrame.FrameRoot; } }
        #endregion
    }
}
