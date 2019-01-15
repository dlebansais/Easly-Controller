using EaslyController.Frame;

namespace EaslyController.Focus
{
    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFocusTemplate : IFrameTemplate
    {
        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        new IFocusFrame Root { get; set; }
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public abstract class FocusTemplate : FrameTemplate, IFocusTemplate
    {
        #region Properties
        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        public new IFocusFrame Root { get { return (IFocusFrame)base.Root; } set { base.Root = value; } }

        protected override bool IsRootValid { get { return (Root.ParentFrame == FocusFrame.FocusRoot); } }
        #endregion
    }
}
