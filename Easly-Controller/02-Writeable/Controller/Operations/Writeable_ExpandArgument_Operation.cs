namespace EaslyController.Writeable
{
    using System;
    using BaseNode;

    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    public interface IWriteableExpandArgumentOperation : IWriteableInsertBlockOperation
    {
    }

    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    internal class WriteableExpandArgumentOperation : WriteableInsertBlockOperation, IWriteableExpandArgumentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableExpandArgumentOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block insertion is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> where a block is inserted.</param>
        /// <param name="block">The inserted block.</param>
        /// <param name="node">The inserted item.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableExpandArgumentOperation(INode parentNode, string propertyName, IBlock block, INode node, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, 0, block, node, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
