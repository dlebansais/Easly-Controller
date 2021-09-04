namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    public interface IReadOnlyBrowsingCollectionNodeIndex : IReadOnlyBrowsingChildIndex, IReadOnlyNodeIndex
    {
    }

    /// <inheritdoc/>
    public abstract class ReadOnlyBrowsingCollectionNodeIndex : IReadOnlyBrowsingCollectionNodeIndex, IReadOnlyBrowsingChildIndex, IReadOnlyNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        public ReadOnlyBrowsingCollectionNodeIndex(Node node, string propertyName)
        {
            Debug.Assert(node != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            Node = node;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <inheritdoc/>
        public Node Node { get; }

        /// <inheritdoc/>
        public string PropertyName { get; }
        #endregion
    }
}
