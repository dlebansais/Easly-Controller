namespace EaslyController.Layout
{
    using EaslyController.Focus;

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface ILayoutTemplate : IFocusTemplate
    {
        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        new ILayoutFrame Root { get; }
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public abstract class LayoutTemplate : FocusTemplate, ILayoutTemplate
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutTemplate"/> object.
        /// </summary>
        public static new ILayoutTemplate Empty { get; } = new LayoutNodeTemplate();
        #endregion

        #region Properties
        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        public new ILayoutFrame Root { get { return (ILayoutFrame)base.Root; } set { base.Root = value; } }

        private protected override bool IsRootValid { get { return Root.ParentFrame == LayoutFrame.LayoutRoot; } }
        #endregion
    }
}
