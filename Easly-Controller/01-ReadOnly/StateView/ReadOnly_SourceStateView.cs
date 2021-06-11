namespace EaslyController.ReadOnly
{
    using System.Diagnostics;

    /// <summary>
    /// View of a source state.
    /// </summary>
    public interface IReadOnlySourceStateView : IReadOnlyNodeStateView
    {
        /// <summary>
        /// The source state.
        /// </summary>
        new IReadOnlySourceState State { get; }
    }

    /// <summary>
    /// View of a source state.
    /// </summary>
    internal class ReadOnlySourceStateView : ReadOnlyNodeStateView, IReadOnlySourceStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySourceStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The source state.</param>
        public ReadOnlySourceStateView(IReadOnlyControllerView controllerView, IReadOnlySourceState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The source state.
        /// </summary>
        public new IReadOnlySourceState State { get { return (IReadOnlySourceState)base.State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlySourceStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlySourceStateView AsSourceStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsSourceStateView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
