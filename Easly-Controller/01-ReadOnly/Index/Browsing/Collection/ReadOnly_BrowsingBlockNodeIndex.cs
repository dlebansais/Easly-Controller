using BaseNode;
using System.Diagnostics;

namespace EaslyController.ReadOnly
{
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

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public abstract class ReadOnlyBrowsingBlockNodeIndex : ReadOnlyBrowsingCollectionNodeIndex, IReadOnlyBrowsingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        public ReadOnlyBrowsingBlockNodeIndex(INode node, string propertyName, int blockIndex)
            : base(node, propertyName)
        {
            Debug.Assert(blockIndex >= 0);

            BlockIndex = blockIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Position of the block in the block list.
        /// </summary>
        public virtual int BlockIndex { get; }
        #endregion
    }
}
