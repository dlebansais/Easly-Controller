namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public interface ILayoutBrowsingBlockNodeIndex : IFocusBrowsingBlockNodeIndex, ILayoutBrowsingCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    internal abstract class LayoutBrowsingBlockNodeIndex : FocusBrowsingBlockNodeIndex, ILayoutBrowsingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutBrowsingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        public LayoutBrowsingBlockNodeIndex(INode node, string propertyName, int blockIndex)
            : base(node, propertyName, blockIndex)
        {
        }
        #endregion
    }
}
