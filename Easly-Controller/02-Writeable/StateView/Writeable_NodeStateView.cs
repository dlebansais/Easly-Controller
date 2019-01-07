using EaslyController.ReadOnly;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface IWriteableNodeStateView : IReadOnlyNodeStateView
    {
        /// <summary>
        /// The controller view to which this object belongs.
        /// </summary>
        new IWriteableControllerView ControllerView { get; }

        /// <summary>
        /// The node state.
        /// </summary>
        new IWriteableNodeState State { get; }
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    public abstract class WriteableNodeStateView : ReadOnlyNodeStateView, IWriteableNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableNodeStateView"/> class.
        /// </summary>
        /// <param name="controllerView">The controller view to which this object belongs.</param>
        /// <param name="state">The node state.</param>
        public WriteableNodeStateView(IWriteableControllerView controllerView, IWriteableNodeState state)
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
        /// The node state.
        /// </summary>
        public new IWriteableNodeState State { get { return (IWriteableNodeState)base.State; } }
        #endregion

        #region Debugging
        /// <summary>
        /// Compares two <see cref="IWriteableNodeStateView"/> objects.
        /// </summary>
        /// <param name="comparer">The comparison support object.</param>
        /// <param name="other">The other object.</param>
        public override bool IsEqual(CompareEqual comparer, IEqualComparable other)
        {
            Debug.Assert(other != null);

            if (!(other is IWriteableNodeStateView AsNodeStateView))
                return false;

            if (!base.IsEqual(comparer, AsNodeStateView))
                return false;

            return true;
        }
        #endregion
    }
}
