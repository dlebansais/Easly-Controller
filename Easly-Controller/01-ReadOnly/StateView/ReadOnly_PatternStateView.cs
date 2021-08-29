namespace EaslyController.ReadOnly
{
    using System.Diagnostics;

    /// <summary>
    /// View of a pattern state.
    /// </summary>
    internal class ReadOnlyPatternStateView : ReadOnlyNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPatternStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The pattern state.</param>
        public ReadOnlyPatternStateView(ReadOnlyControllerView controllerView, IReadOnlyPatternState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The pattern state.
        /// </summary>
        public new IReadOnlyPatternState State { get { return (IReadOnlyPatternState)base.State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="ReadOnlyPatternStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!comparer.IsSameType(other, out ReadOnlyPatternStateView AsPatternStateView))
                return comparer.Failed();

            if (!base.IsEqual(comparer, AsPatternStateView))
                return comparer.Failed();

            return true;
        }
        #endregion
    }
}
