namespace EaslyController.Writeable
{
    using Contracts;
    using EaslyController.ReadOnly;

    /// <summary>
    /// View of a child node.
    /// </summary>
    public class WriteableEmptyNodeStateView : ReadOnlyEmptyNodeStateView, IWriteableNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableEmptyNodeStateView"/> class.
        /// </summary>
        public WriteableEmptyNodeStateView()
            : this(WriteableControllerView.Empty, WriteableNodeState<IWriteableInner<IWriteableBrowsingChildIndex>>.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableEmptyNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The node state.</param>
        protected WriteableEmptyNodeStateView(WriteableControllerView controllerView, IWriteableNodeState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new WriteableControllerView ControllerView { get { return (WriteableControllerView)base.ControllerView; } }

        /// <summary>
        /// The child node.
        /// </summary>
        public new IWriteableEmptyNodeState State { get { return (IWriteableEmptyNodeState)base.State; } }
        IWriteableNodeState IWriteableNodeStateView.State { get { return State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteableEmptyNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out WriteableEmptyNodeStateView AsEmptyNodeStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsEmptyNodeStateView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
