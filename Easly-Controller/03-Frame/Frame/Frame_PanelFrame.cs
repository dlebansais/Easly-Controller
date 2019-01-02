using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public interface IFramePanelFrame : IFrameFrame
    {
        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        IFrameFrameList Items { get; }
    }

    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public abstract class FramePanelFrame : FrameFrame, IFramePanelFrame
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FramePanelFrame"/>.
        /// </summary>
        /// <param name="state">The state that will be browsed.</param>
        public FramePanelFrame()
        {
            Items = CreateItems();
        }
        #endregion

        #region Properties
        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        public IFrameFrameList Items { get; set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Checks that a frame is correctly constructed.
        /// </summary>
        /// <param name="nodeType">Type of the node this frame can describe.</param>
        public override bool IsValid(Type nodeType)
        {
            if (!base.IsValid(nodeType))
                return false;

            if (Items == null || Items.Count == 0)
                return false;

            foreach (IFrameFrame Item in Items)
                if (!Item.IsValid(nodeType))
                    return false;

            return true;
        }

        /// <summary>
        /// Update the reference to the parent frame.
        /// </summary>
        /// <param name="parentFrame">The parent frame.</param>
        public override void UpdateParentFrame(IFrameFrame parentFrame)
        {
            base.UpdateParentFrame(parentFrame);

            foreach (IFrameFrame Item in Items)
                Item.UpdateParentFrame(this);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxFrameList object.
        /// </summary>
        protected virtual IFrameFrameList CreateItems()
        {
            ControllerTools.AssertNoOverride(this, typeof(FramePanelFrame));
            return new FrameFrameList();
        }
        #endregion
    }
}
