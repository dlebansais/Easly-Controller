﻿namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Operation details for inserting a node in a list or block list.
    /// </summary>
    public class WriteableInsertNodeOperation : WriteableInsertOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableInsertNodeOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the insertion is taking place.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where a node is inserted.</param>
        /// <param name="blockIndex">Block position where the node is inserted, if applicable.</param>
        /// <param name="index">Position where the node is inserted.</param>
        /// <param name="node">The inserted node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableInsertNodeOperation(Node parentNode, string propertyName, int blockIndex, int index, Node node, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            BlockIndex = blockIndex;
            Index = index;
            Node = node;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the insertion is taking place.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Property of <see cref="ParentNode"/> where a node is inserted.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Block position where the node is inserted, if applicable.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// Position where the node is inserted.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The inserted node.
        /// </summary>
        public Node Node { get; }

        /// <summary>
        /// Index of the state after it's inserted.
        /// </summary>
        public IWriteableBrowsingCollectionNodeIndex BrowsingIndex { get; private set; }

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
        /// <param name="childState">State inserted.</param>
        public virtual void Update(IWriteableBrowsingCollectionNodeIndex browsingIndex, IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(browsingIndex != null);
            Debug.Assert(childState != null);

            BrowsingIndex = browsingIndex;
            ChildState = childState;
        }

        /// <summary>
        /// Creates an operation to undo the insert operation.
        /// </summary>
        public virtual WriteableRemoveNodeOperation ToRemoveNodeOperation()
        {
            return CreateRemoveNodeOperation(BlockIndex, Index, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxRemoveNodeOperation object.
        /// </summary>
        private protected virtual WriteableRemoveNodeOperation CreateRemoveNodeOperation(int blockIndex, int index, Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableInsertNodeOperation));
            return new WriteableRemoveNodeOperation(ParentNode, PropertyName, blockIndex, index, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
