﻿namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    public interface IReadOnlyPatternState : IReadOnlyNodeState
    {
        /// <summary>
        /// The replication pattern node.
        /// </summary>
        new Pattern Node { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IReadOnlyBrowsingPatternIndex ParentIndex { get; }

        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        new Pattern CloneNode();

        /// <summary>
        /// The parent block state.
        /// </summary>
        IReadOnlyBlockState ParentBlockState { get; }
    }

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    /// <typeparam name="IInner">Parent inner of the state.</typeparam>
    internal interface IReadOnlyPatternState<out IInner> : IReadOnlyPlaceholderNodeState<IInner>
        where IInner : IReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
    }

    /// <inheritdoc/>
    [DebuggerDisplay("Pattern state")]
    internal class ReadOnlyPatternState<IInner> : ReadOnlyPlaceholderNodeState<IInner>, IReadOnlyPatternState<IInner>, IReadOnlyPatternState, IReadOnlyNodeState
        where IInner : IReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPatternState{IInner}"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public ReadOnlyPatternState(IReadOnlyBlockState parentBlockState, IReadOnlyBrowsingPatternIndex index)
            : base(index)
        {
            ParentBlockState = parentBlockState;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The replication pattern node.
        /// </summary>
        public new Pattern Node { get { return (Pattern)base.Node; } }

        /// <summary>
        /// The parent block state.
        /// </summary>
        public IReadOnlyBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IReadOnlyBrowsingPatternIndex ParentIndex { get { return (ReadOnlyBrowsingPatternIndex)base.ParentIndex; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        public new Pattern CloneNode() { return (Pattern)base.CloneNode(); }
        #endregion

        #region Debugging
        /// <inheritdoc/>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            if (!comparer.IsSameType(other, out ReadOnlyPatternState<IInner> AsPatternState))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPatternState))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsPatternState.Node))
                return comparer.Failed();

            if (!comparer.VerifyEqual(ParentBlockState, AsPatternState.ParentBlockState))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
