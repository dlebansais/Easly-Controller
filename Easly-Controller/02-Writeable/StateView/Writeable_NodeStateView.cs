using EaslyController.ReadOnly;

namespace EaslyController.Writeable
{
    /// <summary>
    /// View of a node state.
    /// </summary>
    public interface IWriteableNodeStateView : IReadOnlyNodeStateView
    {
        /// <summary>
        /// The node state.
        /// </summary>
        new IWriteableNodeState State { get; }
    }

    /// <summary>
    /// View of a node state.
    /// </summary>
    public class WriteableNodeStateView : ReadOnlyNodeStateView, IWriteableNodeStateView
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableNodeStateView"/> class.
        /// </summary>
        /// <param name="state">The node state.</param>
        public WriteableNodeStateView(IWriteableNodeState state)
            : base(state)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The node state.
        /// </summary>
        public new IWriteableNodeState State { get { return (IWriteableNodeState)base.State; } }
        #endregion

        #region Debugging
        public override bool IsEqual(IReadOnlyNodeStateView other)
        {
            if (!(other is IWriteableNodeStateView AsWriteable))
                return false;

            if (!base.IsEqual(AsWriteable))
                return false;

            return true;
        }
        #endregion
    }
}
