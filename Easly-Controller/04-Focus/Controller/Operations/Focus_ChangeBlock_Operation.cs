using BaseNode;
using EaslyController.Frame;
using EaslyController.Writeable;
using System;

namespace EaslyController.Focus
{
    /// <summary>
    /// Operation details for changing a block.
    /// </summary>
    public interface IFocusChangeBlockOperation : IFrameChangeBlockOperation, IFocusOperation
    {
        /// <summary>
        /// Block state changed.
        /// </summary>
        new IFocusBlockState BlockState { get; }
    }

    /// <summary>
    /// Operation details for changing a node.
    /// </summary>
    public class FocusChangeBlockOperation : FrameChangeBlockOperation, IFocusChangeBlockOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of <see cref="FocusChangeBlockOperation"/>.
        /// </summary>
        /// <param name="parentNode">Node where the block change is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> for which a block is changed.</param>
        /// <param name="blockIndex">Index of the changed block.</param>
        /// <param name="replication">New replication value.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusChangeBlockOperation(INode parentNode, string propertyName, int blockIndex, ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, replication, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Block state changed.
        /// </summary>
        public new IFocusBlockState BlockState { get { return (IFocusBlockState)base.BlockState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeBlockOperation object.
        /// </summary>
        protected override IWriteableChangeBlockOperation CreateChangeBlockOperation(ReplicationStatus replication, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusChangeBlockOperation));
            return new FocusChangeBlockOperation(ParentNode, PropertyName, BlockIndex, replication, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
