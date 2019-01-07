using System.Diagnostics;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public interface IReadOnlyOptionalNodeStateView : IReadOnlyNodeStateView
    {
        /// <summary>
        /// The optional node state.
        /// </summary>
        new IReadOnlyOptionalNodeState State { get; }
    }

    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public class ReadOnlyOptionalNodeStateView : ReadOnlyNodeStateView, IReadOnlyOptionalNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyOptionalNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The optional node state.</param>
        public ReadOnlyOptionalNodeStateView(IReadOnlyControllerView controllerView, IReadOnlyOptionalNodeState state)
            : base(controllerView, state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The optional node state.
        /// </summary>
        public new IReadOnlyOptionalNodeState State { get { return (IReadOnlyOptionalNodeState)base.State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IReadOnlyOptionalNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IReadOnlyOptionalNodeStateView AsOptionalNodeStateView))
                return false;

            if (!base.IsEqual(comparer, AsOptionalNodeStateView))
                return false;

            return true;
        }
        #endregion
    }
}
