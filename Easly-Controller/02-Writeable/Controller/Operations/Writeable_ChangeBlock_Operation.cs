namespace EaslyController.Writeable
{
    using System;
    using BaseNode;
    using Contracts;

    /// <summary>
    /// Operation details for changing a block.
    /// </summary>
    public class WriteableChangeBlockOperation : WriteableOperation
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
            Contract.RequireNotNull(blockState, out IWriteableBlockState BlockState);

            this.BlockState = BlockState;
        }

        /// <summary>
        /// Creates an operation to undo the change replication operation.
        /// </summary>
        public virtual WriteableChangeBlockOperation ToInverseChange()
        {
            return CreateChangeBlockOperation(Replication == ReplicationStatus.Normal ? ReplicationStatus.Replicated : ReplicationStatus.Normal, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeBlockOperation object.
        /// </summary>
        private protected virtual WriteableChangeBlockOperation CreateChangeBlockOperation(ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableChangeBlockOperation));
            return new WriteableChangeBlockOperation(ParentNode, PropertyName, BlockIndex, replication, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
