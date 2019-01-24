using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public interface IFrameOperation : IWriteableOperation
    {
    }

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public class FrameOperation : WriteableOperation, IFrameOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FrameOperation"/> object.
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
