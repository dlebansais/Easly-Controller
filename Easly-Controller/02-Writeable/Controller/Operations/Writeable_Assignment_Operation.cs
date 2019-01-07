using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    public interface IWriteableAssignmentOperation : IWriteableOperation
    {
        /// <summary>
        /// Inner where the assignment is taking place.
        /// </summary>
        IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner { get; }

        /// <summary>
        /// Position of the assigned or unassigned node.
        /// </summary>
        IWriteableBrowsingOptionalNodeIndex NodeIndex { get; }

        /// <summary>
        /// The modified state.
        /// </summary>
        IWriteableOptionalNodeState State { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="state">The modified state.</param>
        void Update(IWriteableOptionalNodeState state);
    }

    /// <summary>
    /// Operation details for assigning or unassigning a node.
    /// </summary>
    public class WriteableAssignmentOperation : WriteableOperation, IWriteableAssignmentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableAssignmentOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the assignment is taking place.</param>
        /// <param name="nodeIndex">Position of the assigned or unassigned node.</param>
        public WriteableAssignmentOperation(IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> inner, IWriteableBrowsingOptionalNodeIndex nodeIndex)
            : base()
        {
            Inner = inner;
            NodeIndex = nodeIndex;
        }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="state">The new state.</param>
        public virtual void Update(IWriteableOptionalNodeState state)
        {
            Debug.Assert(state != null);

            State = state;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the assignment is taking place.
        /// </summary>
        public IWriteableOptionalInner<IWriteableBrowsingOptionalNodeIndex> Inner { get; }

        /// <summary>
        /// Position of the assigned or unassigned node.
        /// </summary>
        public IWriteableBrowsingOptionalNodeIndex NodeIndex { get; }

        /// <summary>
        /// The modified state.
        /// </summary>
        public IWriteableOptionalNodeState State { get; private set; }
        #endregion
    }
}
