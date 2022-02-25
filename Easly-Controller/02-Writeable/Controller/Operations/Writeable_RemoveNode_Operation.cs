namespace EaslyController.Writeable
{
    using BaseNode;
    using Contracts;
    using NotNullReflection;

    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public class WriteableRemoveNodeOperation : WriteableRemoveOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableRemoveNodeOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the removal is taking place.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where a node is removed.</param>
        /// <param name="blockIndex">Block position where the node is removed, if applicable.</param>
        /// <param name="index">Position of the removed node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableRemoveNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            BlockIndex = blockIndex;
            Index = index;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the removal is taking place.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Property of <see cref="ParentNode"/> where a node is removed.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Block position where the node is removed, if applicable.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// Position of the removed node.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The removed state.
        /// </summary>
        public IWriteablePlaceholderNodeState RemovedState { get; private set; }

        /// <summary>
        /// The removed node.
        /// </summary>
        public Node RemovedNode { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="childState">State removed.</param>
        public virtual void Update(IWriteablePlaceholderNodeState childState)
        {
            Contract.RequireNotNull(childState, out IWriteablePlaceholderNodeState ChildState);

            RemovedState = ChildState;
            RemovedNode = ChildState.Node;
        }

        /// <summary>
        /// Creates an operation to undo the remove operation.
        /// </summary>
        public virtual WriteableInsertNodeOperation ToInsertNodeOperation()
        {
            return CreateInsertNodeOperation(BlockIndex, Index, RemovedNode, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        private protected virtual WriteableInsertNodeOperation CreateInsertNodeOperation(int blockIndex, int index, Node node, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableRemoveNodeOperation>());
            return new WriteableInsertNodeOperation(ParentNode, PropertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
