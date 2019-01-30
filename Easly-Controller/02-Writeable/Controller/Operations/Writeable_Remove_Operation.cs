namespace EaslyController.Writeable
{
    using System;

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public interface IWriteableRemoveOperation : IWriteableOperation
    {
    }

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    internal abstract class WriteableRemoveOperation : WriteableOperation, IWriteableRemoveOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableRemoveOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableRemoveOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
