using BaseNode;
using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public interface IWriteableBrowsingBlockNodeIndex : IReadOnlyBrowsingBlockNodeIndex, IWriteableBrowsingCollectionNodeIndex
    {
    }

    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public abstract class WriteableBrowsingBlockNodeIndex : ReadOnlyBrowsingBlockNodeIndex, IWriteableBrowsingBlockNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableBrowsingBlockNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        public WriteableBrowsingBlockNodeIndex(INode node, string propertyName)
            : base(node, propertyName)
        {
        }
        #endregion
    }
}
