using System;
using System.Diagnostics;

namespace EaslyController.ReadOnly
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
        /// First node state that can be enumerated in the inner.
        /// </summary>
        IReadOnlyPlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public interface IReadOnlyCollectionInner<out IIndex> : IReadOnlyInner<IIndex>
        where IIndex : IReadOnlyBrowsingCollectionNodeIndex
    {
        /// <summary>
        /// Count of all node states in the inner.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// First node state that can be enumerated in the inner.
        /// </summary>
        IReadOnlyPlaceholderNodeState FirstNodeState { get; }
    }

    /// <summary>
    /// Base inner for a list or a block list.
    /// </summary>
    public abstract class ReadOnlyCollectionInner<IIndex, TIndex> : ReadOnlyInner<IIndex>, IReadOnlyCollectionInner<IIndex>, IReadOnlyCollectionInner
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
        /// First node state that can be enumerated in the inner.
        /// </summary>
        public virtual IReadOnlyPlaceholderNodeState FirstNodeState { get { throw new InvalidOperationException(); } } // Can't make this abstract, thank you C#...
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyInner"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyCollectionInner<IIndex> AsCollectionInner))
                return false;

            if (!base.IsEqual(comparer, AsCollectionInner))
                return false;

            return true;
        }
        #endregion
    }
}
