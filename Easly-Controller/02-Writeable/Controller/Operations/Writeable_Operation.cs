namespace EaslyController.Writeable
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public interface IWriteableOperation
    {
        /// <summary>
        /// Handler to execute to redo the operation.
        /// </summary>
        Action<IWriteableOperation> HandlerRedo { get; }

        /// <summary>
        /// Handler to execute to undo the operation.
        /// </summary>
        Action<IWriteableOperation> HandlerUndo { get; }

        /// <summary>
        /// True if the operation is nested within another more general one.
        /// </summary>
        bool IsNested { get; }

        /// <summary>
        /// Execute the operation.
        /// </summary>
        void Redo();

        /// <summary>
        /// Undo the operation.
        /// </summary>
        void Undo();
    }

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    internal class WriteableOperation : IWriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteableOperation"/> class.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableOperation(Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            Debug.Assert(handlerRedo != null);
            Debug.Assert(handlerUndo != null);

            HandlerRedo = handlerRedo;
            HandlerUndo = handlerUndo;
            IsNested = isNested;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Handler to execute to redo the operation.
        /// </summary>
        public Action<IWriteableOperation> HandlerRedo { get; }

        /// <summary>
        /// Handler to execute to undo the operation.
        /// </summary>
        public Action<IWriteableOperation> HandlerUndo { get; }

        /// <summary>
        /// True if the operation is nested within another more general one.
        /// </summary>
        public bool IsNested { get; }
        #endregion

        #region Client Interface
        /// <summary>
        /// Execute the operation.
        /// </summary>
        public void Redo()
        {
            HandlerRedo(this);
        }

        /// <summary>
        /// Undo the operation.
        /// </summary>
        public void Undo()
        {
            HandlerUndo(this);
        }
        #endregion
    }
}
