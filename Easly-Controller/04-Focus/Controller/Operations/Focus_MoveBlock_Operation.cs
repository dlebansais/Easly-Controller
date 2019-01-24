using EaslyController.Frame;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for moving a block in a block list.
    /// </summary>
    public interface IFocusMoveBlockOperation : IFrameMoveBlockOperation, IFocusOperation
    {
        /// <summary>
        /// Inner where the block is moved.
        /// </summary>
        new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get; }

        /// <summary>
        /// The moved block state.
        /// </summary>
        new IFocusBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for moving a block in a block list.
    /// </summary>
    public class FocusMoveBlockOperation : FrameMoveBlockOperation, IFocusMoveBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusMoveBlockOperation"/>.
        /// </summary>
        /// <param name="inner">Inner where the block is move.</param>
        /// <param name="blockIndex">Index of the moved block.</param>
        /// <param name="direction">The change in position, relative to the current position.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusMoveBlockOperation(IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> inner, int blockIndex, int direction, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(inner, blockIndex, direction, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Inner where the block is moved.
        /// </summary>
        public new IFocusBlockListInner<IFocusBrowsingBlockNodeIndex> Inner { get { return (IFocusBlockListInner<IFocusBrowsingBlockNodeIndex>)base.Inner; } }

        /// <summary>
        /// The moved block state.
        /// </summary>
        public new IFocusBlockState BlockState { get { return (IFocusBlockState)base.BlockState; } }
        #endregion
    }
}
