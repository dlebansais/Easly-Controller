namespace EaslyController.Layout
{
    using System.Windows.Markup;
    using EaslyController.Focus;

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    public interface ILayoutBlockTemplate : IFocusBlockTemplate, ILayoutTemplate
    {
    }

    /// <summary>
    /// Template describing all components of a node.
    /// </summary>
    [ContentProperty("Root")]
    public class LayoutBlockTemplate : FocusBlockTemplate, ILayoutBlockTemplate
    {
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
