namespace EaslyController.ReadOnly
{
    using Contracts;

    /// <summary>
    /// Index for a node in a list of nodes.
    /// </summary>
    public interface IReadOnlyBrowsingDummyCollectionNodeIndex : IReadOnlyBrowsingCollectionNodeIndex, IEqualComparable
    {
    }

    /// <inheritdoc/>
    public class ReadOnlyBrowsingDummyCollectionNodeIndex : ReadOnlyBrowsingCollectionNodeIndex, IReadOnlyBrowsingDummyCollectionNodeIndex, IReadOnlyBrowsingCollectionNodeIndex, IEqualComparable
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyBrowsingDummyCollectionNodeIndex"/> class.
        /// </summary>
        public ReadOnlyBrowsingDummyCollectionNodeIndex()
        {
        }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public virtual bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out ReadOnlyBrowsingDummyCollectionNodeIndex AsListNodeIndex))
                return comparer.Failed();

            if (!comparer.IsSameString(PropertyName, AsListNodeIndex.PropertyName))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsListNodeIndex.Node))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
