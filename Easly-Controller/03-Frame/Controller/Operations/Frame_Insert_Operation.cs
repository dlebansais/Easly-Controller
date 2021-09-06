namespace EaslyController.Frame
{
    using System;
    using EaslyController.Writeable;

    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public interface IFrameInsertOperation : IFrameOperation
    {
    }

    /// <inheritdoc/>
    internal abstract class FrameInsertOperation : WriteableInsertOperation, IFrameInsertOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameInsertOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameInsertOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
