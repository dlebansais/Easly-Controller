namespace EaslyController.Frame
{
    using System;
    using EaslyController.Writeable;

    /// <inheritdoc/>
    public interface IFrameOperation : IWriteableOperation
    {
    }

    /// <inheritdoc/>
    public class FrameOperation : WriteableOperation, IFrameOperation
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="FrameOperation"/> object.
        /// </summary>
        public static new FrameOperation Empty { get; } = new FrameOperation();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameOperation"/> class.
        /// </summary>
        protected FrameOperation()
            : this((IWriteableOperation operation) => { }, (IWriteableOperation operation) => { })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        protected FrameOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo)
            : this(handlerRedo, handlerUndo, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
