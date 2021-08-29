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
        new ReadOnlyBrowsingPatternIndex ParentIndex { get; }

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
    /// <typeparam name="TIndex">Parent inner of the state.</typeparam>
    internal class ReadOnlyPatternState<TIndex> : ReadOnlyPlaceholderNodeState<TIndex>, IReadOnlyPatternState, IReadOnlyNodeState
        where TIndex : ReadOnlyInner<IReadOnlyBrowsingChildIndex>
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPatternState{TIndex}"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public ReadOnlyPatternState(IReadOnlyBlockState parentBlockState, ReadOnlyBrowsingPatternIndex index)
            : base(index)
        {
            Debug.Assert(parentBlockState != null);

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
        public new ReadOnlyBrowsingPatternIndex ParentIndex { get { return (ReadOnlyBrowsingPatternIndex)base.ParentIndex; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        public new Pattern CloneNode() { return (Pattern)base.CloneNode(); }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyPatternState{TIndex}"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyPatternState<TIndex> AsPatternState))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPatternState))
                return comparer.Failed();

            if (!comparer.IsSameReference(Node, AsPatternState.Node))
                return comparer.Failed();

            if (!comparer.VerifyEqual(ParentBlockState, AsPatternState.ParentBlockState))
                return comparer.Failed();

            return true;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return "Pattern state";
        }
        #endregion
    }
}
