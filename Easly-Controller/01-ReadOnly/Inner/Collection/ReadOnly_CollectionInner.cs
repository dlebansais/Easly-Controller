﻿namespace EaslyController.ReadOnly
{
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

    /// <inheritdoc/>
    internal abstract class ReadOnlyCollectionInner<IIndex> : ReadOnlyInner<IIndex>, IReadOnlyCollectionInner<IIndex>, IReadOnlyCollectionInner, IReadOnlyInner
        where IIndex : IReadOnlyBrowsingCollectionNodeIndex
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyCollectionInner{IIndex}"/> class.
        /// </summary>
        protected ReadOnlyCollectionInner()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyCollectionInner{IIndex}"/> class.
        /// </summary>
        /// <param name="owner">Parent containing the inner.</param>
        protected ReadOnlyCollectionInner(IReadOnlyNodeState owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyCollectionInner{IIndex}"/> class.
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
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out ReadOnlyCollectionInner<IIndex> AsCollectionInner))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsCollectionInner))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
