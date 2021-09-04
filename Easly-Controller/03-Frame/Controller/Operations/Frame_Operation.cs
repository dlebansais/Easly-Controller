namespace EaslyController.Frame
{
    using System;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    internal class FrameOperation : WriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameOperation(Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
