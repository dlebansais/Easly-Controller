namespace EaslyController.Layout
{
    using System.Diagnostics;
    using EaslyController.Focus;
    using EaslyController.Frame;

    /// <summary>
    /// Base frame for a placeholder node in a block list.
    /// </summary>
    public interface ILayoutCollectionPlaceholderFrame : IFocusCollectionPlaceholderFrame, ILayoutFrame, ILayoutBlockFrame, ILayoutFrameWithSelector
    {
    }

    /// <summary>
    /// Base frame for a placeholder node in a block list.
    /// </summary>
    public abstract class LayoutCollectionPlaceholderFrame : FocusCollectionPlaceholderFrame, ILayoutCollectionPlaceholderFrame
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
        /// List of optional selectors.
        /// (Set in Xaml)
        /// </summary>
        public new LayoutFrameSelectorList Selectors { get { return (LayoutFrameSelectorList)base.Selectors; } }
        #endregion
    }
}
