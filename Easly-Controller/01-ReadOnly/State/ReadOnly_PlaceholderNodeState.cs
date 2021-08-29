namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// State of a child node.
    /// </summary>
    public interface IReadOnlyPlaceholderNodeState : IReadOnlyNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IReadOnlyNodeIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of a child node.
    /// </summary>
    /// <typeparam name="TIndex">Parent inner of the state.</typeparam>
    internal class ReadOnlyPlaceholderNodeState<TIndex> : ReadOnlyNodeState<TIndex>, IReadOnlyPlaceholderNodeState, IReadOnlyNodeState
        where TIndex : ReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPlaceholderNodeState{TIndex}"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public ReadOnlyPlaceholderNodeState(IReadOnlyNodeIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The child node.
        /// </summary>
        public override Node Node { get { return ParentIndex.Node; } }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IReadOnlyNodeIndex ParentIndex { get { return (IReadOnlyNodeIndex)base.ParentIndex; } }

        /// <summary>
        /// The comment associated to this state. Null if none.
        /// </summary>
        public override string Comment { get { return Node.Documentation.Comment; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyPlaceholderNodeState{TIndex}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyPlaceholderNodeState<TIndex> AsPlaceholderNodeState))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPlaceholderNodeState))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsPlaceholderNodeState.Node))
                return comparer.Failed();

            if (!IsChildrenEqual(comparer, AsPlaceholderNodeState))
                return comparer.Failed();

            return true;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            if (ParentInner == null)
                return "Root node";
            else
                return $"Child node of {ParentInner.InterfaceType.Name}";
        }
        #endregion
    }
}
