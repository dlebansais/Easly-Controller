namespace EaslyController.Focus
{
    using BaseNode;
    using EaslyController.Frame;

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public interface IFocusBrowsingBlockNodeIndex : IFrameBrowsingBlockNodeIndex, IFocusBrowsingCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    internal abstract class FocusBrowsingBlockNodeIndex : FrameBrowsingBlockNodeIndex, IFocusBrowsingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusBrowsingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="blockIndex">Position of the block in the block list.</param>
        public FocusBrowsingBlockNodeIndex(INode node, string propertyName, int blockIndex)
            : base(node, propertyName, blockIndex)
        {
        }
        #endregion
    }
}
