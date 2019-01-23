using System;
using System.Diagnostics;

namespace EaslyController.Writeable
{
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
        /// True if the operation is nested within another more general one.
        /// </summary>
        bool IsNested { get; }
    }

    /// <summary>
    /// Base for all operations modifying the node tree.
    /// </summary>
    public class WriteableOperation : IWriteableOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of a <see cref="WriteableOperation"/> object.
        /// </summary>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableOperation(Action<IWriteableOperation> handlerRedo, bool isNested)
        {
            //Debug.Assert(handlerRedo != null);

            HandlerRedo = handlerRedo;
            IsNested = isNested;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Handler to execute to redo the operation.
        /// </summary>
        public Action<IWriteableOperation> HandlerRedo { get; }

        /// <summary>
        /// True if the operation is nested within another more general one.
        /// </summary>
        public bool IsNested { get; }
        #endregion
    }
}
