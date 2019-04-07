namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Constants;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public interface ILayoutPanelFrame : IFocusPanelFrame, ILayoutFrame, ILayoutNodeFrameWithVisibility, ILayoutBlockFrameWithVisibility, ILayoutSelectorPropertyFrame
    {
        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        new ILayoutFrameList Items { get; }

        /// <summary>
        /// Indicates that block geometry must be drawn around a block.
        /// (Set in Xaml)
        /// </summary>
        bool HasBlockGeometry { get; }
    }

    /// <summary>
    /// Base frame for displaying more frames.
    /// </summary>
    public abstract class LayoutPanelFrame : FocusPanelFrame, ILayoutPanelFrame
    {
        #region Properties
        /// <summary>
        /// Parent template.
        /// </summary>
        public new ILayoutTemplate ParentTemplate { get { return (ILayoutTemplate)base.ParentTemplate; } }

        /// <summary>
        /// Parent frame, or null for the root frame in a template.
        /// </summary>
        public new ILayoutFrame ParentFrame { get { return (ILayoutFrame)base.ParentFrame; } }

        /// <summary>
        /// List of frames within this frame.
        /// </summary>
        public new ILayoutFrameList Items { get { return (ILayoutFrameList)base.Items; } }

        /// <summary>
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutNodeFrameVisibility Visibility { get { return (ILayoutNodeFrameVisibility)base.Visibility; } set { base.Visibility = value; } }

        /// <summary>
        /// Block frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutBlockFrameVisibility BlockVisibility { get { return (ILayoutBlockFrameVisibility)base.BlockVisibility; } set { base.BlockVisibility = value; } }

        /// <summary>
        /// Indicates that block geometry must be drawn around a block.
        /// (Set in Xaml)
        /// </summary>
        public bool HasBlockGeometry { get; set; }
        #endregion
    }
}
