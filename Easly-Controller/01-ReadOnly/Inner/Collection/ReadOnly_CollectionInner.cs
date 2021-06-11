namespace EaslyController.ReadOnly
{
    using System.Diagnostics;

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public interface IReadOnlyCollectionInner : IReadOnlyInner
    {
        /// <summary>
        /// Count of all node states in the inner.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Checks if the inner must have at list one item.
        /// </summary>
        bool IsNeverEmpty { get; }
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index.</typeparam>
    internal interface IReadOnlyCollectionInner<out IIndex> : IReadOnlyInner<IIndex>
        where IIndex : IReadOnlyBrowsingCollectionNodeIndex
    {
        /// <summary>
        /// Count of all node states in the inner.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Checks if the inner must have at list one item.
        /// </summary>
        bool IsNeverEmpty { get; }
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    /// <typeparam name="IIndex">Type of the index as interface.</typeparam>
    /// <typeparam name="TIndex">Type of the index as class.</typeparam>
    internal abstract class ReadOnlyCollectionInner<IIndex, TIndex> : ReadOnlyInner<IIndex>, IReadOnlyCollectionInner<IIndex>, IReadOnlyCollectionInner
        where IIndex : IReadOnlyBrowsingCollectionNodeIndex
        where TIndex : ReadOnlyBrowsingCollectionNodeIndex, IIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyCollectionInner{IIndex, TIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        /// <param name="propertyName">Property name of the inner in <paramref name="owner"/>.</param>
        public ReadOnlyCollectionInner(IReadOnlyNodeState owner, string propertyName)
            : base(owner, propertyName)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Count of all node states in the inner.
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Checks if the inner must have at list one item.
        /// </summary>
        public abstract bool IsNeverEmpty { get; }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyCollectionInner{IIndex,TIndex}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyCollectionInner<IIndex, TIndex> AsCollectionInner))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsCollectionInner))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
