namespace EaslyController.Writeable
{
    using System;

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public abstract class WriteableRemoveOperation : WriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableRemoveOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableRemoveOperation(Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
