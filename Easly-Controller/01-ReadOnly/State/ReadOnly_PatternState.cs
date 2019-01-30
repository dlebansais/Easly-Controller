namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    public interface IReadOnlyPatternState : IReadOnlyPlaceholderNodeState
    {
        /// <summary>
        /// The replication pattern node.
        /// </summary>
        new IPattern Node { get; }

        /// <summary>
        /// The parent block state.
        /// </summary>
        IReadOnlyBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IReadOnlyBrowsingPatternIndex ParentIndex { get; }

        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        new IPattern CloneNode();
    }

    /// <summary>
    /// State of an replication pattern node.
    /// </summary>
    internal class ReadOnlyPatternState : ReadOnlyPlaceholderNodeState, IReadOnlyPatternState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPatternState"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public ReadOnlyPatternState(IReadOnlyBlockState parentBlockState, IReadOnlyBrowsingPatternIndex index)
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
        public new IPattern Node { get { return (IPattern)base.Node; } }

        /// <summary>
        /// The parent block state.
        /// </summary>
        public IReadOnlyBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IReadOnlyBrowsingPatternIndex ParentIndex { get { return (IReadOnlyBrowsingPatternIndex)base.ParentIndex; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        public new IPattern CloneNode() { return (IPattern)base.CloneNode(); }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyNodeState"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyPatternState AsPatternState))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPatternState))
                return comparer.Failed();

            if (Node != AsPatternState.Node)
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
