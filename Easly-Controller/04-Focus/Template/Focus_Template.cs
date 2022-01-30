namespace EaslyController.Focus
{
    using EaslyController.Frame;

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFocusTemplate : IFrameTemplate
    {
        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        new IFocusFrame Root { get; }
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public abstract class FocusTemplate : FrameTemplate, IFocusTemplate
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FocusTemplate"/> object.
        /// </summary>
        public static new IFocusTemplate Empty { get; } = new FocusNodeTemplate();
        #endregion

        #region Properties
        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        public new IFocusFrame Root { get { return (IFocusFrame)base.Root; } set { base.Root = value; } }

        private protected override bool IsRootValid { get { return Root.ParentFrame == FocusFrame.FocusRoot; } }
        #endregion
    }
}
