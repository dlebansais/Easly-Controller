namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    public interface IWriteableReplaceOperation : IWriteableOperation
    {
        /// <summary>
        /// Node where the replacement is taking place.
        /// </summary>
        INode ParentNode { get; }

        /// <summary>
        /// Property of <see cref="ParentNode"/> where the node is replaced.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Block position where the node is replaced, if applicable.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// Position where the node is replaced, if applicable.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// The new node. Null to clear an optional node.
        /// </summary>
        INode NewNode { get; }

        /// <summary>
        /// Index of the state before it's replaced.
        /// </summary>
        IWriteableBrowsingChildIndex OldBrowsingIndex { get; }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        IWriteableBrowsingChildIndex NewBrowsingIndex { get; }

        /// <summary>
        /// The old node.
        /// </summary>
        INode OldNode { get; }

        /// <summary>
        /// The new state.
        /// </summary>
        IWriteableNodeState NewChildState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="oldBrowsingIndex">Index of the state before it's replaced.</param>
        /// <param name="newBrowsingIndex">Index of the state after it's replaced.</param>
        /// <param name="oldNode">The old node. Can be null if optional and replaced.</param>
        /// <param name="newChildState">The new state.</param>
        void Update(IWriteableBrowsingChildIndex oldBrowsingIndex, IWriteableBrowsingChildIndex newBrowsingIndex, INode oldNode, IWriteableNodeState newChildState);

        /// <summary>
        /// Creates an operation to undo the replace operation.
        /// </summary>
        IWriteableReplaceOperation ToInverseReplace();
    }

    /// <summary>
    /// Operation details for replacing a node.
    /// </summary>
    public class WriteableReplaceOperation : WriteableOperation, IWriteableReplaceOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableReplaceOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the replacement is taking place.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where the node is replaced.</param>
        /// <param name="blockIndex">Block position where the node is replaced, if applicable.</param>
        /// <param name="index">Position where the node is replaced, if applicable.</param>
        /// <param name="newNode">The new node. Null to clear an optional node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableReplaceOperation(INode parentNode, string propertyName, int blockIndex, int index, INode newNode, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            BlockIndex = blockIndex;
            Index = index;
            NewNode = newNode;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the replacement is taking place.
        /// </summary>
        public INode ParentNode { get; }

        /// <summary>
        /// Property of <see cref="ParentNode"/> where the node is replaced.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Block position where the node is replaced, if applicable.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// Position where the node is replaced, if applicable.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The new node. Null to clear an optional node.
        /// </summary>
        public INode NewNode { get; }

        /// <summary>
        /// Index of the state before it's replaced.
        /// </summary>
        public IWriteableBrowsingChildIndex OldBrowsingIndex { get; private set; }

        /// <summary>
        /// Index of the state after it's replaced.
        /// </summary>
        public IWriteableBrowsingChildIndex NewBrowsingIndex { get; private set; }

        /// <summary>
        /// The old node.
        /// </summary>
        public INode OldNode { get; private set; }

        /// <summary>
        /// The new state.
        /// </summary>
        public IWriteableNodeState NewChildState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="oldBrowsingIndex">Index of the state before it's replaced.</param>
        /// <param name="newBrowsingIndex">Index of the state after it's replaced.</param>
        /// <param name="oldNode">The old node. Can be null if optional and replaced.</param>
        /// <param name="newChildState">The new state.</param>
        public virtual void Update(IWriteableBrowsingChildIndex oldBrowsingIndex, IWriteableBrowsingChildIndex newBrowsingIndex, INode oldNode, IWriteableNodeState newChildState)
        {
            Debug.Assert(oldBrowsingIndex != null);
            Debug.Assert(newBrowsingIndex != null);
            Debug.Assert(newChildState != null);

            OldBrowsingIndex = oldBrowsingIndex;
            NewBrowsingIndex = newBrowsingIndex;
            OldNode = oldNode;
            NewChildState = newChildState;
        }

        /// <summary>
        /// Creates an operation to undo the replace operation.
        /// </summary>
        public virtual IWriteableReplaceOperation ToInverseReplace()
        {
            return CreateReplaceOperation(BlockIndex, Index, OldNode, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxReplaceOperation object.
        /// </summary>
        private protected virtual IWriteableReplaceOperation CreateReplaceOperation(int blockIndex, int index, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableReplaceOperation));
            return new WriteableReplaceOperation(ParentNode, PropertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
