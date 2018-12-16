using BaseNode;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// Base for block list index classes.
    /// </summary>
    public interface IReadOnlyBrowsingBlockNodeIndex : IReadOnlyBrowsingCollectionNodeIndex
    {
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
        public ReadOnlyBrowsingBlockNodeIndex(INode node, string propertyName)
            : base(node, propertyName)
        {
        }
        #endregion
    }
}
