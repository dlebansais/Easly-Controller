namespace EaslyController.ReadOnly
{
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    public interface IReadOnlySourceState : IReadOnlyPlaceholderNodeState
    {
        /// <summary>
        /// The source identifier  node.
        /// </summary>
        new IIdentifier Node { get; }

        /// <summary>
        /// The parent block state.
        /// </summary>
        IReadOnlyBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IReadOnlyBrowsingSourceIndex ParentIndex { get; }

        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        new IIdentifier CloneNode();
    }

    /// <summary>
    /// State of a source identifier node.
    /// </summary>
    internal class ReadOnlySourceState : ReadOnlyPlaceholderNodeState, IReadOnlySourceState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySourceState"/> class.
        /// </summary>
        /// <param name="parentBlockState">The parent block state.</param>
        /// <param name="index">The index used to create the state.</param>
        public ReadOnlySourceState(IReadOnlyBlockState parentBlockState, IReadOnlyBrowsingSourceIndex index)
            : base(index)
        {
            Debug.Assert(parentBlockState != null);

            ParentBlockState = parentBlockState;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The source identifier  node.
        /// </summary>
        public new IIdentifier Node { get { return (IIdentifier)base.Node; } }

        /// <summary>
        /// The parent block state.
        /// </summary>
        public IReadOnlyBlockState ParentBlockState { get; }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IReadOnlyBrowsingSourceIndex ParentIndex { get { return (IReadOnlyBrowsingSourceIndex)base.ParentIndex; } }
        #endregion

        #region Client Interface
        /// <summary>
        /// Returns a clone of the node of this state.
        /// </summary>
        /// <returns>The cloned node.</returns>
        public new IIdentifier CloneNode() { return (IIdentifier)base.CloneNode(); }
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

            if (!comparer.IsSameType(other, out ReadOnlySourceState AsSourceState))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsSourceState))
                return comparer.Failed();

            if (Node != AsSourceState.Node)
                return comparer.Failed();

            if (!comparer.VerifyEqual(ParentBlockState, AsSourceState.ParentBlockState))
                return comparer.Failed();

            return true;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return "Source state";
        }
        #endregion
    }
}
