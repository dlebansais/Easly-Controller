﻿namespace EaslyController.ReadOnly
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
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IReadOnlyPlaceholderNodeState<out IInner> : IReadOnlyNodeState<IInner>
        where IInner : IReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
    }

    /// <inheritdoc/>
    [DebuggerDisplay("Child node of {ParentInner?.InterfaceType.Name}")]
    internal class ReadOnlyPlaceholderNodeState<IInner> : ReadOnlyNodeState<IInner>, IReadOnlyPlaceholderNodeState<IInner>, IReadOnlyPlaceholderNodeState, IReadOnlyNodeState
        where IInner : IReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="ReadOnlyPlaceholderNodeState{IInner}"/> object.
        /// </summary>
        public static new ReadOnlyPlaceholderNodeState<IInner> Empty { get; } = new ReadOnlyPlaceholderNodeState<IInner>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPlaceholderNodeState{IInner}"/> class.
        /// </summary>
        protected ReadOnlyPlaceholderNodeState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPlaceholderNodeState{IInner}"/> class.
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
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out ReadOnlyPlaceholderNodeState<IInner> AsPlaceholderNodeState))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPlaceholderNodeState))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsPlaceholderNodeState.Node))
                return comparer.Failed();

            if (!IsChildrenEqual(comparer, AsPlaceholderNodeState))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
