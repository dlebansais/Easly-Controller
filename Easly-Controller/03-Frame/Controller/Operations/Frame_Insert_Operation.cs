using EaslyController.Writeable;
using System;

namespace EaslyController.Frame
{
    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public interface IFrameInsertOperation : IWriteableInsertOperation, IFrameOperation
    {
    }

    /// <summary>
    /// Details for insertion operations.
    /// </summary>
    public abstract class FrameInsertOperation : WriteableInsertOperation, IFrameInsertOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="FrameInsertOperation"/> object.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameInsertOperation(Action<IWriteableOperation> handlerRedo, bool isNested)
            : base(handlerRedo, isNested)
        {
        }
        #endregion
    }
}
