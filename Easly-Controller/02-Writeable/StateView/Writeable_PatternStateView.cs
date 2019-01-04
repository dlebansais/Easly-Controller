using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public interface IWriteablePatternStateView : IReadOnlyPatternStateView, IWriteablePlaceholderNodeStateView
    {
        /// <summary>
        /// The pattern state.
        /// </summary>
        new IWriteablePatternState State { get; }
    }

    /// <summary>
    /// View of a pattern state.
    /// </summary>
    public class WriteablePatternStateView : ReadOnlyPatternStateView, IWriteablePatternStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteablePatternStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The pattern state.</param>
        public WriteablePatternStateView(IWriteableControllerView controllerView, IWriteablePatternState state)
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
        public new IWriteablePatternState State { get { return (IWriteablePatternState)base.State; } }
        IWriteableNodeState IWriteableNodeStateView.State { get { return State; } }
        IWriteablePlaceholderNodeState IWriteablePlaceholderNodeStateView.State { get { return State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IWriteablePatternStateView"/> objects.
        /// </summary>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteablePatternStateView AsPatternStateView))
                return false;

            if (!base.IsEqual(comparer, AsPatternStateView))
                return false;

            return true;
        }
        #endregion
    }
}
