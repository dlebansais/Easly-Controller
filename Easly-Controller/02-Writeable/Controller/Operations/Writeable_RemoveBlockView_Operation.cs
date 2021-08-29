namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;
    using BaseNode;

    /// <summary>
    /// Operation details for updating a view after block has been removed from a block list.
    /// </summary>
    public interface IWriteableRemoveBlockViewOperation : IWriteableRemoveOperation
    {
        /// <summary>
        /// Node where the block removal is taking place.
        /// </summary>
        Node ParentNode { get; }

        /// <summary>
        /// Block list property of <see cref="ParentNode"/> where a block is removed.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// Index of the removed block.
        /// </summary>
        int BlockIndex { get; }

        /// <summary>
        /// Block state removed.
        /// </summary>
        IWriteableBlockState BlockState { get; }

        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state removed.</param>
        void Update(IWriteableBlockState blockState);
    }

    /// <summary>
    /// Operation details for updating a view after block has been removed from a block list.
    /// </summary>
    internal class WriteableRemoveBlockViewOperation : WriteableRemoveOperation, IWriteableRemoveBlockViewOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableRemoveBlockViewOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block removal is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> where a block is removed.</param>
        /// <param name="blockIndex">index of the removed block.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableRemoveBlockViewOperation(Node parentNode, string propertyName, int blockIndex, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
            ParentNode = parentNode;
            PropertyName = propertyName;
            BlockIndex = blockIndex;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Node where the block removal is taking place.
        /// </summary>
        public Node ParentNode { get; }

        /// <summary>
        /// Block list property of <see cref="ParentNode"/> where a block is removed.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Index of the removed block.
        /// </summary>
        public int BlockIndex { get; }

        /// <summary>
        /// Block state removed.
        /// </summary>
        public IWriteableBlockState BlockState { get; private set; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Update the operation with details.
        /// </summary>
        /// <param name="blockState">Block state removed.</param>
        public virtual void Update(IWriteableBlockState blockState)
        {
            Debug.Assert(blockState != null);

            BlockState = blockState;
        }
        #endregion
    }
}
