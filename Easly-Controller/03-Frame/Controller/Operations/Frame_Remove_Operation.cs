namespace EaslyController.Frame
{
    using System;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    internal abstract class FrameRemoveOperation : WriteableRemoveOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameRemoveOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameRemoveOperation(Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
