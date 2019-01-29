using BaseNode;
using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public interface IWriteableRemoveNodeOperation : IWriteableRemoveOperation
    {
        /// <summary>
        /// Node where the removal is taking place.
        /// </summary>
        INode ParentNode { get; }

        /// <summary>
        /// Property of <see cref="ParentNode"/> where a node is removed.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Block position where the node is removed, if applicable.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// Position of the removed node.
        /// </summary>
        int Index { get; }

        /// <summary>
        /// The removed state.
        /// </summary>
        IWriteablePlaceholderNodeState RemovedState { get; }

        /// <summary>
        /// The removed node.
        /// </summary>
        INode RemovedNode { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="childState">State removed.</param>
        void Update(IWriteablePlaceholderNodeState childState);

        /// <summary>
        /// Creates an operation to undo the remove operation.
        /// </summary>
        IWriteableInsertNodeOperation ToInsertNodeOperation();
    }

    /// <summary>
    /// Operation details for removing a node in a list or block list.
    /// </summary>
    public class WriteableRemoveNodeOperation : WriteableRemoveOperation, IWriteableRemoveNodeOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableRemoveNodeOperation"/>.
        /// </summary>
        /// <param name="parentNode">Node where the removal is taking place.</param>
        /// <param name="propertyName">Property of <paramref name="parentNode"/> where a node is removed.</param>
        /// <param name="blockIndex">Block position where the node is removed, if applicable.</param>
        /// <param name="index">Position of the removed node.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableRemoveNodeOperation(INode parentNode, string propertyName, int blockIndex, int index, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
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
        public INode ParentNode { get; }

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
        public INode RemovedNode { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="childState">State removed.</param>
        public virtual void Update(IWriteablePlaceholderNodeState childState)
        {
            Debug.Assert(childState != null);

            RemovedState = childState;
            RemovedNode = childState.Node;
        }

        /// <summary>
        /// Creates an operation to undo the remove operation.
        /// </summary>
        public virtual IWriteableInsertNodeOperation ToInsertNodeOperation()
        {
            return CreateInsertNodeOperation(BlockIndex, Index, RemovedNode, HandlerUndo, HandlerRedo, IsNested);
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxInsertNodeOperation object.
        /// </summary>
        protected virtual IWriteableInsertNodeOperation CreateInsertNodeOperation(int blockIndex, int index, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(WriteableRemoveNodeOperation));
            return new WriteableInsertNodeOperation(ParentNode, PropertyName, blockIndex, index, node, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
