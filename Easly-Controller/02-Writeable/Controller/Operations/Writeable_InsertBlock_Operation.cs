﻿namespace EaslyController.Writeable
{
    using BaseNode;
    using Contracts;
    using NotNullReflection;

    /// <summary>
    /// Operation details for inserting a block with a single node in a block list.
    /// </summary>
    public interface IWriteableInsertBlockOperation : IWriteableInsertOperation, IWriteableOperation
    {
        /// <summary>
        /// Node where the block insertion is taking place.
        /// </summary>
        Node ParentNode { get; }

        /// <summary>
        /// Block list property of <see cref="ParentNode"/> where a block is inserted.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Index of the inserted block.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// The inserted block.
        /// </summary>
        IBlock Block { get; }

        /// <summary>
        /// The inserted node.
        /// </summary>
        Node Node { get; }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        IWriteableBrowsingExistingBlockNodeIndex BrowsingIndex { get; }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        IWriteableBlockState BlockState { get; }

        /// <summary>
        /// State inserted.
        /// </summary>
        IWriteablePlaceholderNodeState ChildState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="browsingIndex">Index of the state after it's inserted.</param>
        /// <param name="blockState">Block state inserted.</param>
        /// <param name="childState">State inserted.</param>
        void Update(IWriteableBrowsingExistingBlockNodeIndex browsingIndex, IWriteableBlockState blockState, IWriteablePlaceholderNodeState childState);

        /// <summary>
        /// Creates an operation to undo the insert block operation.
        /// </summary>
        IWriteableRemoveBlockOperation ToRemoveBlockOperation();
    }

    /// <summary>
    /// Operation details for inserting a block with a single node in a block list.
    /// </summary>
    public class WriteableInsertBlockOperation : WriteableInsertOperation, IWriteableInsertBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block insertion is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> where a block is inserted.</param>
        /// <param name="blockIndex">Index of the inserted block.</param>
        /// <param name="block">The inserted block.</param>
        /// <param name="node">The inserted node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableInsertBlockOperation(Node parentNode, string propertyName, int blockIndex, IBlock block, Node node, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            BlockIndex = blockIndex;
            Block = block;
            Node = node;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the block insertion is taking place.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Block list property of <see cref="ParentNode"/> where a block is inserted.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Index of the inserted block.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// The inserted block.
        /// </summary>
        public IBlock Block { get; }

        /// <summary>
        /// The inserted node.
        /// </summary>
        public Node Node { get; }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        public IWriteableBrowsingExistingBlockNodeIndex BrowsingIndex { get; private set; }

        /// <summary>
        /// Block state inserted.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }

        /// <summary>
        /// State inserted.
        /// </summary>
        public IWriteablePlaceholderNodeState ChildState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="browsingIndex">Index of the state after it's inserted.</param>
        /// <param name="blockState">Block state inserted.</param>
        /// <param name="childState">State inserted.</param>
        public virtual void Update(IWriteableBrowsingExistingBlockNodeIndex browsingIndex, IWriteableBlockState blockState, IWriteablePlaceholderNodeState childState)
        {
            Contract.RequireNotNull(browsingIndex, out IWriteableBrowsingExistingBlockNodeIndex BrowsingIndex);
            Contract.RequireNotNull(blockState, out IWriteableBlockState BlockState);
            Contract.RequireNotNull(childState, out IWriteablePlaceholderNodeState ChildState);

            this.BrowsingIndex = BrowsingIndex;
            this.BlockState = BlockState;
            this.ChildState = ChildState;
        }

        /// <summary>
        /// Creates an operation to undo the insert block operation.
        /// </summary>
        public virtual IWriteableRemoveBlockOperation ToRemoveBlockOperation()
        {
            return CreateRemoveBlockOperation(BlockIndex, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxRemoveBlockOperation object.
        /// </summary>
        private protected virtual IWriteableRemoveBlockOperation CreateRemoveBlockOperation(int blockIndex, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<WriteableInsertBlockOperation>());
            return new WriteableRemoveBlockOperation(ParentNode, PropertyName, blockIndex, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
