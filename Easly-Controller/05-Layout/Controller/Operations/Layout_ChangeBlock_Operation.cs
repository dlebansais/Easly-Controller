﻿namespace EaslyController.Layout
{
    using BaseNode;
    using EaslyController.Focus;
    using EaslyController.Writeable;
    using NotNullReflection;

    /// <summary>
    /// Operation details for changing a node.
    /// </summary>
    internal class LayoutChangeBlockOperation : FocusChangeBlockOperation, ILayoutOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutChangeBlockOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the block change is taking place.</param>
        /// <param name="propertyName">Block list property of <paramref name="parentNode"/> for which a block is changed.</param>
        /// <param name="blockIndex">Index of the changed block.</param>
        /// <param name="replication">New replication value.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public LayoutChangeBlockOperation(Node parentNode, string propertyName, int blockIndex, ReplicationStatus replication, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, blockIndex, replication, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Block state changed.
        /// </summary>
        public new ILayoutBlockState BlockState { get { return (ILayoutBlockState)base.BlockState; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeBlockOperation object.
        /// </summary>
        private protected override WriteableChangeBlockOperation CreateChangeBlockOperation(ReplicationStatus replication, System.Action<IWriteableOperation> handlerRedo, System.Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, Type.FromTypeof<LayoutChangeBlockOperation>());
            return new LayoutChangeBlockOperation(ParentNode, PropertyName, BlockIndex, replication, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
