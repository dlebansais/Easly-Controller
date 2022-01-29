namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public interface IReadOnlyBrowsingBlockNodeIndex : IReadOnlyBrowsingCollectionNodeIndex
    {
        /// <summary>
        /// Position of the block in the block list.
        /// </summary>
        int BlockIndex { get; }
    }

    /// <inheritdoc/>
    public abstract class ReadOnlyBrowsingBlockNodeIndex : ReadOnlyBrowsingCollectionNodeIndex, IReadOnlyBrowsingBlockNodeIndex, IReadOnlyBrowsingCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingBlockNodeIndex"/> class.
        /// </summary>
        protected ReadOnlyBrowsingBlockNodeIndex()
        {
            BlockIndex = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        /// <param name="blockIndex">The position of the block in the block list.</param>
        public ReadOnlyBrowsingBlockNodeIndex(Node node, string propertyName, int blockIndex)
            : base(node, propertyName)
        {
            Debug.Assert(blockIndex >= 0);

            BlockIndex = blockIndex;
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public virtual int BlockIndex { get; private protected set; }
        #endregion
    }
}
