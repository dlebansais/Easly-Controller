﻿namespace EaslyController.Focus
{
    using System;
    using BaseNode;
    using EaslyController.Frame;
    using EaslyController.Writeable;

    /// <summary>
    /// Operation details for changing a discrete value.
    /// </summary>
    public interface IFocusChangeDiscreteValueOperation : IFrameChangeDiscreteValueOperation, IFocusOperation
    {
        /// <summary>
        /// State changed.
        /// </summary>
        new IFocusNodeState State { get; }
    }

    /// <summary>
    /// Operation details for changing a discrete value.
    /// </summary>
    internal class FocusChangeDiscreteValueOperation : FrameChangeDiscreteValueOperation, IFocusChangeDiscreteValueOperation
    {
        #region Init
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusChangeDiscreteValueOperation"/> class.
        /// </summary>
        /// <param name="parentNode">Node where the change is taking place.</param>
        /// <param name="propertyName">Name of the property to change.</param>
        /// <param name="value">The new value.</param>
        /// <param name="handlerRedo">Handler to execute to redo the operation.</param>
        /// <param name="handlerUndo">Handler to execute to undo the operation.</param>
        /// <param name="isNested">True if the operation is nested within another more general one.</param>
        public FocusChangeDiscreteValueOperation(INode parentNode, string propertyName, int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
            : base(parentNode, propertyName, value, handlerRedo, handlerUndo, isNested)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// State changed.
        /// </summary>
        public new IFocusNodeState State { get { return (IFocusNodeState)base.State; } }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates a IxxxChangeDiscreteValueOperation object.
        /// </summary>
        private protected override IWriteableChangeDiscreteValueOperation CreateChangeDiscreteValueOperation(int value, Action<IWriteableOperation> handlerRedo, Action<IWriteableOperation> handlerUndo, bool isNested)
        {
            ControllerTools.AssertNoOverride(this, typeof(FocusChangeDiscreteValueOperation));
            return new FocusChangeDiscreteValueOperation(ParentNode, PropertyName, value, handlerRedo, handlerUndo, isNested);
        }
        #endregion
    }
}
