namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Operation details for changing a block.
    /// </summary>
    public interface IWriteableChangeBlockOperation : IWriteableOperation
    {
        /// <summary>
        /// Node where the block change is taking place.
        /// </summary>
        Node ParentNode { get; }

        /// <summary>
        /// Block list property of <see cref="ParentNode"/> for which a block is changed.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Index of the changed block.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// New replication value.
        /// </summary>
        ReplicationStatus Replication { get; }

        /// <summary>
        /// Block state changed.
        /// </summary>
        IWriteableBlockState BlockState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state changed.</param>
        void Update(IWriteableBlockState blockState);

        /// <summary>
        /// Creates an operation to undo the change replication operation.
        /// </summary>
        IWriteableChangeBlockOperation ToInverseChange();
    }

    /// <summary>
    /// Operation details for changing a block.
    /// </summary>
    internal class WriteableChangeBlockOperation : WriteableOperation, IWriteableChangeBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableChangeBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block change is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> for which a block is changed.</param>
        /// <param name="blockIndex">Index of the changed block.</param>
        /// <param name="replication">New replication value.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableChangeBlockOperation(Node parentNode, string propertyName, int blockIndex, ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            BlockIndex = blockIndex;
            Replication = replication;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the block change is taking place.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Block list property of <see cref="ParentNode"/> for which a block is changed.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Index of the changed block.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// New replication value.
        /// </summary>
        public ReplicationStatus Replication { get; }

        /// <summary>
        /// Block state changed.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state changed.</param>
        public virtual void Update(IWriteableBlockState blockState)
        {
            Debug.Assert(blockState != null);

            BlockState = blockState;
        }

        /// <summary>
        /// Creates an operation to undo the change replication operation.
        /// </summary>
        public virtual IWriteableChangeBlockOperation ToInverseChange()
        {
            return CreateChangeBlockOperation(Replication == ReplicationStatus.Normal ? ReplicationStatus.Replicated : ReplicationStatus.Normal, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeBlockOperation object.
        /// </summary>
        private protected virtual IWriteableChangeBlockOperation CreateChangeBlockOperation(ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableChangeBlockOperation));
            return new WriteableChangeBlockOperation(ParentNode, PropertyName, BlockIndex, replication, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
