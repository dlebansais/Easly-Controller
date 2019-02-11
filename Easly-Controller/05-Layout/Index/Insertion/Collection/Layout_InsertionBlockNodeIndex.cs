namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;

    /// <summary>
    /// Base for block list insertion index classes.
    /// </summary>
    public interface ILayoutInsertionBlockNodeIndex : IFocusInsertionBlockNodeIndex, ILayoutInsertionCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base for block list insertion index classes.
    /// </summary>
    public abstract class LayoutInsertionBlockNodeIndex : FocusInsertionBlockNodeIndex, ILayoutInsertionBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertionBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">The node in which the insertion operation is taking place.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="node">The inserted node.</param>
        public LayoutInsertionBlockNodeIndex(INode parentNode, string propertyName, INode node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion
    }
}
