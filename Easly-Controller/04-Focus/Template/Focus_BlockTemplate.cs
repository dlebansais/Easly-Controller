namespace EaslyController.Focus
{
    using System.Windows.Markup;
    using EaslyController.Frame;

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface IFocusBlockTemplate : IFrameBlockTemplate, IFocusTemplate
    {
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    [ContentProperty("Root")]
    public class FocusBlockTemplate : FrameBlockTemplate, IFocusBlockTemplate
    {
        #region Properties
        /// <summary>
        /// Root frame.
        /// (Set in Xaml)
        /// </summary>
        public new IFocusFrame Root { get { return (IFocusFrame)base.Root; } set { base.Root = value; } }

        /// <summary></summary>
        private protected override bool IsRootValid { get { return Root.ParentFrame == FocusFrame.FocusRoot; } }
        #endregion
    }
}
