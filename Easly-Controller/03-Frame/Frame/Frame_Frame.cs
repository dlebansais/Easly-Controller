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

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        IFrameCellView BuildCells(IFrameControllerView controllerView, IFrameNodeStateView stateView);
    }

    /// <summary>
    /// Base frame.
    /// </summary>
    public abstract class FrameFrame : IFrameFrame
    {
        #region Init
        private class FrameRootFrame : IFrameFrame
        {
            public IFrameFrame ParentFrame { get { throw new InvalidOperationException(); } }
            public bool IsValid(Type nodeType) { return false; }
            public void UpdateParentFrame(IFrameFrame parentFrame) { throw new InvalidOperationException(); }
        }

        public static IFrameFrame Root = new FrameRootFrame();
        #endregion

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

        /// <summary>
        /// Create cells for the provided state view.
        /// </summary>
        /// <param name="controllerView">The view in cells are created.</param>
        /// <param name="stateView">The state view for which to create cells.</param>
        public abstract IFrameCellView BuildCells(IFrameControllerView controllerView, IFrameNodeStateView stateView);
        #endregion

        #region Debugging
        public override string ToString()
        {
            return base.ToString() + " (" + GetHashCode() + ")";
        }
        #endregion
    }
}
