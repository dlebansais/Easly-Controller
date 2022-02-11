namespace EaslyController.Writeable
{
    using Contracts;
    using EaslyController.ReadOnly;

    /// <summary>
    /// View of a child node.
    /// </summary>
    public class WriteablePlaceholderNodeStateView : ReadOnlyPlaceholderNodeStateView, IWriteableNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteablePlaceholderNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The child node state.</param>
        public WriteablePlaceholderNodeStateView(WriteableControllerView controllerView, IWriteablePlaceholderNodeState state)
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
        public new IWriteablePlaceholderNodeState State { get { return (IWriteablePlaceholderNodeState)base.State; } }
        IWriteableNodeState IWriteableNodeStateView.State { get { return State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="WriteablePlaceholderNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Contract.RequireNotNull(other, out IEqualComparable Other);

            if (!comparer.IsSameType(Other, out WriteablePlaceholderNodeStateView AsPlaceholderNodeStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPlaceholderNodeStateView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
