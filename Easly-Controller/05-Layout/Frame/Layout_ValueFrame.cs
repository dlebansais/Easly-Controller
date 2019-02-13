namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Frame describing a value property (or string) in a node.
    /// </summary>
    public interface ILayoutValueFrame : IFocusValueFrame, ILayoutNamedFrame, ILayoutNodeFrameWithVisibility
    {
    }

    /// <summary>
    /// Frame describing a value property (or string) in a node.
    /// </summary>
    public abstract class LayoutValueFrame : FocusValueFrame, ILayoutValueFrame
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
        /// Node frame visibility. Null if always visible.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutNodeFrameVisibility Visibility { get { return (ILayoutNodeFrameVisibility)base.Visibility; } set { base.Visibility = value; } }
        #endregion
    }
}
