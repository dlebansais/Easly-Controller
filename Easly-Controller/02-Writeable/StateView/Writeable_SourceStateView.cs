namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using EaslyController.ReadOnly;

    /// <summary>
    /// View of a source state.
    /// </summary>
    public interface IWriteableSourceStateView : IReadOnlySourceStateView, IWriteableNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IWriteableSourceState State { get; }
    }

    /// <summary>
    /// View of a source state.
    /// </summary>
    internal class WriteableSourceStateView : ReadOnlySourceStateView, IWriteableSourceStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableSourceStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The source state.</param>
        public WriteableSourceStateView(IWriteableControllerView controllerView, IWriteableSourceState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        public new IWriteableControllerView ControllerView { get { return (IWriteableControllerView)base.ControllerView; } }

        /// <summary>
        /// The pattern state.
        /// </summary>
        public new IWriteableSourceState State { get { return (IWriteableSourceState)base.State; } }
        IWriteableNodeState IWriteableNodeStateView.State { get { return State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IWriteableSourceStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out WriteableSourceStateView AsSourceStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsSourceStateView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
