using EaslyController.Frame;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public interface IFocusRemoveOperation : IFrameRemoveOperation, IFocusOperation
    {
    }

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public abstract class FocusRemoveOperation : FrameRemoveOperation, IFocusRemoveOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FocusRemoveOperation"/> object.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusRemoveOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
