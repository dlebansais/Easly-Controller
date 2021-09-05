namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Base frame for a list of nodes.
    /// </summary>
    public interface ILayoutListFrame : IFocusListFrame, ILayoutNamedFrame, ILayoutNodeFrameWithVisibility, ILayoutFrameWithSelector, ILayoutSelectorPropertyFrame
    {
    }

    /// <summary>
    /// Base frame for a list of nodes.
    /// </summary>
    public abstract class LayoutListFrame : FocusListFrame, ILayoutListFrame
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

        /// <summary>
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        public new LayoutFrameSelectorList Selectors { get { return (LayoutFrameSelectorList)base.Selectors; } }
        #endregion
    }
}
