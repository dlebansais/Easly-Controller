using System;
using System.Diagnostics;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base frame.
    /// </summary>
    public interface IFrameFrame
    {
        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        IFrameFrame ParentFrame { get; }

        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        bool IsValid(Type nodeType);

        /// <summary>
        /// Update the reference to the parent frame.
        /// </summary>
        /// <param name="parentFrame">The parent frame.</param>
        void UpdateParentFrame(IFrameFrame parentFrame);
    }

    /// <summary>
    /// Base frame.
    /// </summary>
    public abstract class FrameFrame : IFrameFrame
    {
        #region Properties
        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public IFrameFrame ParentFrame { get; private set; }
        #endregion

        #region Client Interface
        public virtual bool IsValid(Type nodeType)
        {
            return true;
        }

        /// <summary>
        /// Update the reference to the parent frame.
        /// </summary>
        /// <param name="parentFrame">The parent frame.</param>
        public virtual void UpdateParentFrame(IFrameFrame parentFrame)
        {
            Debug.Assert(parentFrame != null);
            Debug.Assert(ParentFrame == null);

            ParentFrame = parentFrame;
        }
        #endregion

        #region Debugging
        public override string ToString()
        {
            return base.ToString() + " (" + GetHashCode() + ")";
        }
        #endregion
    }
}
