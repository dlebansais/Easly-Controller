namespace EaslyController.Layout
{
    using System;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    public interface ILayoutRemoveOperation : IFocusRemoveOperation, ILayoutOperation
    {
    }

    /// <summary>
    /// Details for removal operations.
    /// </summary>
    internal abstract class LayoutRemoveOperation : FocusRemoveOperation, ILayoutRemoveOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutRemoveOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutRemoveOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
