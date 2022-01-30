namespace EaslyController.Layout
{
    using System;
    using EaslyController.Focus;
    using EaslyController.Writeable;

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public interface ILayoutOperation : IFocusOperation
    {
    }

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public class LayoutOperation : FocusOperation, ILayoutOperation
    {
        #region Init
        /// <summary>
        /// Gets the empty <see cref="LayoutOperation"/> object.
        /// </summary>
        public static new LayoutOperation Empty { get; } = new LayoutOperation();

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutOperation"/> class.
        /// </summary>
        protected LayoutOperation()
            : this((IWriteableOperation operation) => { }, (IWriteableOperation operation) => { })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        protected LayoutOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo)
            : this(handlerRedo, handlerUndo, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion
    }
}
