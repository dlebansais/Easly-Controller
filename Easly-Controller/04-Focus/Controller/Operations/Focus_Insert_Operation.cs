namespace EaslyController.Focus
{
    using System;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public interface IFocusInsertOperation : IFrameInsertOperation
    {
    }

    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    internal abstract class FocusInsertOperation : FrameInsertOperation, IFocusInsertOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusInsertOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusInsertOperation(Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
