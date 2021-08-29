﻿namespace EaslyController.Frame
{
    using System;
    using BaseNode;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for changing text.
    /// </summary>
    public interface IFrameChangeTextOperation : IWriteableChangeTextOperation, IFrameOperation
    {
        /// <summary>
        /// State changed.
        /// </summary>
        new IFrameNodeState State { get; }
    }

    /// <summary>
    /// Operation details for changing text.
    /// </summary>
    internal class FrameChangeTextOperation : WriteableChangeTextOperation, IFrameChangeTextOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FrameChangeTextOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the change is taking place.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="text">The new text.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FrameChangeTextOperation(Node parentNode, string propertyName, string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, text, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State changed.
        /// </summary>
        public new IFrameNodeState State { get { return (IFrameNodeState)base.State; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeTextOperation object.
        /// </summary>
        private protected override IWriteableChangeTextOperation CreateChangeTextOperation(string text, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FrameChangeTextOperation));
            return new FrameChangeTextOperation(ParentNode, PropertyName, text, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
