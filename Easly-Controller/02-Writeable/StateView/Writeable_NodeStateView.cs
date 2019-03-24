namespace EaslyController.Writeable
{
    using System.Diagnostics;
    using EaslyController.ReadOnly;

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
    internal abstract class WriteableNodeStateView : ReadOnlyNodeStateView, IWriteableNodeStateView
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
    }
}
