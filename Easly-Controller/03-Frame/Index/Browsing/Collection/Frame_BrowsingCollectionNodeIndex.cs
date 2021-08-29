namespace EaslyController.Frame
{
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public interface IFrameBrowsingCollectionNodeIndex : IWriteableBrowsingCollectionNodeIndex, IFrameBrowsingChildIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    internal abstract class FrameBrowsingCollectionNodeIndex : WriteableBrowsingCollectionNodeIndex, IFrameBrowsingCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameBrowsingCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        public FrameBrowsingCollectionNodeIndex(Node node, string propertyName)
            : base(node, propertyName)
        {
        }
        #endregion
    }
}
