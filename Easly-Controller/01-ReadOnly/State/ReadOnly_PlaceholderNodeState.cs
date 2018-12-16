using BaseNode;

namespace EaslyController.ReadOnly
{
    /// <summary>
    /// State of an child node.
    /// </summary>
    public interface IReadOnlyPlaceholderNodeState : IReadOnlyNodeState
    {
        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        new IReadOnlyNodeIndex ParentIndex { get; }
    }

    /// <summary>
    /// State of an child node.
    /// </summary>
    public class ReadOnlyPlaceholderNodeState : ReadOnlyNodeState, IReadOnlyPlaceholderNodeState
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyPlaceholderNodeState"/> class.
        /// </summary>
        /// <param name="parentIndex">The index used to create the state.</param>
        public ReadOnlyPlaceholderNodeState(IReadOnlyNodeIndex parentIndex)
            : base(parentIndex)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The child node.
        /// </summary>
        public override INode Node { get { return ParentIndex.Node; } }

        /// <summary>
        /// The index that was used to create the state.
        /// </summary>
        public new IReadOnlyNodeIndex ParentIndex { get { return (IReadOnlyNodeIndex)base.ParentIndex; } }
        #endregion
    }
}
