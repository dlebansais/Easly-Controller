namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;

    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public interface ILayoutBrowsingCollectionNodeIndex : IFocusBrowsingCollectionNodeIndex, ILayoutBrowsingChildIndex, ILayoutNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    internal abstract class LayoutBrowsingCollectionNodeIndex : FocusBrowsingCollectionNodeIndex, ILayoutBrowsingCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBrowsingCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        public LayoutBrowsingCollectionNodeIndex(Node node, string propertyName)
            : base(node, propertyName)
        {
        }
        #endregion
    }
}
