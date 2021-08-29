namespace EaslyController.Frame
{
    using System.Diagnostics;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public interface IFrameInsertionCollectionNodeIndex : IWriteableInsertionCollectionNodeIndex, IFrameInsertionChildNodeIndex, IFrameNodeIndex
    {
    }

    /// <summary>
    /// Base for list and block list insertion index classes.
    /// </summary>
    public abstract class FrameInsertionCollectionNodeIndex : WriteableInsertionCollectionNodeIndex, IFrameInsertionCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInsertionCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="parentNode">The node in which the insertion operation is taking place.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="node">The inserted node.</param>
        public FrameInsertionCollectionNodeIndex(Node parentNode, string propertyName, Node node)
            : base(parentNode, propertyName, node)
        {
        }
        #endregion
    }
}
