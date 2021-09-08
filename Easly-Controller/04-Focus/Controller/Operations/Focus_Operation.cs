namespace EaslyController.Focus
{
    using System;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public interface IFocusOperation : IFrameOperation
    {
    }

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public class FocusOperation : FrameOperation, IFocusOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
