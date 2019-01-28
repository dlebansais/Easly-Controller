using BaseNode;
using System;

namespace EaslyController.Writeable
{
    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    public interface IWriteableExpandArgumentOperation : IWriteableInsertBlockOperation
    {
        /// <summary>
        /// The inserted argument.
        /// </summary>
        IArgument Argument { get; }
    }

    /// <summary>
    /// Operation details for inserting a single argument in a block list.
    /// </summary>
    public class WriteableExpandArgumentOperation : WriteableInsertBlockOperation, IWriteableExpandArgumentOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="WriteableExpandArgumentOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block insertion is taking place.</param>
        /// <param name="block">The inserted block.</param>
        /// <param name="argument">The inserted argument.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public WriteableExpandArgumentOperation(IWriteableBlockListInner<IWriteableBrowsingBlockNodeIndex> inner, IBlock block, IArgument argument, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(inner, 0, block, argument, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// The inserted argument.
        /// </summary>
        public IArgument Argument { get { return (IArgument)Node; } }
        #endregion
    }
}
