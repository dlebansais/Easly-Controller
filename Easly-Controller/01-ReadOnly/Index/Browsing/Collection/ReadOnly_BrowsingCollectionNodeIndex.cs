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

    /// <summary>
    /// Base for list and block list index classes.
    /// </summary>
    internal abstract class ReadOnlyBrowsingCollectionNodeIndex : IReadOnlyBrowsingCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingCollectionNodeIndex"/> class.
        /// </summary>
        /// <param name="node">The indexed node.</param>
        /// <param name="propertyName">The property for the index.</param>
        public ReadOnlyBrowsingCollectionNodeIndex(INode node, string propertyName)
        {
            Debug.Assert(node != null);
            Debug.Assert(!string.IsNullOrEmpty(propertyName));

            Node = node;
            PropertyName = propertyName;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node indexed.
        /// </summary>
        public INode Node { get; }

        /// <summary>
        /// Property indexed for <see cref="Node"/>.
        /// </summary>
        public string PropertyName { get; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyIndex"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyBrowsingCollectionNodeIndex AsBlockNodeIndex))
                return comparer.Failed();

            if (Node != AsBlockNodeIndex.Node)
                return comparer.Failed();

            if (PropertyName != AsBlockNodeIndex.PropertyName)
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
