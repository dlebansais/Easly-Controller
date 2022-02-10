namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// State of a child node.
    /// </summary>
    public interface IReadOnlyEmptyNodeState : IReadOnlyNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IReadOnlyNodeIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of a child node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IReadOnlyEmptyNodeState<out IInner> : IReadOnlyNodeState<IInner>
        where IInner : IReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
    }

    /// <inheritdoc/>
    [DebuggerDisplay("Child node of {ParentInner?.InterfaceType.Name}")]
    internal class ReadOnlyEmptyNodeState<IInner> : ReadOnlyNodeState<IInner>, IReadOnlyEmptyNodeState<IInner>, IReadOnlyEmptyNodeState, IReadOnlyNodeState
        where IInner : IReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyEmptyNodeState{IInner}"/> class.
        /// </summary>
        public ReadOnlyEmptyNodeState()
            : base()
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
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out ReadOnlyEmptyNodeState<IInner> AsEmptyNodeState))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsEmptyNodeState))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsEmptyNodeState.Node))
                return comparer.Failed();

            if (!IsChildrenEqual(comparer, AsEmptyNodeState))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
