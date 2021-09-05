namespace EaslyController.Focus
{
    using System;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public interface IFocusRemoveOperation : IFrameRemoveOperation
    {
    }

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    internal abstract class FocusRemoveOperation : FrameRemoveOperation, IFocusRemoveOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusRemoveOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusRemoveOperation(Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
