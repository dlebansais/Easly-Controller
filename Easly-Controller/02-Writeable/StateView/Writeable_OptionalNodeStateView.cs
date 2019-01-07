using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public interface IWriteableOptionalNodeStateView : IReadOnlyOptionalNodeStateView, IWriteableNodeStateView
    {
        /// <summary>
        /// The optional node state.
        /// </summary>
        new IWriteableOptionalNodeState State { get; }
    }

    /// <summary>
    /// View of an optional node state.
    /// </summary>
    public class WriteableOptionalNodeStateView : ReadOnlyOptionalNodeStateView, IWriteableOptionalNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOptionalNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The optional node state.</param>
        public WriteableOptionalNodeStateView(IWriteableControllerView controllerView, IWriteableOptionalNodeState state)
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
        /// The optional node state.
        /// </summary>
        public new IWriteableOptionalNodeState State { get { return (IWriteableOptionalNodeState)base.State; } }
        IWriteableNodeState IWriteableNodeStateView.State { get { return State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IWriteableOptionalNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableOptionalNodeStateView AsOptionalNodeStateView))
                return false;

            if (!base.IsEqual(comparer, AsOptionalNodeStateView))
                return false;

            return true;
        }
        #endregion
    }
}
