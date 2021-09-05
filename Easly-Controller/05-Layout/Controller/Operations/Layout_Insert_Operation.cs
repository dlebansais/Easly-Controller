namespace EaslyController.Layout
{
    using System;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public interface ILayoutInsertOperation : IFocusInsertOperation
    {
    }

    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    internal abstract class LayoutInsertOperation : FocusInsertOperation, ILayoutInsertOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutInsertOperation(Action<WriteableOperation> handlerRedo, Action<WriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
