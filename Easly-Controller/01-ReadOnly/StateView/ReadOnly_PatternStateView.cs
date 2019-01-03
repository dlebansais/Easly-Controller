using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public interface IReadOnlyPatternStateView : IReadOnlyPlaceholderNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IReadOnlyPatternState State { get; }
    }

    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public class ReadOnlyPatternStateView : ReadOnlyPlaceholderNodeStateView, IReadOnlyPatternStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPatternStateView"/> class.
        /// </summary>
        /// <param name="state">The pattern state.</param>
        public ReadOnlyPatternStateView(IReadOnlyPatternState state)
            : base(state)
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
        /// Compares two <see cref="IReadOnlyPatternStateView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyPatternStateView AsPatternStateView))
                return false;

            if (!base.IsEqual(comparer, AsPatternStateView))
                return false;

            return true;
        }
        #endregion
    }
}
